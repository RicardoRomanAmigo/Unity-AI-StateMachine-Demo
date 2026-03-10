using UnityEngine;
using System;


public class EnemyController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnemyMovement enemyMovement;
    [SerializeField] private EnemyAttack enemyAttack;
    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private GameObject player;
    [SerializeField] private EnemyAnimation enemyAnimation;
    [SerializeField] private GameObject enemybones;

    [Header("LayerMasks")]
    [SerializeField] private LayerMask visionBlockingLayers;

    public EnemyMovement EnemyMovementRef => enemyMovement;
    public EnemyAttack EnemyAttackRef => enemyAttack;
    public EnemyHealth EnemyHealthRef => enemyHealth;
    public EnemyAnimation EnemyAnimationRef => enemyAnimation;

    public GameObject Player => player;


    //"State references"
    private EnemyIdleState idleState;
    private EnemyPatrolState patrolState;
    private EnemyChaseState chaseState;
    private EnemyReturnToPatrolState returnToPatrolState;
    private EnemyAttackState attackState;
    private EnemyStunnedState stunnedState;
    private EnemyDeadState deadState;

    [Header("Stats")]
    [SerializeField] private EnemyStats enemyStats;

    [Header("PathRef")]
    [SerializeField] private Transform[] patrolPoints;


    public EnemyStats EnemyStats => enemyStats;

    public Transform[] PatrolPoints => patrolPoints;

    private Vector3[] patrolPositions;

    public Vector3[] PatrolPositions => patrolPositions;

    private EnemyState currentState;
    private EnemyState previousState;

    public event Action<EnemyController> OnEnemyDefeated;

    private void Awake()
    {
        if(enemyMovement == null) enemyMovement = GetComponent<EnemyMovement>();
        if(enemyAttack == null) enemyAttack = GetComponent<EnemyAttack>();
        if(enemyHealth == null) enemyHealth = GetComponent<EnemyHealth>();

        idleState = new EnemyIdleState(this);
        patrolState = new EnemyPatrolState(this);
        chaseState = new EnemyChaseState(this);
        returnToPatrolState = new EnemyReturnToPatrolState(this);
        attackState = new EnemyAttackState(this);
        stunnedState = new EnemyStunnedState(this);
        deadState = new EnemyDeadState(this);

        //Initialize stats 
        if (enemyStats != null) InitializeStats();
         else Debug.LogError("Enemy stats not assigned in the inspector.");

        // Start in idle state
        currentState = idleState;
        currentState.EnterState();
    }

    private void Start()
    {
        PatrolPointsConverssion();
    }

    private void PatrolPointsConverssion()
    {
        // Convert patrol points to positions for easier use in states
        if (patrolPoints != null && patrolPoints.Length > 0)
        {
            patrolPositions = new Vector3[patrolPoints.Length];
            for (int i = 0; i < patrolPoints.Length; i++)
            {
                patrolPositions[i] = patrolPoints[i].position;
            }
        }
        else
        {
            Debug.LogWarning("No patrol points assigned. Patrol state will not function properly.");
        }
    }

    public void ChangeState(EnemyState state)
    {
        if (state == null)
        {
            Debug.LogError("Trying to change to a null state.");
            return;
        }

        if (state == currentState) return; 

        previousState = currentState;

        previousState.ExitState();
        currentState = state;
        currentState.EnterState();
    }

    private void InitializeStats()
    {
        if(enemyStats == null)
        {
            Debug.LogError("Enemy stats not assigned in the inspector.");
            return;
        }
        if (enemyHealth != null) enemyHealth.Initialize(enemyStats.maxHealth);
        if (enemyMovement != null) enemyMovement.Initialize(enemyStats);

    }

    // State transition methods for external calls

    public void TransitionToIdle() => ChangeState(idleState);
    public void TransitionToPatrol() => ChangeState(patrolState);
    public void TransitionToChase() => ChangeState(chaseState);
    public void TransitionReturnToPatrol() => ChangeState(returnToPatrolState);
    public void TransitionToAttack() => ChangeState(attackState);
    public void TransitionToStunned() => ChangeState(stunnedState);
    public void TransitionToDead() => ChangeState(deadState);

    public void RevertToPreviousState()
    {
        if (previousState != null)
        {
            if (previousState == currentState)
            {
                Debug.LogWarning("Previous state is the same as current state. No state change will occur.");
                return;
            }
            ChangeState(previousState);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (currentState != null) currentState.UpdateState();
    }

    public bool ShouldReturnToPatrol()
    {
        Vector3 nearestPatrolPosition = GetNearestPatrolPoint(); 
        if (nearestPatrolPosition == null) return false;

        float distance = Vector2.Distance(transform.position, nearestPatrolPosition);
        return distance > enemyStats.maxChaseRange;
    }

    public Vector3 GetNearestPatrolPoint()
    {
        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            return transform.position;
        }

        var currentPatrolTarget = patrolPositions[0];
        var currentPatrolIndex = 0; 

        for (int i = 0; i < patrolPositions.Length; i++)
        {
            if (Vector3.Distance(transform.position, patrolPositions[i]) < Vector3.Distance(transform.position, currentPatrolTarget))
            {
                currentPatrolIndex = i;
                currentPatrolTarget = patrolPositions[currentPatrolIndex];
            }
        }

        return currentPatrolTarget;
    }

    public bool HasLineOfSight()
    {
        if (player == null) return false;

        Vector2 origin = transform.position;
        Vector2 target = player.transform.position;

        RaycastHit2D hit = Physics2D.Linecast(origin, target, visionBlockingLayers);

        return hit.collider == null;
    }

    public void DestroyEnemy()
    {
        

        var spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false; // Hide the enemy's sprite
        }

        var collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false; // Disable the enemy's collider
        }

        Instantiate(enemybones, transform.position, Quaternion.identity);

        gameObject.SetActive(false);

        // Notify ZoneController or GameManager that this enemy has been defeated
        OnEnemyDefeated?.Invoke(this);

        Destroy(gameObject);
    }



    // -------------------------------------Gizmos-------------------------------------

    // Gizmos for visualization in the editor
    private void OnDrawGizmosSelected()
    {
        if (patrolPoints != null)
        {
            Gizmos.color = Color.blue;
            foreach (var point in patrolPoints)
            {
                Gizmos.DrawSphere(point.position, 0.2f);
            }
        }
        if (enemyStats != null)
        {
            // Draw maxchase radius
            Gizmos.color = Color.blue;
            for(var i = 0; i < patrolPoints.Length; i++)
            {
                Gizmos.DrawWireSphere(patrolPoints[i].position, enemyStats.maxChaseRange);
            }
            // Draw chase radius
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, enemyStats.chaseRange);
            // Draw attack radius
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, enemyStats.attackRange);
        }
    }
}

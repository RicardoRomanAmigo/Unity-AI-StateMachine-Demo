using UnityEngine;



public class EnemyController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnemyMovement enemyMovement;
    [SerializeField] private EnemyAttack enemyAttack;
    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private GameObject player;
    [SerializeField] private EnemyAnimation enemyAnimation;

    public EnemyMovement EnemyMovementRef => enemyMovement;
    public EnemyAttack EnemyAttackRef => enemyAttack;
    public EnemyHealth EnemyHealthRef => enemyHealth;
    public EnemyAnimation EnemyAnimationRef => enemyAnimation;

    public GameObject Player => player;


    //"State references"
    private EnemyIdleState idleState;
    private EnemyPatrolState patrolState;
    private EnemyChaseState chaseState;
    private EnemyAttackState attackState;
    private EnemyStunnedState stunnedState;
    private EnemyDeadState deadState;

    [Header("Stats")]
    [SerializeField] private EnemyStats enemyStats;

    [Header("PathRef")]
    [SerializeField] private Transform[] patrolPoints;


    public EnemyStats EnemyStats => enemyStats;

    public Transform[] PatrolPoints => patrolPoints;

    private EnemyState currentState;
    private EnemyState previousState;

    private void Awake()
    {
        if(enemyMovement == null) enemyMovement = GetComponent<EnemyMovement>();
        if(enemyAttack == null) enemyAttack = GetComponent<EnemyAttack>();
        if(enemyHealth == null) enemyHealth = GetComponent<EnemyHealth>();

        idleState = new EnemyIdleState(this);
        patrolState = new EnemyPatrolState(this);
        chaseState = new EnemyChaseState(this);
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

    public void ChangeState(EnemyState state)
    {
        if(state == null)
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

    }

    // State transition methods for external calls

    public void TransitionToIdle() => ChangeState(idleState);
    public void TransitionToPatrol() => ChangeState(patrolState);
    public void TransitionToChase() => ChangeState(chaseState);
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

    public void DestroyEnemy()
    {
       Destroy(gameObject);
    }
}

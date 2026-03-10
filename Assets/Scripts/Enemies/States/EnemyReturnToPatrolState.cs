using UnityEngine;

public class EnemyReturnToPatrolState : EnemyState
{

    private EnemyController enemyController;

    private int currentPatrolIndex = 0;
    private Vector3 currentPatrolTarget;

    public EnemyReturnToPatrolState(EnemyController controller)
    {
        enemyController = controller;
    }
    
    public override void EnterState()
    {
        if (enemyController.PatrolPositions == null || enemyController.PatrolPositions.Length == 0) return;

        currentPatrolIndex = Mathf.Clamp(currentPatrolIndex, 0, enemyController.PatrolPositions.Length - 1);
        currentPatrolTarget = enemyController.GetNearestPatrolPoint();
        //currentPatrolTarget = enemyController.PatrolPositions[currentPatrolIndex];

        /*for(int i = 0; i < enemyController.PatrolPositions.Length; i++)
        {
            if(Vector3.Distance(enemyController.transform.position, enemyController.PatrolPositions[i]) < Vector3.Distance(enemyController.transform.position, currentPatrolTarget))
            {
                currentPatrolIndex = i;
                currentPatrolTarget = enemyController.PatrolPositions[currentPatrolIndex];
            }
        }*/


    }

    public override void UpdateState()
    {
        if (currentPatrolTarget == null)
            return;

        var distance = Vector3.Distance(enemyController.transform.position, currentPatrolTarget);

        if (distance > 0.1f)
        {
            enemyController.EnemyMovementRef.MovePatrol(currentPatrolTarget);
        }
        else
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % enemyController.PatrolPoints.Length;
            currentPatrolTarget = enemyController.PatrolPositions[currentPatrolIndex];
            enemyController.TransitionToIdle();
        }

        if (enemyController == null || enemyController.Player == null) return;

        if (Vector3.Distance(enemyController.transform.position, enemyController.Player.transform.position) < enemyController.EnemyStats.chaseRange && enemyController.HasLineOfSight())
        {
            enemyController.TransitionToChase();
        }
    }


    public override void ExitState()
    {
        
    }

    
    
}

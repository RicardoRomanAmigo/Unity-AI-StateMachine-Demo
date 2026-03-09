using UnityEngine;

public class EnemyPatrolState : EnemyState
{
    private EnemyController enemyController;
    public EnemyPatrolState (EnemyController controller)
    {
        enemyController = controller;
    }

    private int currentPatrolIndex = 0;
    private Vector3 currentPatrolTarget;
    private Vector3[] patrolPositions;
    
    public override void EnterState()
    {
        if (enemyController.PatrolPositions == null || enemyController.PatrolPositions.Length == 0) return;

        currentPatrolIndex = Mathf.Clamp(currentPatrolIndex, 0, enemyController.PatrolPositions.Length - 1);
        currentPatrolTarget = enemyController.PatrolPositions[currentPatrolIndex];
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
    }

    public override void ExitState()
    {
        
    }
}

using UnityEngine;

public class EnemyPatrolState : EnemyState
{
    private EnemyController enemyController;
    public EnemyPatrolState (EnemyController controller)
    {
        enemyController = controller;
    }

    private int currentPatrolIndex = 0;
    private Transform currentPatrolTarget;
    
    public override void EnterState()
    {
        if (enemyController.PatrolPoints != null) currentPatrolTarget = enemyController.PatrolPoints[currentPatrolIndex];
    }

    public override void UpdateState()
    {

            var distance = Vector3.Distance(enemyController.transform.position, currentPatrolTarget.position);
            if (distance > 0.1f)
            {
                enemyController.EnemyMovementRef.MovePatrol(currentPatrolTarget);
            }
            else
            {
                currentPatrolIndex = (currentPatrolIndex + 1) % enemyController.PatrolPoints.Length;
                currentPatrolTarget = enemyController.PatrolPoints[currentPatrolIndex];
                enemyController.TransitionToIdle();
            } 
    }

    public override void ExitState()
    {
        
    }
}

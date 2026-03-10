using UnityEngine;

public class EnemyIdleState : EnemyState
{
    //Refernces
    private EnemyController enemyController;

    private float idleTimer;

    public EnemyIdleState(EnemyController controller)
    {
        enemyController = controller;
    }

    public override void EnterState()
    {
        idleTimer = 0f;
        enemyController.EnemyAnimationRef.IdleAnim(true);
    }

    public override void UpdateState()
    {
        if (idleTimer < enemyController.EnemyStats.patrolWaitTime)
        {
            idleTimer += Time.deltaTime;
        }
        else
        {
            enemyController.EnemyAnimationRef.IdleAnim(false);
            enemyController.TransitionToPatrol();
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

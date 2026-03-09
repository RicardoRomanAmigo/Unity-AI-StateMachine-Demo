using UnityEngine;

public class EnemyDeadState : EnemyState
{
    //"References"
    private EnemyController enemyController;

    private float deathTimer;

    public EnemyDeadState(EnemyController controller)
    {
        enemyController = controller;
    }

    public override void EnterState()
    {
        enemyController.EnemyAnimationRef.DieAnim();
        deathTimer = enemyController.EnemyStats.timeToDestroy;
    }

    public override void UpdateState()
    {
        if (deathTimer <= 0)
        {
            enemyController.DestroyEnemy();
        }
        else
        {
            deathTimer -= Time.deltaTime;
        }
    }

    public override void ExitState()
    {
        
    }
}

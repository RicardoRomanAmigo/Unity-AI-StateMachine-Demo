using UnityEngine;

public class EnemyStunnedState : EnemyState
{
    //"References"
    private EnemyController enemyController;

    private float stunDuration;

    public EnemyStunnedState(EnemyController controller)
    {
        enemyController = controller;
    }

    public override void EnterState()
    {
        stunDuration = enemyController.EnemyStats.stunedDuration;
        enemyController.EnemyAnimationRef.StunnedAnim();
    }

    public override void UpdateState()
    {
        if (stunDuration <= 0 )
        {
            if(enemyController.EnemyHealthRef.CurrentHealth > 0)
            {
                enemyController.RevertToPreviousState();
            }
            else
            {
                enemyController.TransitionToDead();
            }
            
        }
        else
        {
            stunDuration -= Time.deltaTime;
        }
    }

    public override void ExitState()
    {
        
    }

}

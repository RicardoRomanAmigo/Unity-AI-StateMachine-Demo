using UnityEngine;

public class EnemyChaseState : EnemyState
{
    //"References"
    private EnemyController enemyController;

    public EnemyChaseState(EnemyController controller)
    {
        enemyController = controller;
    }

    public override void EnterState()
    {
        if (enemyController == null)
        {
            Debug.LogError("EnemyController reference is null in EnemyChaseState.");
            return;
        }

        enemyController.EnemyMovementRef.MoveChase();
    }

    public override void UpdateState()
    {
        if(enemyController == null || enemyController.Player == null)
        {
            Debug.LogError("EnemyController reference is null in EnemyChaseState.");
            return;
        }
        if(Vector3.Distance(enemyController.transform.position, enemyController.Player.transform.position) > enemyController.EnemyStats.chaseRange)
        {
            //Out of chase range, switch back to patrol
            enemyController.TransitionReturnToPatrol();
        }
        else if (enemyController.ShouldReturnToPatrol())
        {
            enemyController.TransitionReturnToPatrol();
        }
        else if (Vector3.Distance(enemyController.transform.position, enemyController.Player.transform.position) <= enemyController.EnemyStats.attackRange)
        {
            //Within attack range, switch to attack state
            enemyController.TransitionToAttack();
        }

            enemyController.EnemyMovementRef.MoveChase();
    }

    public override void ExitState()
    {
        
    }

    
}

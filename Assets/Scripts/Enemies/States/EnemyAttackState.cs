using UnityEngine;

public class EnemyAttackState : EnemyState
{
    //"References"
    private EnemyController enemyController;
    
    private float currentCoolDown;

    public EnemyAttackState(EnemyController controller)
    {
        enemyController = controller;
    }

    public override void EnterState()
    {
        currentCoolDown = enemyController.EnemyStats.attackCoolDown;
    }

    public override void UpdateState()
    {
        var distanceToPlayer = Vector3.Distance(enemyController.transform.position, enemyController.Player.transform.position);

        if (distanceToPlayer < enemyController.EnemyStats.attackRange)
        {
            if (currentCoolDown <= 0)
            {
                enemyController.EnemyAnimationRef.AttackAnim();
                enemyController.EnemyAttackRef.Attack();
                currentCoolDown = enemyController.EnemyStats.attackCoolDown;
            }

            currentCoolDown -= Time.deltaTime;

        }
        else
        {
            enemyController.TransitionToChase();
        }
    }

    public override void ExitState()
    {
        
    } 
}

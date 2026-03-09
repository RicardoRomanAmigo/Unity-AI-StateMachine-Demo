using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnemyController enemyController;

    public void Attack()
    {
        if (enemyController.Player != null)
        {
            enemyController.Player.GetComponent<PlayerHealth>().TakeDamage(enemyController.EnemyStats.damage);   
        }
    }
}

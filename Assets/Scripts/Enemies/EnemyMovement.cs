using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // References
    [SerializeField] private EnemyController enemyController;

    private EnemyStats currentEnemyStats;
    private float speed;
    private float patrolSpeed;
    
    public void Initialize(EnemyStats stats)
    {
        currentEnemyStats = stats;
        speed = currentEnemyStats.speed;
        patrolSpeed = currentEnemyStats.patrolSpeed;
        
    }

    public void MovePatrol(Transform target)
    {
        var direction = (target.position - transform.position).normalized;
        transform.position += direction * patrolSpeed * Time.deltaTime;
    }

    public void MoveChase()
    {     
        var playerPosition = enemyController.Player.transform.position;
        var direction = (playerPosition - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime; 
    }


}

using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "Scriptable Objects/EnemyStats")]
public class EnemyStats : ScriptableObject
{
    public float maxHealth;
    public float speed;
    public float chaseRange;
    public float maxChaseRange;
    public float attackRange;
    public float damage;
    public float attackCoolDown;
    public float patrolSpeed;
    public float patrolWaitTime;
    public float stunedDuration;
    public float timeToDestroy;
}

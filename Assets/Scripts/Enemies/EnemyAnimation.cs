using UnityEngine;


public class EnemyAnimation : MonoBehaviour
{
    [Header("Refernces")]
    [SerializeField] private EnemyController enemyController;
    [SerializeField] private Animator animator;

    public void IdleAnim(bool state)
    {
        animator.SetBool("Idle", state);
    }

    public void PatrolAnim(bool state)
    {
       animator.SetBool("Walking", state);
    }

    public void ChaseAnim(bool state)
    {
        animator.SetBool("Walking", state);
    }
    
    public void AttackAnim()
    {
        animator.SetTrigger("Attack");
    }
    public void StunnedAnim()
    {
        animator.SetTrigger("Stunned");
    }

    public void DieAnim()
    {
        animator.SetTrigger("Die");
    }
}

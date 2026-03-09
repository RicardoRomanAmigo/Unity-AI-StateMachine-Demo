using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [Header("Refernces")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Animator animator;

    public void IdleAnim(bool state)
    {
        animator.SetBool("Idle", state);
    }

    public void WalkAnim(bool state)
    {
        animator.SetBool("Walking", state);
    }

    public void AttackAnim()
    {
        animator.SetTrigger("Attack");
    }
    public void HittedAnim()
    {
        animator.SetTrigger("Hitted");
    }

    public void DieAnim()
    {
        animator.SetTrigger("Die");
    }
}

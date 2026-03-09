using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Refereces")]
    [SerializeField] private PlayerAnimations playerAnimations;
    //Player stats
    private float moveSpeed = 3f;

    public void Move(Vector2 direction)
    {
        transform.Translate(direction * Time.deltaTime * moveSpeed);
    }
}

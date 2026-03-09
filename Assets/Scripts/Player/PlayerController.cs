using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour 
{
    [Header("References")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerAttack playerAttack;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private GameObject projectile;
    [SerializeField] private PlayerAnimations playerAnimations;

    public GameObject Projectile { get => projectile; set => projectile = value; }

    private Vector2 moveInput;

    public static PlayerController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        
    }

    public void OnAttack(InputValue value)
    {
        if (value.isPressed)
        {
            playerAttack.Attack();
        }
    }

    private void Update()
    {
        playerMovement.Move(moveInput);

        bool isMoving = moveInput.sqrMagnitude > 0.01f;
        playerAnimations.WalkAnim(isMoving);
    }
}

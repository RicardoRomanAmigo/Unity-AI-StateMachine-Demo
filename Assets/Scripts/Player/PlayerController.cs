using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour 
{
    [Header("References")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerAttack playerAttack;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private GameObject projectile;

    public GameObject Projectile { get => projectile; set => projectile = value; }

    private Vector2 moveInpt;

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

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInpt = context.ReadValue<Vector2>();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            playerAttack.Attack();
        }
    }

    private void Update()
    {
        playerMovement.Move(moveInpt);
    }
}

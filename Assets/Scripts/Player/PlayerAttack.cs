using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerAttack : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Transform projectileOrigin;
    // Player attack Stats
    private float cooldownTime = 0;
    private Vector2 aimDirection;

    public void Attack()
    {
        if (cooldownTime > 0f) return;
        
        cooldownTime = 0.5f;

        var projectile = Instantiate(PlayerController.Instance.Projectile, projectileOrigin.position, Quaternion.identity);

        ProjectileController projectileController = projectile.GetComponent<ProjectileController>();

        projectileController.SetProjectileStats();
        projectileController.ProjectileOwner = gameObject;
        projectileController.Direction = aimDirection; // Set the direction of the projectile (example: right)
       
    }

    private void Update()
    {
        if (cooldownTime > 0f)
            cooldownTime -= Time.deltaTime;

        Vector3 mouseScreen = Mouse.current.position.ReadValue();
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);

        Vector2 dir = mouseWorld - transform.position;
        aimDirection = dir.normalized;
    }
}

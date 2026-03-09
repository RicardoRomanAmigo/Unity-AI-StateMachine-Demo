using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerAttack : MonoBehaviour
{
    // Player attack Stats
    private float cooldownTime = 0.5f;
    private Vector2 aimDirection;

    public void Attack()
    {
        if (cooldownTime <= 0f)
        {
            var projectile = Instantiate(PlayerController.Instance.Projectile, transform.position, Quaternion.identity);

            ProjectileController projectileController = projectile.GetComponent<ProjectileController>();

            projectileController.SetProjectileStats();
            projectileController.ProjectileOwner = gameObject;

            projectileController.Direction = aimDirection; // Set the direction of the projectile (example: right)
        }
        else
        {
            cooldownTime -= Time.deltaTime;
        }
        
    }

    private void Update()
    {
        Vector3 mouseScreen = Mouse.current.position.ReadValue();
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);

        aimDirection = (mouseWorld - transform.position).normalized;
    }
}

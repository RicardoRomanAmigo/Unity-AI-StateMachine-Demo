using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public void TakeDamage(float damage)
    {
        // Implement damage logic here, such as reducing health and checking for death
        Debug.Log($"Player takes {damage} damage.");
    }
}

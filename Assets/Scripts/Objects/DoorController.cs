using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Collider2D doorSensor;
    [SerializeField] private Collider2D doorLook;
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private AudioClip doorSound;

    private bool isOpen = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isOpen) return;
        if (!other.CompareTag("Player")) return;
         
        if (PlayerController.Instance.HasKey)
        {
            isOpen = true;
            doorAnimator.SetTrigger("Open");
            AudioManager.Instance.PlaySFX(doorSound);
            doorSensor.enabled = false;
            doorLook.enabled = false;

            CallNextZone();
        } 
    }

    private void CallNextZone()
    {
        GameManager.Instance.ChangeZone();
    }
}

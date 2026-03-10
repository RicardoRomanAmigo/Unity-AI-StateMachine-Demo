using UnityEngine;

public enum PickupType
{
    Key,
    //Life,
    Health,
    //Projectile
}

public class PickupController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Collider2D triggerCollider;
    [SerializeField] private Animator animator;
    [SerializeField] private PickupData data;

    private bool canBeCollected;
    private bool collected;

    private void Start()
    {
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        if (triggerCollider == null) triggerCollider = GetComponent<Collider2D>();
        if (animator == null) animator = GetComponent<Animator>();
        SetUp(data);
        PlaySpawnAnimation();
    }

    public void SetUp(PickupData pickupData)
    {
       data = pickupData;
       spriteRenderer.sprite = data.sprite;
       canBeCollected = false;
       collected = false;
    }

    //Animation end Enable
    public void EnableCollection()
    {
        canBeCollected = true;
    }

    private void PlaySpawnAnimation()
    {
        animator.Play("Spawn");
    }

    public void PlaySpawnSound()
    {
       if (data.pickupSound != null) AudioManager.Instance.PlaySFX(data.pickupSound);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!canBeCollected || collected) return;
        if (!other.CompareTag("Player")) return;

        Collect(other.gameObject);
    }

    private void Collect(GameObject other)
    {
        collected = true;
        triggerCollider.enabled = false;
        PlayerController player = other.GetComponent<PlayerController>();
        player.Pickup(data);
        if(data.pickupSound != null)
        {
            PlaySpawnAnimation();
        }
        animator.Play("Collect");
    }

    //Animation End
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}

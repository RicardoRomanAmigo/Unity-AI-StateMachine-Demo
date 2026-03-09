
using UnityEngine;
using System.Collections;

public class ProjectileController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Collider2D myCollider;

    [Header("Projectile Stats")]
    [SerializeField] ProjectileStats projectileStats;

    //Projctile stats
    private float speed;
    private float timeToDestroy = 1.5f;
    private float lifeTime;
    private float damage;
    private GameObject hitAnimationGO;
    private AudioClip hitSound;
    //private string ownerTag;

    

    public GameObject ProjectileOwner { get; set; }

    private bool hasCollided = false;

    public Vector3 Direction { get; set; }

    void Awake()
    {
        if (myCollider == null) myCollider = GetComponent<Collider2D>();
    }

    void Start()
    {
        
    }

    public void SetProjectileStats()
    { if (projectileStats != null) return;
        speed = projectileStats.speed;
        lifeTime = projectileStats.lifetime;
        damage = projectileStats.damage;
        hitAnimationGO = projectileStats.hitAnimationGO;
        hitSound = projectileStats.hitSound;
        //ownerTag = stats.ownerTag;
    }

    private void Update()
    { 
        if (lifeTime <= 0 && hasCollided == false)
        {
            OnCollided();
            StartCoroutine(DestroyGo());
        }
        else if (hasCollided == false)
        {
            lifeTime -= Time.deltaTime;
            transform.Translate(Direction * speed * Time.deltaTime);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == ProjectileOwner || hasCollided)
        {
            return; // Ignore collisions with the owner
        }
        else
        {
            OnCollided();

            IDamagable damagable = other.GetComponent<IDamagable>();
            if (damagable != null)
            {
                damagable.TakeDamage(damage);
                AudioManager.Instance.PlaySFX(hitSound);
            }

            if (hitAnimationGO != null) Instantiate(hitAnimationGO, transform.position, Quaternion.identity);

            StartCoroutine(DestroyGo());
        }
    }

    private void OnCollided()
    {
        if (hasCollided) return;
        hasCollided = true;
        if (myCollider != null) myCollider.enabled = false; 
    }

    private IEnumerator DestroyGo()
    {
        yield return new WaitForSeconds(timeToDestroy);
        Destroy(gameObject);
    }

}

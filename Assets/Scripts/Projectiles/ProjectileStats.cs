using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileStats", menuName = "Scriptable Objects/ProjectileStats")]
public class ProjectileStats : ScriptableObject
{
    public float speed;
    public float lifetime;
    public float damage;
    public GameObject hitAnimationGO;
    public AudioClip hitSound;
    public string ownerTag; 
}

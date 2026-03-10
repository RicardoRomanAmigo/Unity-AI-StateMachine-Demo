using UnityEngine;

[CreateAssetMenu(fileName = "PickupData", menuName = "Scriptable Objects/PickupData")]
public class PickupData : ScriptableObject
{
    public string PickupName;
    public Sprite sprite;
    public PickupType pickupType;
    public AudioClip pickupSound;
}

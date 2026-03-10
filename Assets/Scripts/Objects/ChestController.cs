using UnityEngine;
using System.Collections;

public class ChestController : MonoBehaviour
{
    [Header("references")]
    [SerializeField] private Animator chestAnimator;
    [SerializeField] private GameObject lootPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if(chestAnimator != null) chestAnimator = GetComponent<Animator>();
    }

    public void Open()
    {
        StartCoroutine(OpenChest());
    }

    private IEnumerator OpenChest()
    {
        chestAnimator.SetTrigger("Open");
        yield return new WaitForSeconds(1f);
        // Spawn loot or perform any other actions after the chest is opened
        SpawnLoot();
        yield return new WaitForSeconds(2f);
        
        //Destroy(gameObject); // Destroy the chest after a delay
    }

    private void SpawnLoot()
    {
        if (lootPrefab != null)
        {
            Instantiate(lootPrefab, transform.position, Quaternion.identity);
        }
    }
}

using UnityEngine;
using System;
using System.Collections;

public class ZoneController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] int zoneIndex;
    [SerializeField] EnemyController[] enemiesInZone;
    [SerializeField] GameObject[] environmentElements;
    [SerializeField] GameObject chest;
    [SerializeField] GameObject levelDoor;

    private bool zoneFinished = false;

    private void Awake()
    {   
        //HideZone();
    }

    private void Start()
    {
        HideZone();
    }

    private void OnEnable()
    { 
        GameManager.Instance.OnZoneChanged += HandleZoneChanged;
        SubscribeToEnemies();
    }

    private void OnDisable()
    {
        GameManager.Instance.OnZoneChanged -= HandleZoneChanged;
        UnsubscribeFromEnemies();
    }

    private void HandleZoneChanged(int newZoneIndex)
    {
        if(zoneFinished) return;

        if (newZoneIndex == zoneIndex)
        {
            zoneFinished = false;
            InitializeZone();
        }
    }

    private void HideZone()
    {
        if (enemiesInZone != null && zoneIndex > 0)
        {
            foreach (var enemy in enemiesInZone)
            {
                if (enemy != null)
                    enemy.gameObject.SetActive(false);
            }
        }

        if (environmentElements != null && zoneIndex > 0)
        {
            foreach (var element in environmentElements)
            {
                if (element != null)
                    element.SetActive(false);
            }
        }

        if (chest != null)
            chest.SetActive(false);
    }

    private void InitializeZone()
    {
        PlayerController.Instance.HasKey = false;

        if (enemiesInZone != null)
        {
            foreach (var enemy in enemiesInZone)
            {
                if (enemy != null)
                {
                    enemy.gameObject.SetActive(true);
                }
            }
        }

        if (environmentElements != null)
        {
            foreach (var element in environmentElements)
            {
                if (element != null)
                    element.SetActive(true);
            }
        }
    }

    private void SubscribeToEnemies()
    {
        if (enemiesInZone != null)
        {
            foreach (var enemy in enemiesInZone)
            {
                if (enemy != null)
                    enemy.OnEnemyDefeated += OnEnemyDefeated;
            }
        }
    }

    private void UnsubscribeFromEnemies()
    {
        if (enemiesInZone != null)
        {
            foreach (var enemy in enemiesInZone)
            {
                if (enemy != null)
                    enemy.OnEnemyDefeated -= OnEnemyDefeated;
            }
        }
    }

    private void OnEnemyDefeated(EnemyController enemy)
    {
        CheckZoneCleared();
    }

    private void CheckZoneCleared()
    {
        if (enemiesInZone == null) return;
        foreach (var enemy in enemiesInZone)
        {
            if (enemy != null && enemy.gameObject.activeSelf)
            {
                Debug.Log($"Enemy {enemy.name} is still alive in zone {zoneIndex}.");
                return;
            }
                
        }
        // All enemies are defeated
        zoneFinished = true;

        if (chest != null)
        {
            chest.SetActive(true);
            chest.GetComponent<ChestController>().Open();
        }
            
    }

    private void ActivateNexLevel()
    {
        if(GameManager.Instance != null)
            GameManager.Instance.ChangeZone();
    }
}

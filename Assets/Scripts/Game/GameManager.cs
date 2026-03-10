using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public int CurrentZoneIndex { get; set; } = 0;

    public static GameManager Instance { get; private set; }

    public event Action<int> OnZoneChanged;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ChangeZone()
    {
            CurrentZoneIndex++;
            OnZoneChanged?.Invoke(CurrentZoneIndex);
    }
}

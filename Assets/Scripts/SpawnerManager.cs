using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    [Header("Settings")]
    public int spawnRate;


    [Header("Refs")]
    [SerializeField] private GameObject npcPrefab;

    [Header("List")]
    public List<GameObject> spawnedNpcs;

    public static SpawnerManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    
}

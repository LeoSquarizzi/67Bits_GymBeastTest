
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    [Header("Settings")]
    public float spawnDelay;
    public int maxNpcsOnMap;

    [SerializeField] float spawnAreaMinPosX;
    [SerializeField] float spawnAreaMinPosZ;
    [SerializeField] float spawnAreaMaxPosX;
    [SerializeField] float spawnAreaMaxPosZ;

    [Header("Refs")]
    [SerializeField] private GameObject npcPrefab;

    [Header("List")]
    public List<GameObject> spawnedNpcs;
    public bool spawning;

    private Camera cam;

    public static SpawnerManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        cam = Camera.main;
    }

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        spawning = true;

        while (spawning)
        {
            yield return new WaitForSeconds(spawnDelay);
            if(spawnedNpcs.Count < maxNpcsOnMap)
                SpawnNPC();
        }
    }

    public void SpawnNPC()
    {
        float posX = Random.Range(spawnAreaMinPosX, spawnAreaMaxPosX);
        float posZ = Random.Range(spawnAreaMinPosZ, spawnAreaMaxPosZ);
        Vector3 spawnPosition = new Vector3(posX, 1f, posZ);
        GameObject npc = Instantiate(npcPrefab, spawnPosition, Quaternion.identity);
        spawnedNpcs.Add(npc);
    }

    public void ToggleSpawner(bool toggle)
    {
        spawning = toggle;
    }

    public void RemoveFromList(NpcBase npc)
    {
        spawnedNpcs.Remove(npc.gameObject);
    }

    private void OnEnable()
    {
        NpcBase.OnKnockedOut += RemoveFromList;
    }

    private void OnDisable()
    {
        NpcBase.OnKnockedOut -= RemoveFromList;
    }
}

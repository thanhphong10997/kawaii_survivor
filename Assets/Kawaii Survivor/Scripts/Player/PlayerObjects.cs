using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerStatsManager))]
public class PlayerObjects : MonoBehaviour
{
    [field: SerializeField] public List<ObjectDataSO> Objects { get; private set; }
    private PlayerStatsManager playerStatsManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        playerStatsManager = GetComponent<PlayerStatsManager>();
        foreach (ObjectDataSO objectData in Objects)
        {
            playerStatsManager.AddObject(objectData.BaseStats);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddObject(ObjectDataSO objectData)
    {
        Objects.Add(objectData);
        playerStatsManager.AddObject(objectData.BaseStats);
    }
}

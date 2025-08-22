using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private CharacterDataSO playerData;
    private Dictionary<Stat, float> addends = new Dictionary<Stat, float>();
    private Dictionary<Stat, float> playerStats = new Dictionary<Stat, float>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        // Khởi tạo giá trị cho player Stats bằng giá trị đã khai báo ở property BaseStats
        playerStats = playerData.BaseStats;
        foreach (KeyValuePair<Stat, float> kvp in playerStats) addends.Add(kvp.Key, 0);
    }
    void Start()
    {
        UpdatePlayerStats();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddPlayerStat(Stat stat, float value)
    {
        // Player -> base Stats

        // Addends -> Upgrades in the Wave Transition
        if (addends.ContainsKey(stat)) addends[stat] += value;
        else Debug.LogError($"The key {stat} has not been found");

        UpdatePlayerStats();

        // Objects -> List Object stats

    }

    private void UpdatePlayerStats()
    {
        IEnumerable<IPlayerStatsDependency> playerStatsDependencies = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IPlayerStatsDependency>();    // Kiểm tra nếu Object có type MonoBehaviour có thừa hưởng interface IPlayerStatsDependency
        foreach (IPlayerStatsDependency dependency in playerStatsDependencies)
        {
            dependency.UpdateStats(this);
        }
    }

    public float GetStatValue(Stat stat)
    {
        float value = playerStats[stat] + addends[stat];
        return value;
    }

}

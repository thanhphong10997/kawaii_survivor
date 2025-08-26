using System.Collections.Generic;
using UnityEngine;

public static class WeaponStatsCalculator
{
    public static Dictionary<Stat, float> GetStats(WeaponDataSO weaponData, int level)
    {
        // Khi weapon đạt lv 3 thì damage sẽ nhân đôi, có thể tùy chỉnh / 3 để tăng hoặc giảm damage khi lên lv
        float multiplier = 1 + (float)level / 3;
        Dictionary<Stat, float> calculatedStats = new Dictionary<Stat, float>();
        foreach (KeyValuePair<Stat, float> kvp in weaponData.BaseStats)
        {
            // Trường hợp là range weapon và stat là range thì ko thực hiện tính toán
            if (weaponData.Prefab.GetType() != typeof(RangeWeapon) && kvp.Key == Stat.Range)
                calculatedStats.Add(kvp.Key, kvp.Value);
            else calculatedStats.Add(kvp.Key, kvp.Value * multiplier);

        }

        return calculatedStats;
    }
}

using System;
using UnityEngine;

// Script này sẽ đc gọi khi update player stats
// Nếu wave transition panel inactive, script này sẽ ko đc gọi nên phải thêm tham số FindObjectsInactive.Include vào hàm UpdatePlayerStats ở script PlayerStatManager
// Sau khi thêm tham số, script này vẫn sẽ đc gọi kể cả khi inactive
public class PlayerStatsDisplay : MonoBehaviour, IPlayerStatsDependency
{
    [Header("Elements")]
    [SerializeField] private Transform playerStatContainersParent;


    public void UpdateStats(PlayerStatsManager playerStatsManager)
    {
        int index = 0;
        foreach (Stat stat in Enum.GetValues(typeof(Stat)))
        {
            StatContainer statContainer = playerStatContainersParent.GetChild(index).GetComponent<StatContainer>();
            // Set active để chắc chắn nếu lỡ có quên active object
            statContainer.gameObject.SetActive(true);

            Sprite statIcon = ResourcesManager.GetStatIcon(stat);
            float statValue = playerStatsManager.GetStatValue(stat);
            statContainer.Configure(statIcon, Enums.FormatStatName(stat), statValue, true);

            index++;
        }

        // Lặp từ vị trí index + 1 (vì ở trên dùng index++) đến vị trí cuối của child obbject trong playerStatContainersParent và deactive các obbject đó
        for (int i = index; i < playerStatContainersParent.childCount; i++)
        {
            playerStatContainersParent.GetChild(i).gameObject.SetActive(false);
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

public class StatContainerManager : MonoBehaviour
{
    public static StatContainerManager instance;
    [Header("Elements")]
    [SerializeField] private StatContainer statContainer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void GenerateContainers(Dictionary<Stat, float> statDictionary, Transform parent)
    {
        List<StatContainer> statContainers = new List<StatContainer>();
        foreach (KeyValuePair<Stat, float> kvp in statDictionary)
        {
            StatContainer containerInstance = Instantiate(statContainer, parent);
            statContainers.Add(containerInstance);
            // Lấy icon từ resources
            Sprite statIcon = ResourcesManager.GetStatIcon(kvp.Key);
            string statName = Enums.FormatStatName(kvp.Key);
            string statValue = kvp.Value.ToString("F2");
            containerInstance.Configure(statIcon, statName, statValue);
        }

        // Khi làm việc với UI component thì cần phải đợi 1 khoảng thời gian để có thể update UI\
        // Đợi 2 frame rồi mới resize texts
        LeanTween.delayedCall(Time.deltaTime * 2, () => ResizeTexts(statContainers));

    }

    private void ResizeTexts(List<StatContainer> statContainers)
    {
        float minFontSize = 5000;
        for (int i = 0; i < statContainers.Count; i++)
        {
            StatContainer statContainer = statContainers[i];
            float fontSize = statContainer.GetFontSize();
            if (fontSize < minFontSize) minFontSize = fontSize;
        }

        // Đến được dòng này có nghĩa là ta đã lấy được min font size
        // Set min font size cho tất cả các stats name
        for (int i = 0; i < statContainers.Count; i++)
        {
            statContainers[i].SetFontSize(minFontSize);
        }
    }

    public static void GenerateStatContainers(Dictionary<Stat, float> statDictionary, Transform parent)
    {
        instance.GenerateContainers(statDictionary, parent);
    }
}

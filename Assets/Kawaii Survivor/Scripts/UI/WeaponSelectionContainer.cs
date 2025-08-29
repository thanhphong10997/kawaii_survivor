using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class WeaponSelectionContainer : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] Image icon;
    [SerializeField] private TextMeshProUGUI nameText;

    [field: SerializeField] public Button Button { get; private set; }

    [Header("Stats")]
    [SerializeField] private Transform statContainersParent;

    [Header("Color")]
    [SerializeField] private Image[] levelDependentImages;
    [SerializeField] private Image outline;
    public void Configure(WeaponDataSO weaponData, int level)
    {
        icon.sprite = weaponData.Sprite;
        nameText.text = weaponData.Name + $" (lvl {level + 1})";

        Color imageColor = ColorHolder.GetColor(level);
        // Set màu cho tên weapon
        nameText.color = imageColor;

        outline.color = ColorHolder.GetOutlineColor(level); ;

        foreach (Image image in levelDependentImages)
        {
            image.color = imageColor;
        }

        Dictionary<Stat, float> calculatedStats = WeaponStatsCalculator.GetStats(weaponData, level);
        ConfigureStatContainers(calculatedStats);
    }

    private void ConfigureStatContainers(Dictionary<Stat, float> calculatedStats)
    {
        StatContainerManager.GenerateStatContainers(calculatedStats, statContainersParent);
    }

    public void Select()
    {
        // Dừng LeanTween hiện tại lại
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, Vector3.one * 1.075f, 0.3f).setEase(LeanTweenType.easeInSine);
    }

    public void Deselect()
    {
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, Vector3.one, 0.3f);
    }
}

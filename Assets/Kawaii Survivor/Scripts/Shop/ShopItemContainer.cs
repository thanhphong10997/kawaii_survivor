using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
public class ShopItemContainer : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] Image icon;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI priceText;

    [field: SerializeField] public Button PurchaseButton { get; private set; }

    [Header("Stats")]
    [SerializeField] private Transform statContainersParent;

    [Header("Color")]
    [SerializeField] private Image[] levelDependentImages;
    [SerializeField] private Image outline;

    [Header("Lock Elements")]
    [SerializeField] private Image lockImage;
    [SerializeField] private Sprite lockedSprite, unlockedSprite;
    public bool IsLocked { get; private set; }


    public void Configure(WeaponDataSO weaponData, int level)
    {
        icon.sprite = weaponData.Sprite;
        nameText.text = weaponData.Name + $" (lvl {level + 1})";
        priceText.text = WeaponStatsCalculator.GetPurchasePrice(weaponData, level).ToString();

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

    public void Configure(ObjectDataSO objectData)
    {
        icon.sprite = objectData.Icon;
        nameText.text = objectData.Name;
        priceText.text = objectData.Price.ToString();

        Color imageColor = ColorHolder.GetColor(objectData.Rarity);
        // Set màu cho tên weapon
        nameText.color = imageColor;

        outline.color = ColorHolder.GetOutlineColor(objectData.Rarity); ;

        foreach (Image image in levelDependentImages)
        {
            image.color = imageColor;
        }

        // Dictionary<Stat, float> calculatedStats = WeaponStatsCalculator.GetStats(weaponData, level);
        ConfigureStatContainers(objectData.BaseStats);
    }

    private void ConfigureStatContainers(Dictionary<Stat, float> stats)
    {
        statContainersParent.Clear();
        StatContainerManager.GenerateStatContainers(stats, statContainersParent);
    }

    public void LockButtonCallback()
    {
        IsLocked = !IsLocked;
        UpdateLockVisuals();
    }

    private void UpdateLockVisuals()
    {
        lockImage.sprite = IsLocked ? lockedSprite : unlockedSprite;
    }
}

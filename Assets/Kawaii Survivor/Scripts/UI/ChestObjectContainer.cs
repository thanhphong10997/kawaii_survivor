using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class ChestObjectContainer : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] Image icon;
    [SerializeField] private TextMeshProUGUI nameText;

    [field: SerializeField] public Button TakeButton { get; private set; }
    [field: SerializeField] public Button RecycleButton { get; private set; }
    [SerializeField] public TextMeshProUGUI recyclePriceText;
    [Header("Stats")]
    [SerializeField] private Transform statContainersParent;

    [Header("Color")]
    [SerializeField] private Image[] levelDependentImages;
    [SerializeField] private Image outline;
    public void Configure(ObjectDataSO objectData)
    {
        icon.sprite = objectData.Icon;
        nameText.text = objectData.Name;
        recyclePriceText.text = objectData.RecyclePrice.ToString();

        Color imageColor = ColorHolder.GetColor(objectData.Rarity);
        // Set màu cho tên object
        nameText.color = imageColor;

        outline.color = ColorHolder.GetOutlineColor(objectData.Rarity); ;

        foreach (Image image in levelDependentImages)
        {
            image.color = imageColor;
        }

        ConfigureStatContainers(objectData.BaseStats);
    }

    private void ConfigureStatContainers(Dictionary<Stat, float> stats)
    {
        StatContainerManager.GenerateStatContainers(stats, statContainersParent);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

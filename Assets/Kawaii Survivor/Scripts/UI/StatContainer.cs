using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Đây là container chứa các Stats
public class StatContainer : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image statImage;
    [SerializeField] private TextMeshProUGUI statNameText;
    [SerializeField] private TextMeshProUGUI statValueText;

    public void Configure(Sprite icon, string statName, string statValue)
    {
        statImage.sprite = icon;
        statNameText.text = statName;
        statValueText.text = statValue;
    }
}

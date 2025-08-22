using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeContainer : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI upgradeNameText;
    [SerializeField] private TextMeshProUGUI upgradeValueText;

    // Khai báo property Button gồm 2 phương thức set và get. Lưu ý phải có field: thì property mới xuất hiện trong Inspector vì SerializeField chỉ áp dụng cho field
    [field: SerializeField] public Button Button { get; private set; }

    public void Configure(Sprite icon, string upgradeName, string upgradeValue)
    {
        image.sprite = icon;
        upgradeNameText.text = upgradeName;
        upgradeValueText.text = upgradeValue;
    }
}

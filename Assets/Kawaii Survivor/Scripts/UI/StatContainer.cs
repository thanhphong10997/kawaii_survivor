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

    public void Configure(Sprite icon, string statName, float statValue, bool useColor = false)
    {
        statImage.sprite = icon;
        statNameText.text = statName;

        if (useColor) ColorizeStatValueText(statValue);
        else
        {
            statValueText.color = Color.white;
            statValueText.text = statValue.ToString("F2");
        }


    }

    private void ColorizeStatValueText(float statValue)
    {
        float sign = Mathf.Sign(statValue);   // Trả về -1 nếu statValue < 0, trả về 0 nếu statValue = 0 và trả về 1 nếu statValue > 0
        if (statValue == 0) sign = 0;   // Mặc định sign sẽ = 0.0f nếu statValue = 0 và 0.0f > 0 nên phải gán sign = 0
        float absStatValue = Mathf.Abs(statValue);
        Color statValueTextColor = Color.white;
        if (sign > 0) statValueTextColor = Color.green;
        else if (sign < 0) statValueTextColor = Color.red;
        statValueText.color = statValueTextColor;

        statValueText.text = absStatValue.ToString("F2");
    }

    public float GetFontSize()
    {
        return statNameText.fontSize;
    }

    public void SetFontSize(float minFontSize)
    {
        statNameText.fontSizeMax = minFontSize;
        statValueText.fontSizeMax = minFontSize;
    }
}

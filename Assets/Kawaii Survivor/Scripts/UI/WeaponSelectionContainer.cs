using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelectionContainer : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] Image icon;
    [SerializeField] private TextMeshProUGUI nameText;

    [field: SerializeField] public Button Button { get; private set; }

    [Header("Color")]
    [SerializeField] private Image[] levelDependentImages;
    public void Configure(Sprite sprite, string name, int level)
    {
        icon.sprite = sprite;
        nameText.text = name;

        Color imageColor;
        switch (level)
        {
            case 0:
                imageColor = Color.white;
                break;
            case 1:
                imageColor = Color.red;
                break;
            case 2:
                imageColor = Color.blue;
                break;
            default:
                imageColor = Color.green;
                break;
        }
        foreach (Image image in levelDependentImages)
        {
            image.color = imageColor;
        }
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

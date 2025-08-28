using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager instance;
    [field: SerializeField] public int Currency { get; private set; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    void Start()
    {
        UpdateTexts();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddCurrency(int amount)
    {
        Currency += amount;
        UpdateTexts();
    }

    private void UpdateTexts()
    {
        CurrencyText[] currencyTexts = FindObjectsByType<CurrencyText>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (CurrencyText text in currencyTexts)
        {
            text.UpdateText(Currency.ToString());
        }
    }
}

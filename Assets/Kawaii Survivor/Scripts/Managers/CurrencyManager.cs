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

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddCurrency(int amount)
    {
        Currency += amount;
    }
}

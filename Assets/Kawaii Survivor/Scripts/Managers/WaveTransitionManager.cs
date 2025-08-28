using System;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Random = UnityEngine.Random;
public class WaveTransitionManager : MonoBehaviour, IGameStateListener
{
    [Header("Elements")]
    [SerializeField] private UpgradeContainer[] upgradeContainers;
    [SerializeField] private PlayerStatsManager playerStatsManager;
    [SerializeField] private GameObject upgradeContainersParent;
    public static WaveTransitionManager instance;

    [Header("Player")]
    [SerializeField] private PlayerObjects playerObjects;

    [Header("Chest Related Stuff")]
    [SerializeField] private ChestObjectContainer chestContainerPrefab;
    [SerializeField] private Transform chestContainerParent;

    [Header("Settings")]
    private int chestCollected;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        Chest.onCollected += ChestCollectedCallback;
    }


    private void OnDestroy()
    {
        Chest.onCollected -= ChestCollectedCallback;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GameStateChangedCallback(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.WAVETRANSITION:
                TryOpenChest();
                break;
        }
    }

    private void TryOpenChest()
    {
        // Xóa chest container UI (thẻ object cộng chỉ số) sau khi bấm nút Take (mở rương)
        // Mỗi thẻ chest container sẽ tương ứng với 1 rương, nếu ko xóa chest container sẽ vẫn tồn tại mặc dù chỉ số đã đc cộng
        chestContainerParent.Clear();

        if (chestCollected > 0) ShowObject();
        else ConfigureUpgradeContainers();
    }

    private void ShowObject()
    {
        chestCollected--;

        upgradeContainersParent.SetActive(false);

        ObjectDataSO[] objectDatas = ResourcesManager.Objects;
        // Tạo một thẻ chứa object ngẫu nhiên trong folder Resources -> Data -> Objects
        ObjectDataSO randomObjectData = objectDatas[Random.Range(0, objectDatas.Length)];

        ChestObjectContainer containerInstance = Instantiate(chestContainerPrefab, chestContainerParent);
        containerInstance.Configure(randomObjectData);

        containerInstance.TakeButton.onClick.AddListener(() => TakeButtonCallback(randomObjectData));
        containerInstance.RecycleButton.onClick.AddListener(() => RecycleButtonCallback(randomObjectData));
    }

    private void TakeButtonCallback(ObjectDataSO objectToTake)
    {
        playerObjects.AddObject(objectToTake);
        // Tiếp tục mở các rương tiếp theo
        TryOpenChest();
    }

    private void RecycleButtonCallback(ObjectDataSO objectToRecycle)
    {
        CurrencyManager.instance.AddCurrency(objectToRecycle.RecyclePrice);
        // Tiếp tục mở các rương tiếp theo
        TryOpenChest();
    }

    [Button]
    private void ConfigureUpgradeContainers()
    {
        upgradeContainersParent.SetActive(true);
        for (int i = 0; i < upgradeContainers.Length; i++)
        {
            int randomIndex = Random.Range(0, Enum.GetValues(typeof(Stat)).Length);
            Stat stat = (Stat)Enum.GetValues(typeof(Stat)).GetValue(randomIndex);
            string randomStatString = Enums.FormatStatName(stat);
            Sprite upgradeSprite = ResourcesManager.GetStatIcon(stat);

            // Lấy TextMeshProUGUI component từ upgradeContainers sau đó gán giá trị text cho TextMeshProUGUI
            // upgradeContainers[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = randomStatString;
            string valueString;
            Action action = GetActionToPeform(stat, out valueString);
            upgradeContainers[i].Configure(upgradeSprite, randomStatString, valueString);
            // upgradeContainers[i].Button => tham chiếu đến phương thức Button của UpgradeContainer script
            // Xóa tất cả các listener có thể vô tình add vào nút này
            upgradeContainers[i].Button.onClick.RemoveAllListeners();
            upgradeContainers[i].Button.onClick.AddListener(() => action?.Invoke());
            // Khi bấm vào các thẻ tăng chỉ số thì nếu nhân vật vẫn còn tăng level thì cho tiếp tục tăng chỉ số, nếu ko còn tăng cấp thì cho vào Shop
            upgradeContainers[i].Button.onClick.AddListener(() => BonusSelectedCallback());
        }
    }

    private void BonusSelectedCallback()
    {
        GameManager.instance.WaveCompletedCallback();
    }

    private Action GetActionToPeform(Stat stat, out string valueString)
    {
        valueString = "";
        float value = 0;

        switch (stat)
        {
            case Stat.Attack:
                value = Random.Range(1, 10);
                valueString = $"+{value}%";
                break;
            case Stat.AttackSpeed:
                value = Random.Range(1, 10);
                valueString = $"+{value}%";
                break;
            case Stat.CriticalChance:
                value = Random.Range(1, 10);
                valueString = $"+{value}%";
                break;
            case Stat.CriticalPercent:
                value = Random.Range(1f, 2f);
                valueString = $"+{value:F2}x";
                break;
            case Stat.MoveSpeed:
                value = Random.Range(1, 10);
                valueString = $"+{value}%";
                break;
            case Stat.MaxHealth:
                value = Random.Range(1, 5);
                valueString = $"+{value}";
                break;
            case Stat.Range:
                value = Random.Range(1f, 5f);
                valueString = $"+{value:F2}";
                break;
            case Stat.HealthRecoverySpeed:
                value = Random.Range(1, 10);
                valueString = $"+{value}%";
                break;
            case Stat.Armor:
                value = Random.Range(1, 10);
                valueString = $"+{value}%";
                break;
            case Stat.Luck:
                value = Random.Range(1, 10);
                valueString = $"+{value}%";
                break;
            case Stat.Dodge:
                value = Random.Range(1, 10);
                valueString = $"+{value}%";
                break;
            case Stat.Lifesteal:
                value = Random.Range(1, 10);
                valueString = $"+{value}%";
                break;
            default:
                Debug.Log("Invalid Stat");
                break;
        }

        // Hiển thị text kiểu: Critical + Enter + 5%
        // valueString = Enums.FormatStatName(stat) + "\n" + valueString;

        return () => playerStatsManager.AddPlayerStat(stat, value);
    }

    private void ChestCollectedCallback(Chest chest)
    {
        chestCollected++;
    }

    public bool HasCollectedChest()
    {
        return chestCollected > 0;
    }
}

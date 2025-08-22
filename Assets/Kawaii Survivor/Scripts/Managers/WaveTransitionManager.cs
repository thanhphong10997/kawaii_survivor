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
                ConfigureUpgradeContainers();
                break;
        }
    }

    [Button]
    private void ConfigureUpgradeContainers()
    {
        for (int i = 0; i < upgradeContainers.Length; i++)
        {
            int randomIndex = Random.Range(0, Enum.GetValues(typeof(Stat)).Length);
            Stat stat = (Stat)Enum.GetValues(typeof(Stat)).GetValue(randomIndex);
            string randomStatString = Enums.FormatStatName(stat);

            // Lấy TextMeshProUGUI component từ upgradeContainers sau đó gán giá trị text cho TextMeshProUGUI
            // upgradeContainers[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = randomStatString;
            string valueString;
            Action action = GetActionToPeform(stat, out valueString);
            upgradeContainers[i].Configure(null, randomStatString, valueString);
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
        float value;
        value = Random.Range(1, 10);
        valueString = $"+{value}%";
        switch (stat)
        {
            case Stat.Attack:
                value = Random.Range(1, 10);
                break;
            case Stat.AttackSpeed:
                break;
            case Stat.CriticalChance:
                value = Random.Range(1, 10);
                break;
            case Stat.CriticalPercent:
                value = Random.Range(1f, 2f);
                valueString = $"+{value:F2}";
                break;
            case Stat.MoveSpeed:
                value = Random.Range(1, 10);
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
                break;
            case Stat.Armor:
                value = Random.Range(1, 10);
                break;
            case Stat.Luck:
                value = Random.Range(1, 10);
                break;
            case Stat.Dodge:
                value = Random.Range(1, 10);
                break;
            case Stat.Lifesteal:
                value = Random.Range(1, 10);
                break;
            default:
                Debug.Log("Invalid Stat");
                break;
        }

        return () => playerStatsManager.AddPlayerStat(stat, value);
    }
}

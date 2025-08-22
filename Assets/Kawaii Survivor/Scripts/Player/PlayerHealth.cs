using TMPro;
using Unity.IntegerTime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IPlayerStatsDependency
{
    [Header("Settings")]
    [SerializeField] private int baseMaxHealth;
    private int maxHealth;

    [Header("Components")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;
    private int health;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        // Lý do sử dụng Mathf.Min: Nếu health còn ít hơn lượng damage gây ra thì health = health - health = 0 => health sẽ ko ra số âm
        int realDamage = Mathf.Min(damage, health);
        health -= realDamage;
        UpdateHealthUI();

        if (health <= 0) PassAway();
    }

    private void PassAway()
    {
        GameManager.instance.SetGameState(GameState.GAMEOVER);
    }

    private void UpdateHealthUI()
    {
        float healthBarValue = (float)health / maxHealth;
        healthSlider.value = healthBarValue;
        healthText.text = $"{health} / {maxHealth}";
    }

    public void UpdateStats(PlayerStatsManager playerStatsManager)
    {
        float addedHealth = playerStatsManager.GetStatValue(Stat.MaxHealth);
        maxHealth = baseMaxHealth + (int)addedHealth;

        // Nếu addedHealth có giá trị âm lớn hơn baseMaxHealth thì maxHealth sẽ ra số âm, nên sử dụng Mathf.Max để tránh trường hợp đó
        maxHealth = Mathf.Max(maxHealth, 1);
        health = maxHealth;
        Debug.Log("maxHealth: " + maxHealth);
        UpdateHealthUI();
    }
}

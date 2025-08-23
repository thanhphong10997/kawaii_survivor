using System;
using TMPro;
using Unity.IntegerTime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Random = UnityEngine.Random;
public class PlayerHealth : MonoBehaviour, IPlayerStatsDependency
{
    [Header("Settings")]
    [SerializeField] private int baseMaxHealth;

    [Header("Components")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;
    private float health;
    private float maxHealth;
    private float armor;
    private float lifeSteal;
    private float dodge;
    private float healthRecoverySpeed;
    private float healthRecoveryTimer;
    private float healthRecoveryDuration;


    [Header("Actions")]
    public static Action<Vector2> onAttackDodged;

    void Awake()
    {
        Enemy.onDamageTaken += EnemyTookDamageCallback;
    }

    void OnDestroy()
    {
        Enemy.onDamageTaken -= EnemyTookDamageCallback;
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (health < maxHealth) RecoverHealth();
    }

    private void RecoverHealth()
    {
        healthRecoveryTimer += Time.deltaTime;
        if (healthRecoveryTimer >= healthRecoveryDuration)
        {
            healthRecoveryTimer = 0;
            // Số lượng máu hồi, vd 0.1f => hồi 1 máu 1s
            float healthToAdd = Mathf.Min(0.1f, maxHealth - health);
            health += healthToAdd;
            UpdateHealthUI();
        }
    }

    public void TakeDamage(int damage)
    {
        if (ShouldDodge())
        {
            onAttackDodged?.Invoke(transform.position);
            return;
        }

        // armor / 1000 => Giới hạn chỉ số armor sẽ là 1000, tức là nếu player có 1000 armor thì damage nhận vào sẽ = 0 (tính dựa theo Mathf.Clamp)
        // Có thể tăng 1000 thành số lớn hơn để tránh trường hợp ở trên. VD: 100000
        float realDamage = damage * Mathf.Clamp(1 - (armor / 1000), 0, 10000);
        // Lý do sử dụng Mathf.Min: Nếu health còn ít hơn lượng damage gây ra thì health = health - health = 0 => health sẽ ko ra số âm
        realDamage = Mathf.Min(realDamage, health);
        health -= realDamage;
        UpdateHealthUI();

        if (health <= 0) PassAway();
    }

    private bool ShouldDodge()
    {
        // Nếu dodge có chỉ số trên 100 thì kết quả luôn là true, có thể dùng Math.Clamp để giới hạn chỉ số
        return Random.Range(1f, 100f) <= dodge;
    }

    private void PassAway()
    {
        GameManager.instance.SetGameState(GameState.GAMEOVER);
    }

    private void UpdateHealthUI()
    {
        float healthBarValue = health / maxHealth;
        healthSlider.value = healthBarValue;
        healthText.text = $"{(int)health} / {maxHealth}";
    }

    public void UpdateStats(PlayerStatsManager playerStatsManager)
    {
        float addedHealth = playerStatsManager.GetStatValue(Stat.MaxHealth);
        maxHealth = baseMaxHealth + (int)addedHealth;

        // Nếu addedHealth có giá trị âm lớn hơn baseMaxHealth thì maxHealth sẽ ra số âm, nên sử dụng Mathf.Max để tránh trường hợp đó
        maxHealth = Mathf.Max(maxHealth, 1);
        health = maxHealth;
        UpdateHealthUI();

        armor = playerStatsManager.GetStatValue(Stat.Armor);
        lifeSteal = playerStatsManager.GetStatValue(Stat.Lifesteal) / 100;   // lifeSteal sẽ là dạng %
        dodge = playerStatsManager.GetStatValue(Stat.Dodge);

        // Health recovery
        healthRecoverySpeed = Mathf.Max(0.0001f, playerStatsManager.GetStatValue(Stat.HealthRecoverySpeed));
        healthRecoveryDuration = 1f / healthRecoverySpeed;   //    1s / tốc độ hồi máu = chu kỳ hồi máu
    }

    private void EnemyTookDamageCallback(int damage, Vector2 enemyPosition, bool isCriticalHit)
    {
        if (health >= maxHealth) return;

        // lifeSteal tính bằng dmg enemy nhận vào nhân với phần trăm lifeSteal
        float lifeStealValue = damage * lifeSteal;
        // health đc add phải luôn bé hơn max health. Có nghĩa là số lượng health đc add sẽ là giá trị min giữa lifeSteal và giá trị máu đã mất
        float healthToAdd = Mathf.Min(lifeStealValue, maxHealth - health);
        health += healthToAdd;
    }
}

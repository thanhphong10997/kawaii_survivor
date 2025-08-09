using TMPro;
using Unity.IntegerTime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int maxHealth;

    [Header("Components")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;
    private int health;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
        UpdateHealthUI();
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
        Debug.Log("Health left:" + health);
        UpdateHealthUI();

        if (health <= 0) PassAway();
    }

    private void PassAway()
    {
        SceneManager.LoadScene(0);
    }

    private void UpdateHealthUI()
    {
        float healthBarValue = (float)health / maxHealth;
        healthSlider.value = healthBarValue;
        healthText.text = $"{health} / {maxHealth}";
    }
}

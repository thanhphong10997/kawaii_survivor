using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLevel : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField] private Slider xpBar;
    [SerializeField] private TextMeshProUGUI levelText;

    [Header("Settings")]
    private int requireXp;
    private int currentXp;
    private int level;
    private int levelsEarnedThisWave;


    void Awake()
    {
        Candy.onCollected += CandyCollectedCallback;
    }

    void Start()
    {
        UpdateRequireXp();
        UpdateVisuals();
    }

    void OnDestroy()
    {
        Candy.onCollected -= CandyCollectedCallback;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void CandyCollectedCallback(Candy candy)
    {
        currentXp++;

        if (currentXp >= requireXp) LevelUp();
        UpdateVisuals();
    }

    private void LevelUp()
    {
        level++;
        levelsEarnedThisWave++;
        currentXp = 0;
        UpdateRequireXp();
    }

    private void UpdateRequireXp()
    {
        // Cần thu thập đủ 5 candy sẽ lên cấp
        requireXp = (level + 1) * 5;
    }

    private void UpdateVisuals()
    {
        xpBar.value = (float)currentXp / requireXp;
        levelText.text = "lvl " + (level + 1);
    }

    public bool HasLeveledUp()
    {
        if (levelsEarnedThisWave > 0)
        {
            levelsEarnedThisWave--;
            return true;
        }

        return false;
    }
}

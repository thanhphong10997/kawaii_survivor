using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement), typeof(RangeEnemyAttack))]
public class RangeEnemy : MonoBehaviour
{
    [Header("Elements")]
    private Player player;
    private EnemyMovement enemyMovement;

    [Header("Spawn Sequence Related")]
    [SerializeField] private SpriteRenderer enemyRenderer;
    [SerializeField] private SpriteRenderer spawnIndicator;

    [Header("Settings")]
    [SerializeField] private float scaleValue;
    [SerializeField] private float playerDetectionRadius;
    [SerializeField] private int maxHealth;

    [Header("Effects")]
    [SerializeField] private ParticleSystem passAwayParticle;

    [Header("Attack")]
    [SerializeField] private int damage;
    [SerializeField] private float attackFrequency;
    private RangeEnemyAttack rangeAttack;

    [Header("Health")]
    [SerializeField] private TextMeshPro healthText;

    [Header("Actions")]
    public static Action<int, Vector2> onDamageTaken;

    [Header("Collider")]
    [SerializeField] private Collider2D enemyCollider;

    [Header("Debug")]
    [SerializeField] private bool showGizmos;

    private int health;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
        enemyMovement = GetComponent<EnemyMovement>();
        rangeAttack = GetComponent<RangeEnemyAttack>();

        // Tìm GameObject đầu tiên có component Player
        player = FindFirstObjectByType<Player>();
        rangeAttack.StorePlayer(player);

        if (player == null)
        {
            Debug.LogWarning("No player was found, destroying it...");
            // Xóa GameObject
            Destroy(gameObject);
        }

        StartSpawnSequence();

        // Hide Renderer
        // enemyRenderer.enabled = false;
        // Show spawn indicator
        // spawnIndicator.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        // attackTimer là số giây trên mỗi Frame, khi số giây trên mỗi Frame >= số giây delay thì sẽ đánh 1 cái rồi reset attackTimer

        // Nếu enemy chưa đc spawn thì ko làm gì cả
        if (!enemyRenderer.enabled) return;
        ManageAttack();
    }

    private void StartSpawnSequence()
    {
        SetRendererVisibility(false);

        // Add animation to spawn indicator (scale up & down)
        Vector3 targetScale = spawnIndicator.transform.localScale * scaleValue;

        // Thực hiện scale animation 4 lần sau đó gọi hàm SpawnSequenceComplete
        LeanTween.scale(spawnIndicator.gameObject, targetScale, .5f).setLoopPingPong(4).setOnComplete(SpawnSequenceComplete);

    }

    private void ManageAttack()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        // float magnitude = (player.transform.position - transform.position).magnitude;
        // Debug.Log("distance " + distanceToPlayer);
        // Debug.Log("magnitude " + magnitude);

        if (distanceToPlayer > playerDetectionRadius) enemyMovement.FollowPlayer();
        else TryAttack();
    }

    private void TryAttack()
    {
        rangeAttack.AutoAim();

    }

    public void TakeDamage(int damage)
    {
        int realDamage = Mathf.Min(damage, health);
        health -= realDamage;

        // call onDamageTaken action
        onDamageTaken?.Invoke(damage, transform.position);

        healthText.text = health.ToString();

        if (health <= 0) PassAwayEffect();

    }

    private void PassAwayEffect()
    {
        // unparent the passAway component from Enemy component
        passAwayParticle.transform.parent = null;
        // passAwayParticle.transform.SetParent(null);

        passAwayParticle.Play();
        Destroy(gameObject);
    }

    private void SpawnSequenceComplete()
    {
        // Show the renderer(enemy) after looping animation
        // enemyRenderer.enabled = true;
        // Hide spawn indicator
        // spawnIndicator.enabled = false;

        // Active collider only when spawn sequence is completed to prevent attack enemy before spawning process
        enemyCollider.enabled = true;

        SetRendererVisibility(true);

        // Prevent Following and Attacking during the whole spawn sequence
        // hasSpawned = true;

        enemyMovement.StorePlayer(player);

    }

    private void SetRendererVisibility(bool visibility)
    {
        enemyRenderer.enabled = visibility;
        spawnIndicator.enabled = !visibility;
    }

    void OnDrawGizmos()
    {
        if (!showGizmos) return;

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, playerDetectionRadius);
    }
}

using UnityEngine;
using System;
using TMPro;

public abstract class Enemy : MonoBehaviour
{
    [Header("Elements")]
    protected Player player;
    protected EnemyMovement enemyMovement;

    [Header("Spawn Sequence Related")]
    [SerializeField] protected SpriteRenderer enemyRenderer;
    [SerializeField] protected SpriteRenderer spawnIndicator;

    [Header("Settings")]
    [SerializeField] protected float scaleValue;
    [SerializeField] protected float playerDetectionRadius;
    [SerializeField] protected int maxHealth;

    [Header("Effects")]
    [SerializeField] protected ParticleSystem passAwayParticle;

    [Header("Attack")]

    [Header("Health")]
    [SerializeField] protected TextMeshPro healthText;
    protected int health;

    [Header("Actions")]
    public static Action<int, Vector2, bool> onDamageTaken;

    [Header("Collider")]
    [SerializeField] protected Collider2D enemyCollider;

    [Header("Debug")]
    [SerializeField] protected bool showGizmos;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        health = maxHealth;
        enemyMovement = GetComponent<EnemyMovement>();
        // Tìm GameObject đầu tiên có component Player
        player = FindFirstObjectByType<Player>();

        if (player == null)
        {
            Debug.LogWarning("No player was found, destroying it...");
            // Xóa GameObject
            Destroy(gameObject);
        }

        StartSpawnSequence();
    }

    // Update is called once per frame
    protected bool CanAttack()
    {
        // Nếu enemy chưa đc spawn thì ko làm gì cả
        return enemyRenderer.enabled;
    }

    private void StartSpawnSequence()
    {
        SetRendererVisibility(false);

        // Add animation to spawn indicator (scale up & down)
        Vector3 targetScale = spawnIndicator.transform.localScale * scaleValue;

        // Thực hiện scale animation 4 lần sau đó gọi hàm SpawnSequenceComplete
        LeanTween.scale(spawnIndicator.gameObject, targetScale, .5f).setLoopPingPong(4).setOnComplete(SpawnSequenceComplete);

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

    public void TakeDamage(int damage, bool isCriticalHit)
    {
        int realDamage = Mathf.Min(damage, health);
        health -= realDamage;

        // call onDamageTaken action
        onDamageTaken?.Invoke(damage, transform.position, isCriticalHit);

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

    void OnDrawGizmos()
    {
        if (!showGizmos) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerDetectionRadius);
    }
}

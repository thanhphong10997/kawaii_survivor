using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
public class Enemy : MonoBehaviour
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

    [Header("Effects")]
    [SerializeField] private ParticleSystem passAwayParticle;

    [Header("Attack")]
    [SerializeField] private int damage;
    [SerializeField] private float attackFrequency;
    private float attackTimer;
    private float attackDelay;

    [Header("Debug")]
    [SerializeField] private bool showGizmos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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

        // Hide Renderer
        // enemyRenderer.enabled = false;
        // Show spawn indicator
        // spawnIndicator.enabled = true;

        // Thời gian delay sẽ bằng 1s / số lần đánh
        attackDelay = 1f / attackFrequency;
    }

    // Update is called once per frame
    void Update()
    {
        // attackTimer là số giây trên mỗi Frame, khi số giây trên mỗi Frame >= số giây delay thì sẽ đánh 1 cái rồi reset attackTimer
        if (attackTimer >= attackDelay) TryAttack();
        else
            Wait();
    }

    private void StartSpawnSequence()
    {
        SetRendererVisibility(false);

        // Add animation to spawn indicator (scale up & down)
        Vector3 targetScale = spawnIndicator.transform.localScale * scaleValue;

        // Thực hiện scale animation 4 lần sau đó gọi hàm SpawnSequenceComplete
        LeanTween.scale(spawnIndicator.gameObject, targetScale, .5f).setLoopPingPong(4).setOnComplete(SpawnSequenceComplete);

    }


    private void TryAttack()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        // float magnitude = (player.transform.position - transform.position).magnitude;
        // Debug.Log("distance " + distanceToPlayer);
        // Debug.Log("magnitude " + magnitude);

        // playerDectionRadius: phạm vi xung quanh enemy mà khi chạm player thì sẽ kích hoạt Attack
        if (distanceToPlayer <= playerDetectionRadius)
        {
            // PassAwayEffect();
            Attack();
        }
    }

    private void Attack()
    {
        Debug.Log("Attack");

        attackTimer = 0;
    }

    private void Wait()
    {
        attackTimer += Time.deltaTime;
    }

    public Player StorePlayer()
    {
        return player;
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

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerDetectionRadius);
    }
}

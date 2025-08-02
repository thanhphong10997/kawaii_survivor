using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Elements")]
    private Player player;
    private bool hasSpawned;

    [Header("Spawn Sequence Related")]
    [SerializeField] private SpriteRenderer enemyRenderer;
    [SerializeField] private SpriteRenderer spawnIndicator;

    [Header("Settings")]
    [SerializeField] private float playerDetectionRadius;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float scaleValue;

    [Header("Effects")]
    [SerializeField] private ParticleSystem passAwayParticle;

    [Header("Debug")]
    [SerializeField] private bool showGizmos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Tìm GameObject đầu tiên có component Player
        player = FindFirstObjectByType<Player>();
        if (player == null)
        {
            Debug.LogWarning("No player was found, destroying it...");
            // Xóa GameObject
            Destroy(gameObject);
        }

        // Hide Renderer
        enemyRenderer.enabled = false;

        // Show spawn indicator
        spawnIndicator.enabled = true;

        // Add animation to spawn indicator (scale up & down)
        Vector3 targetScale = spawnIndicator.transform.localScale * scaleValue;

        // Thực hiện scale animation 4 lần sau đó gọi hàm SpawnSequenceComplete
        LeanTween.scale(spawnIndicator.gameObject, targetScale, .3f).setLoopPingPong(4).setOnComplete(SpawnSequenceComplete);


    }

    // Update is called once per frame
    void Update()
    {
        if (!hasSpawned) return;

        FollowPlayer();
        TryAttack();

    }

    private void SpawnSequenceComplete()
    {
        // Show the renderer(enemy) after looping animation
        enemyRenderer.enabled = true;

        // Hide spawn indicator
        spawnIndicator.enabled = false;

        // Prevent Following and Attacking during the whole spawn sequence
        hasSpawned = true;
    }

    private void FollowPlayer()
    {
        // Nếu ko dùng normalized thì độ lớn của vector sẽ rất lớn và enemy sẽ luôn luôn di chuyển sát nhân vật
        Vector2 direction = (player.transform.position - transform.position).normalized;

        // Khoảng cách enemy sẽ di chuyển được tính bằng khoảng cách của enemy + khoảng cách giữa enemy và nvat sau đó nhân với tốc độ 
        Vector2 targetPosition = (Vector2)transform.position + direction * moveSpeed * Time.deltaTime;
        transform.position = targetPosition;
    }

    private void TryAttack()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        // float magnitude = (player.transform.position - transform.position).magnitude;
        // Debug.Log("distance " + distanceToPlayer);
        // Debug.Log("magnitude " + magnitude);
        if (distanceToPlayer <= playerDetectionRadius) PassAwayEffect();

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

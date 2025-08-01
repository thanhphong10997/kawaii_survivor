using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Player player;
    [SerializeField] private float moveSpeed;

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
    }

    // Update is called once per frame
    void Update()
    {
        // Nếu ko dùng normalized thì độ lớn của vector sẽ rất lớn và enemy sẽ luôn luôn di chuyển sát nhân vật
        Vector2 direction = (player.transform.position - transform.position).normalized;

        // Khoảng cách enemy sẽ di chuyển được tính bằng khoảng cách của enemy + khoảng cách giữa enemy và nvat sau đó nhân với tốc độ 
        Vector2 targetPosition = (Vector2)transform.position + direction * moveSpeed * Time.deltaTime;
        transform.position = targetPosition;
    }
}

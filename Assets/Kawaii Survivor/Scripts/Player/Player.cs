using UnityEngine;

[RequireComponent(typeof(PlayerHealth), typeof(PlayerLevel))]
public class Player : MonoBehaviour
{
    [Header("Components")]
    public static Player instance;
    private PlayerHealth playerHealth;
    private PlayerLevel playerLevel;
    [SerializeField] private CircleCollider2D playerCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        // Kiểm tra nếu đã có thể hiện của lớp này
        if (instance != null && instance != this)
        {
            // 3. Nếu đã có, hủy đối tượng hiện tại
            Destroy(this.gameObject);
        }
        else
        {
            // Nếu chưa có, đây là thể hiện duy nhất
            instance = this;
            // Đảm bảo đối tượng không bị hủy khi chuyển Scene
            DontDestroyOnLoad(this.gameObject);
        }
    }
    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        playerLevel = GetComponent<PlayerLevel>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        playerHealth.TakeDamage(damage);
    }

    public Vector2 GetCenter()
    {
        return (Vector2)transform.position + playerCollider.offset;
    }

    public bool HasLeveledUp()
    {
        return playerLevel.HasLeveledUp();
    }
}

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
        // Kiểm tra nếu chưa có instance nào đc tạo thì gán đây là instance, nếu có rồi thì destroy object
        if (instance == null) instance = this;
        else Destroy(gameObject);
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

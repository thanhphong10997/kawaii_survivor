using UnityEngine;
[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class EnemyBullet : MonoBehaviour
{

    [Header("Elements")]
    private Rigidbody2D rig;
    private RangeEnemyAttack rangeEnemyAttack;
    private Collider2D _collider;

    [Header("Settings")]
    [SerializeField] private float moveSpeed;

    private int damage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();

        // Đạn sẽ disable sau 5s nếu như ko đụng trúng player
        LeanTween.delayedCall(gameObject, 5, () => rangeEnemyAttack.ReleaseBullet(this));
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Configure(RangeEnemyAttack rangeEnemyAttack)
    {
        this.rangeEnemyAttack = rangeEnemyAttack;
    }

    public void Reload()
    {
        // Ngăn chặn đạn di chuyển
        rig.linearVelocity = Vector2.zero;
        _collider.enabled = true;
    }


    public void Shoot(int damage, Vector2 direction)
    {
        this.damage = damage;
        // Hướng trục x bên phải của đạn (hướng đạn bay) về hướng player
        transform.right = direction;
        rig.linearVelocity = direction * moveSpeed;
    }

    // Khi collider va chạm với 1 Game Object(Player) thì hàm này sẽ được gọi
    void OnTriggerEnter2D(Collider2D collider)
    {
        // Check nếu tìm thấy Player Component, trả true/false
        if (collider.TryGetComponent(out Player player))
        {
            // Nếu đạn trúng player thì hủy tất cả các lệnh LeanTween đã đăng ký
            LeanTween.cancel(gameObject);
            player.TakeDamage(damage);
            _collider.enabled = false;

            rangeEnemyAttack.ReleaseBullet(this);
        }
    }
}

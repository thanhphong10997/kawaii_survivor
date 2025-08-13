using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Bullet : MonoBehaviour
{
    [Header("Elements")]
    private Rigidbody2D rig;
    private Collider2D _collider;
    private RangeWeapon rangeWeapon;
    private Enemy target;

    [Header("Settings")]
    [SerializeField] private float moveSpeed;

    [Header("Layer")]
    [SerializeField] private LayerMask enemyMask;
    private int damage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        rangeWeapon = GetComponent<RangeWeapon>();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Configure(RangeWeapon rangeWeapon)
    {
        this.rangeWeapon = rangeWeapon;
    }

    public void Shoot(int damage, Vector2 direction)
    {
        Invoke("Release", 1);
        this.damage = damage;
        // Hướng trục x bên phải của đạn (hướng đạn bay) về hướng player
        transform.right = direction;
        rig.linearVelocity = direction * moveSpeed;
    }

    public void Reload()
    {
        // Gán lại target = null khi reload scene
        target = null;
        // Ngăn chặn đạn di chuyển
        rig.linearVelocity = Vector2.zero;
        _collider.enabled = true;
    }

    private void Release()
    {
        // Check nếu gameObject ko active có nghĩa là đã release rồi
        if (!gameObject.activeSelf) return;
        rangeWeapon.ReleaseBullet(this);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        // Target ko phải là cùng 1 enemy thì ko attack
        if (target != null) return;

        if (IsInLayerMask(collider.gameObject.layer, enemyMask))
        {

            CancelInvoke();
            // Gán target = enemy hiện tại
            target = collider.GetComponent<Enemy>();
            Attack(target);
            Release();
        }
    }

    private void Attack(Enemy enemy)
    {
        enemy.TakeDamage(damage);
    }

    private bool IsInLayerMask(int layer, LayerMask layerMask)
    {
        return (layerMask.value & (1 << layer)) != 0;
    }

}

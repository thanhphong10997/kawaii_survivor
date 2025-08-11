using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class RangeEnemyAttack : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private EnemyBullet bulletPrefab;

    [Header("Bullet Pooling")]
    private ObjectPool<EnemyBullet> bulletPool;


    [Header("Settings")]
    [SerializeField] private float attackFrequency;
    [SerializeField] private int damage;

    private Player player;
    private float attackTimer;
    private float attackDelay;


    void Awake()
    {
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Dùng Pool để có thể tái sử dụng Object
        bulletPool = new ObjectPool<EnemyBullet>(CreateFunction, ActionOnGet, ActionOnRelease, ActionOnDestroy);

        // Thời gian delay sẽ bằng 1s / số lần đánh
        attackDelay = 1f / attackFrequency;
        // Khi component này đc gọi thì sẽ lập tức attack
        attackTimer = attackDelay;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private EnemyBullet CreateFunction()
    {
        EnemyBullet bulletInstance = Instantiate(bulletPrefab, shootingPoint.position, Quaternion.identity);
        // Lưu thông tin của class tạo ra bulletInstance
        bulletInstance.Configure(this);

        return bulletInstance;
    }


    private void ActionOnGet(EnemyBullet bullet)
    {
        // move đạn đến shootingPoint (trong trường hợp vị trí của đạn có thể khác vị trí shootingPoint)
        bullet.transform.position = shootingPoint.position;
        // Reload có chức năng re-enable collider của đạn 
        bullet.Reload();
        bullet.gameObject.SetActive(true);
    }

    private void ActionOnRelease(EnemyBullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    private void ActionOnDestroy(EnemyBullet bullet)
    {
        Destroy(bullet.gameObject);
    }

    public void ReleaseBullet(EnemyBullet bullet)
    {
        bulletPool.Release(bullet);
    }

    public void MangeShooting()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackDelay)
        {
            Shoot();
            attackTimer = 0;
        }
    }

    private void Shoot()
    {
        Vector2 playerCenterPosition = player.GetCenter();
        Vector2 direction = (playerCenterPosition - (Vector2)shootingPoint.position).normalized;

        // EnemyBullet bullet = Instantiate(bulletPrefab, shootingPoint.position, Quaternion.identity);
        EnemyBullet bullet = bulletPool.Get();
        bullet.Shoot(damage, direction);

    }

    public void StorePlayer(Player player)
    {
        this.player = player;
    }

    public void AutoAim()
    {
        MangeShooting();
    }


}


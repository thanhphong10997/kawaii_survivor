using System;
using UnityEngine;
using UnityEngine.Pool;

public class RangeWeapon : Weapon
{
    [Header("Elements")]
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private Bullet bulletPrefab;

    [Header("Pooling")]
    private ObjectPool<Bullet> bulletPool;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bulletPool = new ObjectPool<Bullet>(CreateFunction, ActionOnGet, ActionOnRelease, ActionOnDestroy);
    }

    // Update is called once per frame
    void Update()
    {
        SmoothAutoAim();
    }

    private Bullet CreateFunction()
    {
        Bullet bulletInstance = Instantiate(bulletPrefab, shootingPoint.position, Quaternion.identity);
        // Lưu thông tin của class tạo ra bulletInstance
        bulletInstance.Configure(this);

        return bulletInstance;
    }


    private void ActionOnGet(Bullet bullet)
    {
        // move đạn đến shootingPoint (trong trường hợp vị trí của đạn có thể khác vị trí shootingPoint)
        bullet.transform.position = shootingPoint.position;
        // Reload có chức năng re-enable collider của đạn 
        bullet.Reload();
        bullet.gameObject.SetActive(true);
    }

    private void ActionOnRelease(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    private void ActionOnDestroy(Bullet bullet)
    {
        Destroy(bullet.gameObject);
    }

    public void ReleaseBullet(Bullet bullet)
    {
        bulletPool.Release(bullet);
    }

    private void SmoothAutoAim()
    {
        Enemy closestEnemy = GetClosestEnemy();
        // Gán targetUpVector theo giá trị Vector hệ tọa độ thế giới, mặc định luôn là (0,1,0) (up thì weapon hướng lên theo trục y, right thì weapon sẽ hướng bên phải trục x...)
        Vector2 targetUpVector = Vector3.up;

        if (closestEnemy != null)
        {
            targetUpVector = (closestEnemy.transform.position - transform.position).normalized;
            // Giúp tâm của weapon chuyển động hướng vào địch khi tấn công
            transform.up = targetUpVector;
            MangeShooting();
            return;
        }

        // Lerp giúp chuyển động mượt mà hơn
        // Nếu ko phát hiện enemy nào gần thì tâm của weapon sẽ chuyển động hướng lên trên (độ mượt phụ thuộc vào airmLerp)
        transform.up = Vector3.Lerp(transform.up, targetUpVector, aimLerp * Time.deltaTime);


    }

    private void MangeShooting()
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
        int damage = GetDamage(out bool isCriticalHit);
        Bullet bulletInstance = bulletPool.Get();
        bulletInstance.Shoot(damage, transform.up, isCriticalHit);
    }

    public override void UpdateStats(PlayerStatsManager playerStatsManager)
    {
        // Initialize weapon stats
        ConfigureStats();
        damage = Mathf.RoundToInt(damage * (1 + playerStatsManager.GetStatValue(Stat.Attack) / 100));
        attackDelay /= 1 + (playerStatsManager.GetStatValue(Stat.AttackSpeed) / 100);
        criticalChance = Mathf.RoundToInt(criticalChance * (1 + playerStatsManager.GetStatValue(Stat.CriticalChance) / 100));
        criticalPercent += playerStatsManager.GetStatValue(Stat.CriticalPercent);
        range += playerStatsManager.GetStatValue(Stat.Range) / 10;
    }
}

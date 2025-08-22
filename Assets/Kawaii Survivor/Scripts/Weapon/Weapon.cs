using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public abstract class Weapon : MonoBehaviour, IPlayerStatsDependency
{
    [field: SerializeField] public WeaponDataSO WeaponData { get; private set; }
    [Header("Settings")]
    [SerializeField] protected float range;
    [SerializeField] protected LayerMask enemyMask;
    [SerializeField] protected float aimLerp;
    [SerializeField] protected int damage;
    [SerializeField] protected float attackDelay;

    [Header("Animations")]
    [SerializeField] protected Animator animator;

    protected float attackTimer;

    [Header("Critical")]
    protected int criticalChance;
    protected float criticalPercent;

    [Header("Level")]
    [field: SerializeField] public int Level { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Lấy ra tất cả các Enemy Object và gán vào Enemy Array
        // Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);


    }



    protected Enemy GetClosestEnemy()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, range, enemyMask);
        Enemy closestEnemy = null;
        float minDistance = range;

        if (enemies.Length <= 0) return null;

        for (int i = 0; i < enemies.Length; i++)
        {
            Enemy enemyChecked = enemies[i].GetComponent<Enemy>();
            float distanceToEnemy = Vector2.Distance(transform.position, enemyChecked.transform.position);
            if (distanceToEnemy < minDistance)
            {
                minDistance = distanceToEnemy;
                closestEnemy = enemyChecked;
            }
        }

        return closestEnemy;
    }

    protected int GetDamage(out bool isCriticalHit)
    {
        isCriticalHit = false;
        // 50% cơ hội đc critical hit
        if (Random.Range(0, 101) <= criticalChance)
        {
            isCriticalHit = true;

            return Mathf.RoundToInt(damage * criticalPercent);
        }

        return damage;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blueViolet;
        Gizmos.DrawWireSphere(transform.position, range);

    }

    public abstract void UpdateStats(PlayerStatsManager playerStatsManager);

    protected void ConfigureStats()
    {
        // Khi weapon đạt lv 3 thì damage sẽ nhân đôi, có thể tùy chỉnh / 3 để tăng hoặc giảm damage khi lên lv
        float multiplier = 1 + (float)Level / 3;
        damage = Mathf.RoundToInt(WeaponData.GetStatValue(Stat.Attack) * multiplier);
        attackDelay = 1f / (WeaponData.GetStatValue(Stat.AttackSpeed) * multiplier);
        criticalChance = Mathf.RoundToInt(WeaponData.GetStatValue(Stat.CriticalChance) * multiplier);
        criticalPercent = WeaponData.GetStatValue(Stat.CriticalPercent) * multiplier;

        if (WeaponData.Prefab.GetType() == typeof(RangeWeapon))
            range = WeaponData.GetStatValue(Stat.Range) * multiplier;
    }

}

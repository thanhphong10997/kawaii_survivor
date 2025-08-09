using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    enum State
    {
        Idle,
        Attack
    };
    private State state;
    [Header("Elements")]
    [SerializeField] private Transform hitDetectionTransform;


    [Header("Settings")]
    [SerializeField] private float range;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private float aimLerp;
    [SerializeField] private float hitRadius;
    [SerializeField] private int damage;
    [SerializeField] private float attackDelay;

    [Header("Animations")]
    [SerializeField] private Animator animator;

    private float attackTimer;
    private List<Enemy> damagedEnemies = new List<Enemy>();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        state = State.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        // Lấy ra tất cả các Enemy Object và gán vào Enemy Array
        // Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        switch (state)
        {
            case State.Idle:
                SmoothAutoAim();
                break;

            case State.Attack:
                Attacking();
                break;
        }

    }

    private void SmoothAutoAim()
    {
        Enemy closestEnemy = GetClosestEnemy();
        // Gán targetUpVector theo giá trị Vector hệ tọa độ thế giới, mặc định luôn là (0,1,0) (up thì weapon hướng lên theo trục y, right thì weapon sẽ hướng bên phải trục x...)
        Vector2 targetUpVector = Vector3.up;

        if (closestEnemy != null)
        {
            targetUpVector = (closestEnemy.transform.position - transform.position).normalized;
            if (attackTimer >= attackDelay)
            {
                StartAttack();
                attackTimer = 0;
            }
            ;
        }

        // Lerp giúp chuyển động mượt mà hơn
        transform.up = Vector3.Lerp(transform.up, targetUpVector, aimLerp * Time.deltaTime);

        // Wait a delay time then attack the enemy
        Wait();
    }

    private Enemy GetClosestEnemy()
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

    private void Attack()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(hitDetectionTransform.position, hitRadius, enemyMask);
        for (int i = 0; i < enemies.Length; i++)
        {
            Enemy currentEnemy = enemies[i].GetComponent<Enemy>();

            // Check if enemy is already in the list or not
            if (!damagedEnemies.Contains(currentEnemy))
            {
                currentEnemy.TakeDamage(damage);
                damagedEnemies.Add(currentEnemy);

            }

        }
    }

    private void Wait()
    {
        attackTimer += Time.deltaTime;
    }

    [NaughtyAttributes.Button]
    private void StartAttack()
    {
        animator.Play("Attack");
        state = State.Attack;
        damagedEnemies.Clear();
        // Điều chỉnh tốc độ animation của weapon. VD attackDelay = 0.5 thì 1s sẽ đánh 2 cái
        animator.speed = 1f / attackDelay;
    }

    private void Attacking()
    {
        Attack();

    }

    private void StopAttack()
    {
        state = State.Idle;
        damagedEnemies.Clear();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blueViolet;
        Gizmos.DrawWireSphere(transform.position, range);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(hitDetectionTransform.position, hitRadius);
    }
}

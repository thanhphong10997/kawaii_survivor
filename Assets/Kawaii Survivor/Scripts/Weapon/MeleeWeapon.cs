using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    enum State
    {
        Idle,
        Attack
    };

    private State state;

    [Header("Elements")]
    [SerializeField] private Transform hitDetectionTransform;
    [SerializeField] private BoxCollider2D hitCollider;

    [Header("Settings")]
    private List<Enemy> damagedEnemies = new List<Enemy>();
    // [SerializeField] private float hitRadius;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        state = State.Idle;
    }

    // Update is called once per frame
    void Update()
    {
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
            // Giúp tâm của weapon chuyển động hướng vào địch khi tấn công
            transform.up = targetUpVector;
            ManageAttack();
        }

        // Lerp giúp chuyển động mượt mà hơn
        // Nếu ko phát hiện enemy nào gần thì tâm của weapon sẽ chuyển động hướng lên trên (độ mượt phụ thuộc vào aimLerp)
        transform.up = Vector3.Lerp(transform.up, targetUpVector, aimLerp * Time.deltaTime);

        // Wait a delay time then attack the enemy
        Wait();
    }

    private void ManageAttack()
    {
        if (attackTimer >= attackDelay)
        {
            StartAttack();
            attackTimer = 0;
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

    private void Attack()
    {
        // Tìm tất cả các Collider chồng lấn lên vòng tròn ảo được tạo ra từ bán kính (hitRadius) và có layer: enemyMask tính từ vị trí weapon
        // Collider2D[] enemies = Physics2D.OverlapCircleAll(hitDetectionTransform.position, hitRadius, enemyMask);

        // Tìm tất cả các Collider chồng lấn nằm trong phạm vi kích thước hình hộp được tạo ra từ Box Collider có layer: enemyMask tính từ vị trí weapon
        Collider2D[] enemies = Physics2D.OverlapBoxAll(hitDetectionTransform.position, hitCollider.bounds.size, hitDetectionTransform.localEulerAngles.z, enemyMask);
        for (int i = 0; i < enemies.Length; i++)
        {
            Enemy currentEnemy = enemies[i].GetComponent<Enemy>();

            // Check if enemy is already in the list or not
            if (!damagedEnemies.Contains(currentEnemy))
            {
                int damage = GetDamage(out bool isCriticalHit);
                currentEnemy.TakeDamage(damage, isCriticalHit);
                damagedEnemies.Add(currentEnemy);

            }

        }
    }

    private void StopAttack()
    {
        state = State.Idle;
        damagedEnemies.Clear();
    }


}

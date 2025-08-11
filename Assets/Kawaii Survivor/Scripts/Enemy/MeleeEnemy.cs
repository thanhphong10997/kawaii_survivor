using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
public class MeleeEnemy : Enemy
{
    [Header("Attacks")]
    [SerializeField] private int damage;
    [SerializeField] private float attackFrequency;
    private float attackTimer;
    private float attackDelay;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        // Thời gian delay sẽ bằng 1s / số lần đánh
        attackDelay = 1f / attackFrequency;
    }

    // Update is called once per frame
    void Update()
    {
        if (!CanAttack()) return;
        // attackTimer là số giây trên mỗi Frame, khi số giây trên mỗi Frame >= số giây delay thì sẽ đánh 1 cái rồi reset attackTimer
        if (attackTimer >= attackDelay) TryAttack();
        else
            Wait();

        // Always follow the player
        enemyMovement.FollowPlayer();
    }


    private void TryAttack()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        // float magnitude = (player.transform.position - transform.position).magnitude;
        // Debug.Log("distance " + distanceToPlayer);
        // Debug.Log("magnitude " + magnitude);

        // playerDectionRadius: phạm vi xung quanh enemy mà khi chạm player thì sẽ kích hoạt Attack
        if (distanceToPlayer <= playerDetectionRadius) Attack();

    }

    private void Attack()
    {
        player.TakeDamage(damage);
        attackTimer = 0;
    }

    private void Wait()
    {
        attackTimer += Time.deltaTime;
    }


}

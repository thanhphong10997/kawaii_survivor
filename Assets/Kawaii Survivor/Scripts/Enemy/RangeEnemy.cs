using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement), typeof(RangeEnemyAttack))]
public class RangeEnemy : Enemy
{
    private RangeEnemyAttack rangeAttack;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        rangeAttack = GetComponent<RangeEnemyAttack>();
        rangeAttack.StorePlayer(player);

    }

    // Update is called once per frame
    void Update()
    {
        // attackTimer là số giây trên mỗi Frame, khi số giây trên mỗi Frame >= số giây delay thì sẽ đánh 1 cái rồi reset attackTimer
        if (!CanAttack()) return;
        ManageAttack();
        transform.localScale = player.transform.position.x > transform.position.x ? Vector3.one : Vector3.one.With(x: -1);
    }


    private void ManageAttack()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        // float magnitude = (player.transform.position - transform.position).magnitude;
        // Debug.Log("distance " + distanceToPlayer);
        // Debug.Log("magnitude " + magnitude);

        if (distanceToPlayer > playerDetectionRadius) enemyMovement.FollowPlayer();
        else TryAttack();
    }

    private void TryAttack()
    {
        rangeAttack.AutoAim();

    }


}

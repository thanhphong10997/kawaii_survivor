using System;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Elements")]
    private Player player;

    [Header("Settings")]
    [SerializeField] private float moveSpeed;

    // Update is called once per frame

    void Update()
    {
        if (player != null) FollowPlayer();

    }

    public void StorePlayer(Player player)
    {
        this.player = player;
    }

    private void FollowPlayer()
    {
        // Nếu ko dùng normalized thì độ lớn của vector sẽ rất lớn và enemy sẽ luôn luôn di chuyển sát nhân vật
        Vector2 direction = (player.transform.position - transform.position).normalized;

        // Khoảng cách enemy sẽ di chuyển được tính bằng khoảng cách hiện tại của enemy + khoảng cách giữa enemy và nvat sau đó nhân với tốc độ 
        Vector2 targetPosition = (Vector2)transform.position + direction * moveSpeed * Time.deltaTime;
        transform.position = targetPosition;
    }
}

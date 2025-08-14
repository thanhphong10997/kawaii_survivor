using Unity.VisualScripting;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    [Header("Collider")]
    [SerializeField] private CircleCollider2D daveCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    void Update()
    {
    }

    // Cách 1: Detect candy component khi collider của player chạm collider của candy
    // void FixedUpdate()
    // {
    //     Collider2D[] candyColliders = Physics2D.OverlapCircleAll((Vector2)transform.position + daveCollider.offset, daveCollider.radius);
    //     foreach (Collider2D collider in candyColliders)
    //     {
    //         if (collider.TryGetComponent(out Candy candy)) Destroy(candy.gameObject);
    //     }
    // }


    // Cách 2: Detect candy component khi collider của player chạm collider của candy
    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Debug.Log("Đã có va chạm Trigger với: " + collider.gameObject.name);

        // Lấy candy component từ collider(player)
        if (collider.TryGetComponent(out Candy candy))
        {
            // Nếu ko chạm collider của Dave (chạm các collider khác như cây cối enemy...) thì return
            if (!collider.IsTouching(daveCollider)) return;

            Destroy(candy.gameObject);
        }
    }
}

using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Rigidbody2D rig;
    [SerializeField] private MobileJoystick joystick;

    [Header("Settings")]
    [SerializeField] private float moveSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // rig = GetComponent<Rigidbody2D>()

        // Move character to the right until the character outside of the screen
        // velocity: vận tốc

        // Có hay ko cũng đc
        rig.linearVelocity = Vector2.right;
    }

    // Code related to physics(velocity in this case) should be put in FixedUpdate()
    private void FixedUpdate()
    {
        // Time.deltaTime đảm bảo tốc độ di chuyển trên tất cả các thiết bị là như nhau
        rig.linearVelocity = joystick.GetMoveVector() * moveSpeed * Time.deltaTime;
    }

    // Update is called once per frame

}


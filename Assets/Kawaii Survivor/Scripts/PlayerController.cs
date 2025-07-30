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
        rig.linearVelocity = Vector2.right;
    }

    private void FixedUpdate()
    {
        rig.linearVelocity = joystick.GetMoveVector() * moveSpeed * Time.deltaTime;
    }

    // Update is called once per frame

}


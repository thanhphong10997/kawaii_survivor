using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController1 : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rig;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // rig = GetComponent<Rigidbody2D>()
        rig.linearVelocity = Vector2.right;
    }

    // Update is called once per frame
    void Update()
    {

    }
}


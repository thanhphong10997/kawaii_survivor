using UnityEngine;

[RequireComponent(typeof(PlayerHealth))]
public class Player : MonoBehaviour
{
    [Header("Components")]
    private PlayerHealth playerHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        playerHealth.TakeDamage(damage);
    }
}

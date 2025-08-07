using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField] private float range;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private float aimLerp;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Lấy ra tất cả các Enemy Object và gán vào Enemy Array
        // Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

        SmoothAutoAim();

    }

    private void SmoothAutoAim()
    {
        Enemy closestEnemy = GetClosestEnemy();
        // Gán targetUpVector theo giá trị Vector hệ tọa độ thế giới, mặc định luôn là (0,1,0) (up thì weapon hướng lên theo trục y, right thì weapon sẽ hướng bên phải trục x...)
        Vector2 targetUpVector = Vector3.up;

        if (closestEnemy != null) targetUpVector = (closestEnemy.transform.position - transform.position).normalized;

        // Lerp giúp chuyển động mượt mà hơn
        transform.up = Vector3.Lerp(transform.up, targetUpVector, aimLerp * Time.deltaTime);
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blueViolet;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}

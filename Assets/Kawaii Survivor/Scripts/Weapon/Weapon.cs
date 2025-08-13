using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public abstract class Weapon : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField] protected float range;
    [SerializeField] protected LayerMask enemyMask;
    [SerializeField] protected float aimLerp;
    [SerializeField] protected int damage;
    [SerializeField] protected float attackDelay;

    [Header("Animations")]
    [SerializeField] protected Animator animator;

    protected float attackTimer;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Lấy ra tất cả các Enemy Object và gán vào Enemy Array
        // Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);


    }



    protected Enemy GetClosestEnemy()
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

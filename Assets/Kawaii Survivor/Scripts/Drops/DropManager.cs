using System;
using UnityEngine;

public class DropManager : MonoBehaviour
{
    [SerializeField] private Candy candyPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Enemy.onPassedAway += EnemyPassedAwayCallback;
    }

    void OnDestroy()
    {
        Enemy.onPassedAway -= EnemyPassedAwayCallback;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void EnemyPassedAwayCallback(Vector2 enemyPosition)
    {
        Candy candyInstance = Instantiate(candyPrefab, enemyPosition, Quaternion.identity, transform);
    }
}

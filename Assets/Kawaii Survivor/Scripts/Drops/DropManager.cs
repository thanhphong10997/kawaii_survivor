using System;
using UnityEngine;

using Random = UnityEngine.Random;
public class DropManager : MonoBehaviour
{
    [SerializeField] private Candy candyPrefab;
    [SerializeField] private Cash cashPrefab;

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
        bool shouldSpawnCash = Random.Range(0, 101) <= 20;
        GameObject dropObject = shouldSpawnCash ? cashPrefab.gameObject : candyPrefab.gameObject;
        GameObject dropInstance = Instantiate(dropObject, enemyPosition, Quaternion.identity, transform);
    }
}

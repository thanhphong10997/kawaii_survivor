using System;
using UnityEngine;
using UnityEngine.Pool;

using Random = UnityEngine.Random;
public class DropManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Candy candyPrefab;
    [SerializeField] private Cash cashPrefab;
    [SerializeField] private Chest chestPrefab;

    [Header("Pooling")]
    private ObjectPool<Candy> candyPool;
    private ObjectPool<Cash> cashPool;
    private ObjectPool<Chest> chestPool;

    [Header("Settings")]
    [SerializeField][Range(0, 100)] private int cashDropChance;
    [SerializeField][Range(0, 100)] private int chestDropChance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        // subscribe action
        Enemy.onPassedAway += EnemyPassedAwayCallback;
        Candy.onCollected += ReleaseCandy;
        Cash.onCollected += ReleaseCash;
        Chest.onCollected += ReleaseChest;

    }

    void Start()
    {
        candyPool = new ObjectPool<Candy>(CandyCreateFunction, CandyActionOnGet, CandyActionOnRelease, CandyActionOnDestroy);
        cashPool = new ObjectPool<Cash>(CashCreateFunction, CashActionOnGet, CashActionOnRelease, CashActionOnDestroy);
        chestPool = new ObjectPool<Chest>(ChestCreateFunction, ChestActionOnGet, ChestActionOnRelease, ChestActionOnDestroy);

    }

    void OnDestroy()
    {
        Enemy.onPassedAway -= EnemyPassedAwayCallback;
        Candy.onCollected -= ReleaseCandy;
        Cash.onCollected -= ReleaseCash;
        Chest.onCollected -= ReleaseChest;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private Candy CandyCreateFunction() => Instantiate(candyPrefab, transform);
    private void CandyActionOnGet(Candy candy) => candy.gameObject.SetActive(true);
    private void CandyActionOnRelease(Candy candy) => candy.gameObject.SetActive(false);
    private void CandyActionOnDestroy(Candy candy) => Destroy(candy.gameObject);

    private Cash CashCreateFunction() => Instantiate(cashPrefab, transform);
    private void CashActionOnGet(Cash cash) => cash.gameObject.SetActive(true);
    private void CashActionOnRelease(Cash cash) => cash.gameObject.SetActive(false);
    private void CashActionOnDestroy(Cash cash) => Destroy(cash.gameObject);

    private Chest ChestCreateFunction() => Instantiate(chestPrefab, transform);
    private void ChestActionOnGet(Chest chest) => chest.gameObject.SetActive(true);
    private void ChestActionOnRelease(Chest chest) => chest.gameObject.SetActive(false);
    private void ChestActionOnDestroy(Chest chest) => Destroy(chest.gameObject);


    private void EnemyPassedAwayCallback(Vector2 enemyPosition)
    {
        bool shouldSpawnCash = Random.Range(0, 101) <= cashDropChance;
        DroppableCurrency dropObject = shouldSpawnCash ? cashPool.Get() : candyPool.Get();
        dropObject.transform.position = enemyPosition;
        // DroppableCurrency dropInstance = Instantiate(dropObject, enemyPosition, Quaternion.identity, transform);

        TryDropChest(enemyPosition);
    }

    private void TryDropChest(Vector2 spawnPosition)
    {
        bool shouldDropChest = Random.Range(0, 101) <= chestDropChance;

        if (!shouldDropChest) return;

        Instantiate(chestPrefab, spawnPosition, Quaternion.identity, transform);
    }

    private void ReleaseCandy(Candy candy) => candyPool.Release(candy);
    private void ReleaseCash(Cash cash) => cashPool.Release(cash);
    private void ReleaseChest(Chest chest) => chestPool.Release(chest);
}

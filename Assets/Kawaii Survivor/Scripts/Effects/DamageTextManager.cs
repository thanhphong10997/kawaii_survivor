using System;
using UnityEngine;
using UnityEngine.Pool;

public class DamageTextManager : MonoBehaviour
{
    [SerializeField] private DamageText damageTextPrefab;
    private ObjectPool<DamageText> damageTextPool;

    void Awake()
    {
        // subscribe to action
        MeleeEnemy.onDamageTaken += EnemyHitCallback;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Dùng Pool để có thể tái sử dụng Object
        damageTextPool = new ObjectPool<DamageText>(CreateFunction, ActionOnGet, ActionOnRelease, ActionOnDestroy);
    }


    // Clear action if the game scene reload 
    void OnDestroy()
    {
        MeleeEnemy.onDamageTaken -= EnemyHitCallback;
    }

    private DamageText CreateFunction()
    {
        return Instantiate(damageTextPrefab, transform);

    }

    // Hàm ActionOnGet set gì thì ActionOnRelease phải set ngược lại 
    // Hàm này giúp config text khi Get từ damageInstance
    private void ActionOnGet(DamageText damageText)
    {
        damageText.gameObject.SetActive(true);
    }
    private void ActionOnRelease(DamageText damageText)
    {
        damageText.gameObject.SetActive(false);
    }
    private void ActionOnDestroy(DamageText damageText)
    {
        Destroy(damageText.gameObject);
    }



    private void EnemyHitCallback(int damage, Vector2 enemyPosition)
    {
        DamageText damageInstance = damageTextPool.Get();

        Vector3 spawnPosition = enemyPosition + (Vector2.up * 1.5f) + (Vector2.left * 0.2f);
        damageInstance.transform.position = spawnPosition;

        damageInstance.StartAnimation(damage);

        // Release damageInstance sau 1s kể từ lúc start animation
        LeanTween.delayedCall(1, () => damageTextPool.Release(damageInstance));
    }


}

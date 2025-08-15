using System.Collections;
using UnityEngine;

public abstract class DroppableCurrency : MonoBehaviour, ICollectable
{
    private bool collected;

    // Trong trường hợp cash hoặc candy được re-enable thì phải set collected = false để về trạng thái chưa collect vì ta đang tái sử dụng Pool cho candy và cash
    void OnEnable()
    {
        collected = false;
    }

    public void Collect(Transform playerTransform)
    {
        if (collected) return;

        collected = true;
        StartCoroutine(MoveTowardsPlayer(playerTransform));
    }

    IEnumerator MoveTowardsPlayer(Transform playerTransform)
    {
        float timer = 0;
        Vector2 initialPosition = transform.position;

        while (timer < 1)
        {
            transform.position = Vector2.Lerp(initialPosition, playerTransform.position, timer);
            timer += Time.deltaTime;
            yield return null;
        }

        Collected();
    }

    // Dùng abstract bởi vì candy và cash sẽ có những hành vi khác nhau vì chúng có đơn vị khác nhau 
    // Khai báo phương thức abstract trong 1 class abstract để bắt buộc các class con kế thừa phải thực thi phương thức này, tạo nên 1 sự nhất quán giữa các class con
    protected abstract void Collected();
}

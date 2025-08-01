using UnityEngine;

public class SpriteSorter : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Nhân 10 là để so sánh cả phần thập phân sau dấu chấm vì postion là kiểu float và tọa độ của trục x và y trong map là khoảng 11 ô (x:11, y:11). 
        // Nếu ko nhân 10 thì chỉ so sánh 1 số đầu sẽ ko chính xác, VD: 6,1 với 6,2 chỉ so sánh số 6 thì Unity ko phân biệt được số nào lớn hơn, nhân 10 => 61 vs 62 thì so sánh được 
        spriteRenderer.sortingOrder = -(int)(transform.position.y * 10);
    }
}

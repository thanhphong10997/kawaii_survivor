using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Animator damageTextAnimator;
    [SerializeField] private TextMeshPro damageText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    [NaughtyAttributes.Button]
    public void StartAnimation(int damage, bool isCriticalHit)
    {
        Debug.Log("isCriticalHit:" + isCriticalHit);
        damageText.text = damage.ToString();
        damageText.color = isCriticalHit ? Color.red : Color.white;
        damageTextAnimator.Play("Jump Up and Fade Out");
    }
}

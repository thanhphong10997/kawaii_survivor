using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

// Thêm option khi chuột phải Create => Thêm option Script Object -> New Character Data, order: 0 => Layer sẽ nằm ở trên, số càng cao thì layer sẽ nằm dưới
// Khi tạo file New Character Data từ Scriptable Object, New Character Data sẽ tự động kế thừa CharacterDataSO script
[CreateAssetMenu(fileName = "Character Data", menuName = "Scriptable Object/New Character Data", order = 0)]
public class CharacterDataSO : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public int PurchasePrice { get; private set; }

    // HorizontalLine => Tạo 1 đường line cắt ngang chia cắt
    [HorizontalLine]
    [SerializeField] private float attack;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float criticalChance;
    [SerializeField] private float criticalPercent;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxHealth;
    [SerializeField] private float range;
    [SerializeField] private float healthRecoverySpeed;
    [SerializeField] private float armor;
    [SerializeField] private float luck;
    [SerializeField] private float dodge;
    [SerializeField] private float lifeSteal;

    public Dictionary<Stat, float> BaseStats
    {
        get
        {
            return new Dictionary<Stat, float>
            {
                {Stat.Attack, attack},
                {Stat.AttackSpeed, attackSpeed},
                {Stat.CriticalChance, criticalChance},
                {Stat.CriticalPercent, criticalPercent},
                {Stat.MoveSpeed, moveSpeed},
                {Stat.MaxHealth, maxHealth},
                {Stat.Range, range},
                {Stat.HealthRecoverySpeed, healthRecoverySpeed},
                {Stat.Armor, armor},
                {Stat.Luck, luck},
                {Stat.Dodge, dodge},
                {Stat.Lifesteal, lifeSteal}

            };
        }
        private set { }
    }

}

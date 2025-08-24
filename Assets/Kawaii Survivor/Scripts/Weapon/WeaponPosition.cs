using UnityEngine;

public class WeaponPosition : MonoBehaviour
{
    [Header("Elements")]
    public Weapon Weapon { get; private set; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AssignWeapon(Weapon weapon, int weaponLevel)
    {
        // Khởi tạo game object weapon từ prefab
        Weapon = Instantiate(weapon, transform);

        // Chỉnh vị trí và rotation của weapon trùng với object cha
        Weapon.transform.localPosition = Vector3.zero;
        Weapon.transform.localRotation = Quaternion.identity;

        Weapon.UpgradeTo(weaponLevel);
    }
}

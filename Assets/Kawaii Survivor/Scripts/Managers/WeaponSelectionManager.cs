using NaughtyAttributes.Test;
using UnityEngine;

// WeaponSelectionManager sẽ xuất hiện dựa theo game state
public class WeaponSelectionManager : MonoBehaviour, IGameStateListener
{
    [Header("Elements")]
    [SerializeField] private Transform containersParent;
    [SerializeField] private WeaponSelectionContainer weaponContainerPrefab;

    [Header("Data")]
    [SerializeField] private WeaponDataSO[] starterWeapon;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GameStateChangedCallback(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.WEAPONSELECTION:
                Configure();
                break;
        }
    }

    [NaughtyAttributes.Button]
    private void Configure()
    {
        // Clean our parent, no children
        containersParent.Clear();

        // Generate weapon containers
        for (int i = 0; i < 3; i++)
        {
            GenerateWeaponContainer();
        }
    }

    private void GenerateWeaponContainer()
    {
        WeaponSelectionContainer weaponContainerInstance = Instantiate(weaponContainerPrefab, containersParent);
        WeaponDataSO weaponData = starterWeapon[Random.Range(0, starterWeapon.Length)];

        // Khởi tạo level cho weapon
        int level = Random.Range(0, 4);

        weaponContainerInstance.Configure(weaponData.Sprite, weaponData.Name, level);
        weaponContainerInstance.Button.onClick.RemoveAllListeners();
        // Lý do thêm weaponContainerInstance để biết nút nào được nhấn, weaponData để truy cập text của weapon được nhấn
        weaponContainerInstance.Button.onClick.AddListener(() => WeaponSelectedCallback(weaponContainerInstance, weaponData));

    }

    private void WeaponSelectedCallback(WeaponSelectionContainer containerInstance, WeaponDataSO weaponData)
    {
        // Lặp qua các children nằm bên trong containersParent có type là WeaponSelectionContainer
        foreach (WeaponSelectionContainer container in containersParent.GetComponentsInChildren<WeaponSelectionContainer>())
        {
            if (container == containerInstance) container.Select();
            else container.Deselect();
        }
    }

}

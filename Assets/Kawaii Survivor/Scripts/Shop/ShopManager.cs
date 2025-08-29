using UnityEngine;

// Shop hiển thị cả thẻ Weapon và thẻ Object
public class ShopManager : MonoBehaviour, IGameStateListener
{
    [Header("Elements")]
    [SerializeField] private Transform containerParents;
    [SerializeField] private ShopItemContainer shopItemContainerPrefab;



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
        if (gameState == GameState.SHOP) Configure();
    }

    private void Configure()
    {
        containerParents.Clear();
        int containersToAdd = 6;
        int weaponContainerCount = Random.Range(Mathf.Min(2, containersToAdd), containersToAdd);   // containersToAdd có thể là 0 nên dùng hàm Min
        int objectContainerCount = containersToAdd - weaponContainerCount;

        for (int i = 0; i < weaponContainerCount; i++)
        {
            ShopItemContainer weaponContainerInstance = Instantiate(shopItemContainerPrefab, containerParents);
            WeaponDataSO randomWeapon = ResourcesManager.GetRandomWeapon();
            weaponContainerInstance.Configure(randomWeapon, Random.Range(0, 2));
        }
        for (int i = 0; i < objectContainerCount; i++)
        {
            ShopItemContainer objectContainerInstance = Instantiate(shopItemContainerPrefab, containerParents);
            ObjectDataSO randomObject = ResourcesManager.GetRandomObject();

            objectContainerInstance.Configure(randomObject);
        }
    }
}

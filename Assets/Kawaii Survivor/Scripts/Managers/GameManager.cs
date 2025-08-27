using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    void Awake()
    {
        // Kiểm tra nếu chưa có instance nào đc tạo thì gán đây là instance, nếu có rồi thì destroy object
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Application.targetFrameRate = 60;
        SetGameState(GameState.MENU);
    }

    public void StartGame() => SetGameState(GameState.GAME);
    public void StartWeaponSelection() => SetGameState(GameState.WEAPONSELECTION);
    public void StartShop() => SetGameState(GameState.SHOP);

    // Hàm có tác dụng thông báo cho các script khác biết state đã thay đổi
    public void SetGameState(GameState gameState)
    {
        IEnumerable<IGameStateListener> gameStateListeners = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IGameStateListener>();    // Kiểm tra nếu Object có type MonoBehaviour có thừa hưởng interface IGameStateListener
        foreach (IGameStateListener gameStateListener in gameStateListeners)
        {
            gameStateListener.GameStateChangedCallback(gameState);
        }


    }

    public void WaveCompletedCallback()
    {
        // Check nếu player lên level hoặc nhặt được rương thì game state -> wave transition
        if (Player.instance.HasLeveledUp() || WaveTransitionManager.instance.HasCollectedChest())
        {
            SetGameState(GameState.WAVETRANSITION);
        }
        else
        {
            SetGameState(GameState.SHOP);
        }
    }

    public void ManageGameover()
    {
        SceneManager.LoadScene(0);
    }
}

public interface IGameStateListener
{
    void GameStateChangedCallback(GameState gameState);
}

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    void Awake()
    {
        // Kiểm tra nếu đã có thể hiện của lớp này
        if (instance != null && instance != this)
        {
            // 3. Nếu đã có, hủy đối tượng hiện tại
            Destroy(this.gameObject);
        }
        else
        {
            // Nếu chưa có, đây là thể hiện duy nhất
            instance = this;
            // Đảm bảo đối tượng không bị hủy khi chuyển Scene
            DontDestroyOnLoad(this.gameObject);
        }
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
        if (Player.instance.HasLeveledUp())
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

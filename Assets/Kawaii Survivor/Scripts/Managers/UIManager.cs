using System.Collections.Generic;
using UnityEngine;

// Kế thừa interface IGameStateListener để có thể biết được khi nào game State thay đổi
public class UIManager : MonoBehaviour, IGameStateListener
{
    [Header("Panels")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject waveTransitionPanel;
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private GameObject weaponSelectionPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject stageCompletePanel;

    private List<GameObject> panels = new List<GameObject>();

    void Awake()
    {
        panels.AddRange(new GameObject[]{
        menuPanel,
        weaponSelectionPanel,
        gamePanel,
        gameOverPanel,
        waveTransitionPanel,
        stageCompletePanel,
        shopPanel
    });
    }
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
            case GameState.MENU:
                ShowPanel(menuPanel);
                break;
            case GameState.WEAPONSELECTION:
                ShowPanel(weaponSelectionPanel);
                break;
            case GameState.GAME:
                ShowPanel(gamePanel);
                break;
            case GameState.GAMEOVER:
                ShowPanel(gameOverPanel);
                break;
            case GameState.STAGECOMPLETE:
                ShowPanel(stageCompletePanel);
                break;
            case GameState.WAVETRANSITION:
                ShowPanel(waveTransitionPanel);
                break;
            case GameState.SHOP:
                ShowPanel(shopPanel);
                break;

        }
    }

    private void ShowPanel(GameObject panel, bool hidePreviousPanels = true)
    {
        if (hidePreviousPanels)
        {

            foreach (GameObject p in panels)
            {
                // Cách 1:
                p.SetActive(p == panel);

                // Cách 2:
                // if (p == panel) p.SetActive(true);
                // else p.SetActive(false);
            }
        }
        else
        {
            panel.SetActive(true);
        }
    }
}

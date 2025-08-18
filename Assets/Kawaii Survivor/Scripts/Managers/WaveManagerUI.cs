using TMPro;
using UnityEngine;

public class WaveManagerUI : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI waveTimer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateWaveText(string text) => waveText.text = text;
    public void UpdateWaveTimer(string timerText) => waveTimer.text = timerText;
}

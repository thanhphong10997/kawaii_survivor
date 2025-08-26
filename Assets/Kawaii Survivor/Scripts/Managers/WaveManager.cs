using System.Collections.Generic;
using System.Runtime.CompilerServices;
using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(WaveManagerUI))]
public class WaveManager : MonoBehaviour, IGameStateListener
{
    [Header("Elements")]
    [SerializeField] private Player player;
    private WaveManagerUI waveManagerUI;

    [Header("Waves")]
    [SerializeField] Wave[] waves;

    [Header("Settings")]
    [SerializeField] private float waveDuration;
    private List<float> localCounters = new List<float>();
    private float timer;
    private int currentWaveIndex;

    private bool isTimerOn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        waveManagerUI = GetComponent<WaveManagerUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTimerOn) return;
        if (timer < waveDuration)
        {
            string timerCountDown = ((int)(waveDuration - timer)).ToString();
            waveManagerUI.UpdateWaveTimer(timerCountDown);
            ManageCurrentWave();

        }
        else StartWaveTransition();
    }

    private void StartWave(int waveIndex)
    {
        isTimerOn = true;
        localCounters.Clear();
        timer = 0;
        waveManagerUI.UpdateWaveText($"Wave {currentWaveIndex + 1} / {waves.Length}");
        foreach (WaveSegment waveSegment in waves[waveIndex].segments) localCounters.Add(1);
    }

    private void ManageCurrentWave()
    {
        Wave currentWave = waves[currentWaveIndex];
        for (int i = 0; i < currentWave.segments.Count; i++)
        {
            WaveSegment segment = currentWave.segments[i];
            float tStart = segment.tStartEnd.x / 100 * waveDuration;
            float tEnd = segment.tStartEnd.y / 100 * waveDuration;

            // Đi tới segment tiếp theo
            if (timer < tStart || timer > tEnd) continue;

            // thời gian tính từ lúc bắt đầu một segment
            float timeSinceSegmentStart = timer - tStart;

            // Thời gian delay để spawn enemy trong 1s
            float spawnDelay = 1f / segment.spawnFrequency;

            //  localCounters rất quan trọng vì có tác dụng chỉ spawn số lượng =  spawnFrequency trong mỗi giây, nếu ko có localCounters thì enemy sẽ spawn mỗi frame
            if (timeSinceSegmentStart / spawnDelay > localCounters[i])
            {
                Instantiate(segment.prefab, GetSpawnPosition(), Quaternion.identity, transform);
                localCounters[i]++;
            }
        }
        timer += Time.deltaTime;

    }
    private Vector2 GetSpawnPosition()
    {
        Vector2 direction = Random.onUnitSphere;
        //  Random.onUnitSphere trả ra Vector3 đã đc normalize nhưng offset là Vector2 nên phải normalize thêm 1 lần 
        Vector2 offset = direction.normalized * Random.Range(6, 10);
        Vector2 targetPosition = (Vector2)player.transform.position + offset;

        targetPosition.x = Mathf.Clamp(targetPosition.x, -18, 18);
        targetPosition.y = Mathf.Clamp(targetPosition.y, -8, 8);


        return targetPosition;
    }

    private void StartWaveTransition()
    {
        isTimerOn = false;
        DefeatAllEnemies();

        currentWaveIndex++;
        if (currentWaveIndex >= waves.Length)
        {
            Debug.Log("Waves completed");
            waveManagerUI.UpdateWaveText("Stage Completed");
            waveManagerUI.UpdateWaveTimer("");
            GameManager.instance.SetGameState(GameState.STAGECOMPLETE);
        }
        // Dùng singleton cho Game Manager, lấy hàm WaveCompletedCallback thông qua instance mà k cần reference tới Game Manager
        else GameManager.instance.WaveCompletedCallback();


    }

    private void DefeatAllEnemies()
    {
        foreach (Enemy enemy in transform.GetComponentsInChildren<Enemy>())
            enemy.PassAwayAfterWave();
    }

    private void StartNextWave()
    {
        StartWave(currentWaveIndex);
    }

    public void GameStateChangedCallback(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.GAME:
                StartNextWave();
                break;
            case GameState.GAMEOVER:
                isTimerOn = false;
                DefeatAllEnemies();
                break;
        }
    }

    // Khai báo wave
    [System.Serializable]
    public struct Wave
    {
        public string name;
        public List<WaveSegment> segments;
    }

    // Khai báo wave segments
    [System.Serializable]
    public struct WaveSegment
    {
        // Cách 1:
        // public float t0;  // time start
        // public float t1;  // time end

        // Cách 2:
        [MinMaxSlider(0, 100)] public Vector2 tStartEnd;   // 0 -> 100% trên tổng thời gian hoặc duration

        public float spawnFrequency;   // Số lần spawn trong 1s
        public GameObject prefab;
    }
}

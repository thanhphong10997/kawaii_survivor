using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Waves")]
    [SerializeField] Wave[] waves;

    [Header("Settings")]
    [SerializeField] private float waveDuration;
    private List<float> localCounters = new List<float>();
    private float timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        localCounters.Add(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < waveDuration) ManageCurrentWave();
    }

    private void ManageCurrentWave()
    {
        Wave currentWave = waves[0];
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
                Instantiate(segment.prefab, Vector2.zero, Quaternion.identity, transform);
                localCounters[i]++;
            }
        }
        timer += Time.deltaTime;
    }

    [System.Serializable]
    public struct Wave
    {
        public string name;
        public List<WaveSegment> segments;
    }

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

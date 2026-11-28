using System;
using System.Collections.Generic;
using UnityEngine;

public class BpmNoteSpawner : MonoBehaviour
{
    [Header("ノーツ関連")]
    [SerializeField] private GameObject notePrefab; // ノーツのプレハブ
    [SerializeField] private Transform noteSpawnPoint; // ノーツの出現位置
    [SerializeField] private Transform judgeLine; // 判定ラインの位置
    [SerializeField] private float noteSpeed = 5f; // ノーツの落下速度(NoteMoverと同じ値にする)

    [Header("楽曲・チャート")]
    [SerializeField] private MusicChart musicChart; // ノーツのBPMチャート情報
    [SerializeField] private MusicPlayer musicPlayer; // 楽曲情報

    private readonly List<float> hitTimes = new List<float>(); // ノーツのヒットタイムリスト
    private float fallTime; // 落下時間(秒)
    private int index = 0; // 現在のノーツ生成インデックス

    public enum KeyType
    {
        A,
        S,
        D,
    }

    public KeyType keyType; // 担当するキー

    void Start()
    {
        // 設定値のチェック
        if (!notePrefab || !noteSpawnPoint || !judgeLine)
        {
            Debug.LogWarning("ノーツのプレハブ、出現位置、判定ラインのいずれかが設定されていません。");
            return;
        }

        // 落下時間を計算
        float distance = Mathf.Abs(noteSpawnPoint.position.y - judgeLine.position.y);
        fallTime = distance / noteSpeed;

        // BPMから1拍の秒数を計算
        float secPerBeat = 60f / musicChart.bpm;

        // 何ステップ（四分 / 八分 / 十六分）分のノーツを出すか
        int totalSteps = musicChart.beats * musicChart.beatsPerBar * musicChart.subdivision;
        Debug.Log(totalSteps);
        hitTimes.Clear();

        for (int step = 0; step < totalSteps; step++)
        {
            int[] pattern = Array.ConvertAll(musicChart.pattern[step].Split(','), int.Parse);
            if (keyType == KeyType.A && pattern[0] == 0) continue;
            if (keyType == KeyType.S && pattern[1] == 0) continue;
            if (keyType == KeyType.D && pattern[2] == 0) continue;

            float beatIndex = step / (float)musicChart.subdivision; // 現在のビートインデックス

            // 曲開始からのPerfect 中心タイミング（秒）
            float hitTime = musicChart.offset + beatIndex * secPerBeat;
            hitTimes.Add(hitTime);
        }

        // 曲の再生
        musicPlayer.MusicPlay();
    }

    void Update()
    {
        if (index >= hitTimes.Count) return;
        if (!notePrefab) return;

        // 現在の「曲時間」
        float currentTime = musicPlayer.CurrentTime;

        float spawnTime = hitTimes[index] - fallTime;

        if (currentTime >= spawnTime)
        {
            SpawnNote();
            index++;
        }
    }

    private void SpawnNote()
    {
        Instantiate(notePrefab, noteSpawnPoint.position, Quaternion.identity);
    }
}

using System.Collections.Generic;
using UnityEngine;

public class BpmNoteSpawner : MonoBehaviour
{
    [Header("ノーツ関連")]
    [SerializeField] private GameObject notePrefab; // ノーツのプレハブ
    [SerializeField] private Transform noteSpawnPoint; // ノーツの出現位置
    [SerializeField] private Transform judgeLine; // 判定ラインの位置
    [SerializeField] private float noteSpeed = 5f; // ノーツの落下速度(NoteMoverと同じ値にする)

    [Header("楽曲・BPM設定")]
    [SerializeField] private AudioSource music; // 楽曲のBGM
    [SerializeField] private float bpm = 120 /*楽曲のBPMを設定してください*/; // 楽曲のBPM
    [SerializeField] private int beatsPerBar = 4; // 1小節あたりのビート数
    [SerializeField] private int bars = 4; // 何小節分ノーツを生成するか
    [SerializeField] private int subdivision = 1; // 1ビートあたりの分割数(例: 2なら8分音符、4なら16分音符)
    [SerializeField] private float songOffset = 0f; // 楽曲のオフセット時間(秒)

    // ノーツ生成パターン (1: ノーツ生成, 0: ノーツ生成しない)
    [SerializeField]
    private int[] pattern =
    {
        1,0,  0,1,  1,0,  0,1,   // 1小節目
        1,0,  1,0,  0,1,  0,1,   // 2小節目
        1,0,  0,1,  1,0,  1,0,   // 3小節目
        1,1,  1,1,  1,1,  1,1    // 4小節目
    };

    private readonly List<float> hitTimes = new List<float>(); // ノーツのヒットタイムリスト
    private float fallTime; // 落下時間(秒)
    private int index = 0; // 現在のノーツ生成インデックス

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
        float secPerBeat = 60f / bpm;

        // 何ステップ（四分 / 八分 / 十六分）分のノーツを出すか
        int totalSteps = bars * beatsPerBar * subdivision;
        Debug.Log(totalSteps);
        hitTimes.Clear();

        for (int step = 0; step < totalSteps; step++)
        {
            
            if (pattern[step] == 0) continue;
            float beatIndex = step / (float)subdivision; // 現在のビートインデックス

            // 曲開始からのPerfect 中心タイミング（秒）
            float hitTime = songOffset + beatIndex * secPerBeat;
            hitTimes.Add(hitTime);
        }

        // 曲の再生
        if (music)
        {
            music.time = 0f;
            music.Play();
        }
        else
        {
            Debug.LogWarning("AudioSourceが設定されていません。");
        }
    }

    void Update()
    {
        if (index >= hitTimes.Count) return;
        if (!notePrefab) return;

        // 現在の「曲時間」
        float currentTime = music ? music.time : Time.time;

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

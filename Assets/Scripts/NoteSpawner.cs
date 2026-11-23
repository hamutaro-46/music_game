using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    [SerializeField] private GameObject notePrefab; // ノーツのプレハブ
    [SerializeField] private float spawnInterval = 0.6f; // ノーツの出現間隔
    [SerializeField] private int spawnCount = 32; // 出現させるノーツの数

    private float timer = 0;
    private int spawned = 0;

    void Update()
    {
        if (notePrefab == null) return;
        if (spawned >= spawnCount) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer -= spawnInterval;
            Spawn();
        }
    }

    private void Spawn()
    {
        Instantiate(notePrefab, transform.position, Quaternion.identity);
        spawned++;
    }
}

using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JudgeLine : MonoBehaviour
{
    [SerializeField] private KeyCode inputKey = KeyCode.Space; // 判定ラインに対応するキー
    [SerializeField] private float perfectRange = 0.1f; // パーフェクト判定の範囲
    [SerializeField] private float goodRange  = 0.3f; // グッド判定の範囲
    [SerializeField] private TextMeshProUGUI judgeText; // 判定結果を表示するテキスト

    // 判定ライン付近にいるノーツのリスト
    private readonly List<Transform> notesInRange = new List<Transform>();

    void Start()
    {
        judgeText.text = "";
    }

    void Update()
    {
        if (Input.GetKeyDown(inputKey))
        {
            JudgeNearestNotes();
        }
    }

    private void JudgeNearestNotes()
    {
        if (notesInRange.Count == 0) return;

        // 最も近いノーツを見つける
        Transform bestNote = null;
        float bestDist = float.MaxValue;

        foreach (Transform note in notesInRange)
        {
            if (note == null) continue;

            float dist = Mathf.Abs(note.position.y - transform.position.y);
            if (dist < bestDist)
            {
                bestDist = dist;
                bestNote = note;
            }
        }

        // 距離に応じてPerfect、Good、Missを判定する
        if (bestDist <= perfectRange)
        {
            ShowJudge("Perfect!", Color.yellow);
        }else if (bestDist <= goodRange)
        {
            ShowJudge("Good", Color.green);
        }
        else
        {
            ShowJudge("Miss", Color.grey);
        }

        // 判定したノーツをリストから削除し、シーンからも削除する
        notesInRange.Remove(bestNote);
        Destroy(bestNote.gameObject);
    }

    private void ShowJudge(string text, Color color)
    {
        if (judgeText == null) return;

        judgeText.text = text;
        judgeText.color = color;
    }

    // 判定ラインのTriggerにノーツが入ったとき
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Note")) return;

        if (!notesInRange.Contains(collision.transform))
        {
            notesInRange.Add(collision.transform);
        }
    }

    // 判定ラインのTriggerからノーツが出たとき
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Note")) return;

        if (notesInRange.Contains(collision.transform))
        {
            ShowJudge("Miss", Color.grey);
            notesInRange.Remove(collision.transform);
        }
    }
}

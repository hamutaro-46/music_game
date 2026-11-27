using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText; // スコアを表示するテキスト
    private int currentScore = 0; // 現在のスコア
    
    public void AddScore(int value)
    {
        currentScore += value;
        UpdateScore();
    }

    private void UpdateScore()
    {
        if (scoreText)
        {
            scoreText.text = "Score: " + currentScore.ToString();
        }
    }
}
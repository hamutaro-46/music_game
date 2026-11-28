using UnityEngine;

[CreateAssetMenu(fileName = "Chart", menuName = "Scriptable Object/MusicChart")]
public class MusicChart : ScriptableObject
{
    public float bpm; // 曲のBPM
    public float offset; // 曲のオフセット
    public int beatsPerBar; // 小節あたりの拍数
    public int beats; // 総拍数
    public int subdivision; // 1拍あたりの分割数

    public string[] pattern; // ノーツパターンの配列(「1, 0, 0」のようにカンマで区切る)

    private void OnValidate()
    {
        // 配列の値が空欄だった場合に"0,0,0"で埋める
        for (int i = 0; i < pattern.Length; i++)
        {
            if (string.IsNullOrEmpty(pattern[i]))
            {
                pattern[i] = "0,0,0";
            }
        }
    }
}
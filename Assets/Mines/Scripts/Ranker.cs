using UnityEngine;
using UnityEngine.UI;

// ランキングに掲載するランカーの設定をするクラス
public class Ranker : MonoBehaviour
{
    [SerializeField] private Text rank_text, name_text, time_text;

    public void SetRanker(int rank, string name, float time)
    {
        rank_text.text = rank.ToString();
        name_text.text = name;
        time_text.text = string.Format("{0:F2} sec", time);
    }
}

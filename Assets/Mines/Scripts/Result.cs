using UnityEngine;
using TMPro;

// 結果の処理をクラス
public class Result : MonoBehaviour
{
    // クリアタイム
    public static float currentClearedTime;
    // クリア時にアクティブにするオブジェクト、失敗時にアクティブにするオブジェクト
    [SerializeField] private GameObject clearEffect, failedEffect;
    // クリアタイムのテキスト
    [SerializeField] private TextMeshProUGUI clearTime_text;

    // クリア時に呼び出される
    public void ClearedEvent()
    {
        clearEffect.SetActive(true);
        clearTime_text.text = string.Format("Time : {0:F2} sec", currentClearedTime);
    }

    // 失敗時に呼び出される
    public void FailedEvent()
    {
        failedEffect.SetActive(true);
    }
}

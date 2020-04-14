using System.Collections;
using UnityEngine;
using TMPro;
using DG.Tweening;

// タイトルロゴのエフェクト（アニメーション）
public class TitleEffect : Utility
{
    // アニメーションに要する時間
    [SerializeField] private float dulation = 3f;
    // 効果音
    [SerializeField] private AudioClip opening;

    // タイトルのアニメーションを開始　表示に要する時間を返す
    public float StartTitleAnim()
    {
        StartCoroutine(TitleAnim());
        return dulation + 1.5f;
    }

    private IEnumerator TitleAnim()
    {
        // TextMeshProのマテリアルを取得
        Material mat = GetComponent<TextMeshProUGUI>().fontMaterial;
        // タイトルを表示
        mat.DOFloat(0, "_FaceDilate", dulation);
        // 効果音を鳴らす
        soundEffecter.Play(opening, SoundEffectPitch.x1);
        yield return new WaitForSeconds(dulation + 1.5f);
        // 明暗を繰り返す
        while (true)
        {
            mat.SetFloat("_GlowOuter", 0.25f * Mathf.Sin(2f * Mathf.PI * 0.25f * Time.time) + 0.75f);
            yield return null;
        }
    }
}

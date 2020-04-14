using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Helper : MonoBehaviour
{
    // テロップ、目的、アクションのトランスフォーム
    [SerializeField] private RectTransform how, howS, telop, purpose, action;
    // テロップ、目的、アクションのテキスト
    [SerializeField] private Text telopText, purposeText, actionText;
    // テロップのアニメーションスピード、表示時間
    [SerializeField] private float telopFloatSpeed = 0.5f, telopDisplaDulation = 5.0f;
    // 目的のアニメーションスピード
    [SerializeField] private float purposeFloatSpeed = 0.5f;

    // 操作方法を表示
    public void HowDisplay()
    {
        how.gameObject.SetActive(true);
    }

    // 操作方法を非表示
    public void HowHidden()
    {
        how.gameObject.SetActive(false);
    }

    // 操作方法を表示するかの設定 trueだと、上の関数が呼ばれても表示されない
    public void HowDisplaySetting(bool enable)
    {
        howS.gameObject.SetActive(enable);
    }

    // テロップのアニメーション
    private Coroutine telopCor;
    public void Telop(string message)
    {
        // テロップの文字列を変更する
        telopText.text = message;
        // すでに表示されている時は、中止
        if (telopCor != null)
        {
            StopCoroutine(telopCor);
        }
        // コルーチンを実行
        telopCor = StartCoroutine(TelopCor());
    }

    // テロップのコルーチン
    private IEnumerator TelopCor()
    {
        // アクティブにして、画面の下はしに移動し、透明にする
        telop.gameObject.SetActive(true);
        telop.DOMoveY(0, 0);
        telopText.DOFade(0f, 0f);
        // フロートアップ
        telop.DOMoveY(Screen.height / 10.0f, telopFloatSpeed);
        telopText.DOFade(1f, telopFloatSpeed);
        // 指定時間表示させる
        yield return new WaitForSeconds(telopDisplaDulation);
        // フロートダウン
        telop.DOMoveY(0f, telopFloatSpeed);
        telopText.DOFade(0f, telopFloatSpeed);
        yield return new WaitForSeconds(telopFloatSpeed);
        // 非アクティブにする
        telop.gameObject.SetActive(false);
    }

    // 目的を変更する
    public void Purpose(string message)
    {
        StartCoroutine(PurposeCor(message));
    }

    private IEnumerator PurposeCor(string message)
    {
        // 一旦画面外に移動
        purpose.DOMoveX(-450f * transform.localScale.x, purposeFloatSpeed);
        yield return new WaitForSeconds(purposeFloatSpeed);
        // 見えていない隙に目的を変更
        purposeText.text = "目的 : " + message;
        yield return new WaitForSeconds(0.1f);
        // 画面内に表示
        purpose.DOMoveX(0f, purposeFloatSpeed);
    }

    // アクション内容を表示
    public void ActionDisplay(string message)
    {
        action.gameObject.SetActive(true);
        actionText.text = message;
    }

    // アクションを非表示に
    public void ActionHidden()
    {
        action.gameObject.SetActive(false);
    }
}

using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using System.Collections;

// フェードの色の種類
public enum FadeColor
{
    Black, White
}

public class Fader : MonoBehaviour
{
    // この関数を呼び出すとフェードインする
    public void FadeIn(FadeColor color, float dulation, Action method)
    {
        StartCoroutine(CorFadeIn(color, dulation, method));
    }
    // フェードイン
    private IEnumerator CorFadeIn(FadeColor color, float dulation, Action method)
    {
        yield return null;
        // フェードイン中は、何も入力できない
        MyInput.invalidAnyKey = true;
        Image img = GetComponent<Image>();
        Color c = FadeColorToColor(color);
        // 不透明に
        c.a = 1f;
        // 色を設定
        img.color = c;
        // 有効化
        img.enabled = true;
        // 透明度を0にしていく
        img.DOFade(0f, dulation);
        yield return new WaitForSecondsRealtime(dulation + 0.5f);
        MyInput.invalidAnyKey = false;
        // フェードインが終わったら関数を呼び出す
        method();
        // faderを非表示
        img.enabled = false;
        yield return null;
    }

    // この関数を呼び出すとフェードアウトする
    public void FadeOut(FadeColor color, float dulation, Action method)
    {
        StartCoroutine(CorFadeOut(color, dulation, method));
    }

    // フェードアウト
    private IEnumerator CorFadeOut(FadeColor color, float dulation, Action method)
    {
        yield return null;
        // フェードアウト中は何も入力できない
        MyInput.invalidAnyKey = true;
        Image img = GetComponent<Image>();
        Color c = FadeColorToColor(color);
        // 透明に
        c.a = 0f;
        // 色を設定
        img.color = c;
        // 有効化
        img.enabled = true;
        // 透明度を1にしていく
        img.DOFade(1f, dulation);
        yield return new WaitForSecondsRealtime(dulation + 0.5f);
        // フェードアウトが終わったら関数を呼び出す
        method();
        yield return null;
    }

    // enumのFadeColorをColorに変換
    private Color FadeColorToColor(FadeColor fadeColor)
    {
        Color c = new Color();
        switch (fadeColor)
        {
            case FadeColor.Black:
                c = Color.black;
                break;
            case FadeColor.White:
                c = Color.white;
                break;
            default:
                break;
        }
        return c;
    }
}

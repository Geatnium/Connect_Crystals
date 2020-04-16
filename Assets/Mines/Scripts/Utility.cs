using System.Collections;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public enum Scene
{
    First, Title, Game
}

// 管理クラスが良く使いそうな関数やプロパティをまとめたクラス
public class Utility : MonoBehaviour
{
    public Fader fader
    {
        get { return GameObject.FindWithTag("Fader").GetComponent<Fader>(); }
    }

    public SoundEffecter soundEffecter
    {
        get { return GameObject.FindWithTag("SoundEffect").GetComponent<SoundEffecter>(); }
    }

    public AudioSource bgm
    {
        get { return GameObject.FindWithTag("BGM").GetComponent<AudioSource>(); }
    }

    public Helper helper
    {
        get { return GameObject.FindWithTag("Helper").GetComponent<Helper>(); }
    }

    // プレイヤーはキャッシュしておく
    public Player player 
    { 
        get { return GameObject.FindWithTag("Player").GetComponent<Player>(); }
    }

    // シーン遷移
    public void SceneChangeFadeOut(FadeColor fadeColor, Scene to, float dulation)
    {
        // フェードアウト フェードアウト完了後、ロードが完了次第遷移
        fader.FadeOut(fadeColor, dulation, () =>
        {
            StartCoroutine(SceneAsyncLoad(to));
        });
    }

    // シーンの読み込みまで待ち、完了したら遷移
    private IEnumerator SceneAsyncLoad(Scene to)
    {
        // 次のシーンのロード開始
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync((int)to);
        // すぐには遷移しない
        asyncLoad.allowSceneActivation = false;
        while (true)
        {
            yield return null;
            // 読み込み完了したら
            if (asyncLoad.progress >= 0.9f)
            {
                // シーン読み込み
                asyncLoad.allowSceneActivation = true;
                break;
            }
        }

    }

    // 遅れて実行させたい時
    public void Delay(float delay, Action action)
    {
        StartCoroutine(CorDelay(delay, action));
    }

    private IEnumerator CorDelay(float delay, Action action)
    {
        yield return new WaitForSecondsRealtime(delay);
        action();
    }

    // カーソルを表示
    public void CursorVisible()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // カーソルを非表示
    public void CursorIsVisible()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}

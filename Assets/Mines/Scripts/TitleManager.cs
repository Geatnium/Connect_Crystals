﻿using UnityEngine;

// タイトル画面の状態
public enum TitleState
{
    // 特になし、クリア後、失敗後
    None, Cleared, Failed
}

// タイトル画面での処理などを行うクラス
public class TitleManager : Utility
{
    // タイトル画面の状態
    public static TitleState state = TitleState.None;

    // タイトルエフェクト
    [SerializeField] private TitleEffect titleEffect;

    private void Start()
    {
        // 念のためタイムスケールを1に
        Time.timeScale = 1f;
        // カーソルを非表示に
        CursorIsVisible();
        // クリア後の場合は白のフェードインで、それ以外だったら黒のフェードイン
        if (state == TitleState.Cleared)
        {
            // フェードイン
            fader.FadeIn(FadeColor.White, 1.0f, () =>
            {
                FadeInAfterStart();
            });
        }
        else
        {
            // フェードイン
            fader.FadeIn(FadeColor.Black, 1.0f, () =>
            {
                FadeInAfterStart();
            });
        }

        // ゲームに成功して戻ってきた時
        if (state == TitleState.Cleared)
        {
            GetComponent<Result>().ClearedEvent();
        }
        // ゲームに失敗して戻ってきた時
        else if (state == TitleState.Failed)
        {
            GetComponent<Result>().FailedEvent();
        }

        GameObject.FindWithTag("Ranking").GetComponent<LeaderBoard>().UpdateBestTime();
        // 最後に状態を戻す
        state = TitleState.None;
    }

    // フェードイン終了後にやる処理
    private void FadeInAfterStart()
    {
        // 入力を受け付けない
        MyInput.invalidAnyKey = true;
        // フェードイン終了後、タイトルのアニメーションを再生し、終了したら動けるようになる
        Delay(titleEffect.StartTitleAnim(), () =>
        {
            MyInput.invalidAnyKey = false;
            player.MoveActive();
            helper.HowDisplay();
        });
    }

    // ステージの難易度を設定してゲームシーンに移行
    public void StartEasyStage()
    {
        Difficulty.difficult = Difficult.Easy;
        GoGameScene();
    }

    public void StartHardStage()
    {
        Difficulty.difficult = Difficult.Hard;
        GoGameScene();
    }

    // ゲームシーンに移行する関数
    private void GoGameScene()
    {
        SceneChangeFadeOut(FadeColor.White, Scene.Game, 1.0f);
        player.MoveStop();
    }
}

using UnityEngine;

public class Pause : Utility
{
    // 一時停止中か
    private bool isPause = false;

    private void Update()
    {
        // 一時停止ボタンを押した時
        if (MyInput.GetButtonDown("Pause"))
        {
            // 一時停止を切り替え
            isPause = !isPause;
            // 一時停止中
            if (isPause)
            {
                PauseOpen();
            }
            // 一時停止解除
            else
            {
                PauseClose();
            }   
        }
    }

    // 一時停止画面にする
    public void PauseOpen()
    {
        isPause = true;
        // 一時停止画面を表示
        transform.GetChild(0).gameObject.SetActive(true);
        // カーソルを表示
        CursorVisible();
        // プレイヤーの動きをストップ
        player.MoveStop();
        // 停止中は時間を止める
        Time.timeScale = 0f;
    }

    // 一時停止を解除する
    public void PauseClose()
    {
        isPause = false;
        // 一時停止画面を非表示
        transform.GetChild(0).gameObject.SetActive(false);
        // カーソルを非表示
        CursorIsVisible();
        // プレイヤーを動けるようにする
        player.MoveActive();
        // 時間を進める
        Time.timeScale = 1f;
    }

    // ゲームを終了する
    public void GameExit()
    {
        Application.Quit();
    }

    // タイトルへ移動
    public void GoTitle()
    {
        SceneChangeFadeOut(FadeColor.Black, Scene.Title, 0.5f);
    }
}

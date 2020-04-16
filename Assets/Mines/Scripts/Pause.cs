using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

// 一時停止の管理をしているクラス
public class Pause : Utility
{
    // 一時停止中か
    private bool isPause = false;

    [SerializeField] private float pauseFadeTime = 0.5f;
    [SerializeField] private float pauseFadeSize = 1.1f;

    private RectTransform panel;
    private Image[] images;
    private Text[] texts;
    private float[] imagesAlpha;
    private float[] textsAlpha;

    private bool isAnim = false;

    private void Start()
    {
        // 入れ子にあるパネルやImage、Textをすべて取得
        panel = transform.GetChild(0).GetComponent<RectTransform>();
        panel.gameObject.SetActive(true);
        images = transform.GetComponentsInChildren<Image>();
        texts = transform.GetComponentsInChildren<Text>();
        panel.gameObject.SetActive(false);
        // 少し大きくしておく
        panel.DOScale(Vector3.one * pauseFadeSize, 0f);
        // 元の透明度を保持しておき、透明にする
        imagesAlpha = new float[images.Length];
        textsAlpha = new float[texts.Length];
        for (int i = 0; i < images.Length; i++)
        {
            imagesAlpha[i] = images[i].color.a;
            images[i].DOFade(0f, 0f);
        }
        for (int i = 0; i < texts.Length; i++)
        {
            textsAlpha[i] = texts[i].color.a;
            texts[i].DOFade(0f, 0f);
        }
    }

    private void Update()
    {
        // 一時停止画面がアニメーション中は反応させない
        if (isAnim) return;
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
        isAnim = true;
        Delay(pauseFadeTime, () => { isAnim = false; });
        // カーソルを表示
        CursorVisible();
        // プレイヤーの動きをストップ
        player.MoveStop();
        // 停止中は時間を止める
        Time.timeScale = 0f;
        // 一時停止画面をふわっと表示する
        panel.gameObject.SetActive(true);
        panel.DOScale(Vector3.one, pauseFadeTime);
        // 透明度をもとに戻す
        for (int i = 0; i < images.Length; i++)
        {
            images[i].DOFade(imagesAlpha[i], pauseFadeTime);
        }
        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].DOFade(textsAlpha[i], pauseFadeTime);
        }
    }

    // 一時停止を解除する
    public void PauseClose()
    {
        // カーソルを非表示
        CursorIsVisible();
        isAnim = true;
        // ふわっと非表示
        panel.DOScale(Vector3.one * pauseFadeSize, pauseFadeTime);
        // 不透明にする
        for (int i = 0; i < images.Length; i++)
        {
            images[i].DOFade(0f, pauseFadeTime);
        }
        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].DOFade(0f, pauseFadeTime);
        }
        // アニメーションが終わった後にする処理
        Delay(pauseFadeTime, () => 
        {
            isPause = false;
            isAnim = false;
            // 一時停止画面を非アクティブ
            panel.gameObject.SetActive(false);
            // プレイヤーを動けるようにする
            player.MoveActive();
            // 時間を進める
            Time.timeScale = 1f;
        });
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

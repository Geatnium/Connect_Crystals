using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

// ゲームシーンでの進行などを管理
public class GameManager : Utility
{
    // ゲームの進行度
    private enum Progress
    {
        Tutorial, MainGame, Ending, Clear, Failure
    }
    private Progress progress = Progress.Tutorial;

    // スポナー
    private UmaSpawner spawner;
    // ゲームのUI, マップ, 移動制限、ワープ1, 2, 3, 最後の馬のトリガー、ゲームオーバー時のエフェクト
    [SerializeField] private GameObject gameUI, map, guards, warp1, warp2, warp3, endUmaTriggers, gameOverEffect;
    // ゲームオーバー時の効果音
    [SerializeField] private AudioClip gameOverSound;
    // 接続したクリスタルの数
    private int connectedNum = 0;
    // タイムカウントをしている親オブジェクト
    [SerializeField] private RectTransform timeCountTransform;
    // タイムカウントをしているテキスト
    [SerializeField] private Text timeCountText;
    // タイムをカウントする変数
    private float timeCount = 0f;

    private void Start()
    {
        // スポナーを取得
        spawner = GetComponent<UmaSpawner>();
        // カーソルを非表示
        CursorIsVisible();
        // フェードインする
        fader.FadeIn(FadeColor.White, 1.0f, () =>
        {
            // フェードインが終わったら動けるようになる
            player.MoveActive();
            // 操作方法を表示
            helper.HowDisplay();
            // テロップと目的を表示
            helper.Telop("道なりに進んでください。\n先にある魔法陣で転移します。");
            helper.Purpose("道なりに進み、魔法陣に向かう");
        });
    }

    // クリスタルを接続したときの処理
    public void ConnectCrystal(CrystalColor crystalColor)
    {
        connectedNum++;
        // 馬を生成する
        spawner.Spawn();
        // 青クリスタルを接続したらゲーム開始
        if(crystalColor == CrystalColor.Blue)
        {
            // ゲーム開始
            GameStart();
        }
        else
        {
            // クリスタルをカウントする
            CountCrystal();
        }
    }

    // ゲーム開始
    private void GameStart()
    {
        // ゲームの進行を進める
        progress = Progress.MainGame;
        // BGMを再生
        bgm.Play();
        // マップを表示
        map.SetActive(true);
        // ステージを移動できるようにする
        guards.SetActive(false);
        // テロップと目的を表示
        helper.Telop("ゲームスタート！\n敵から逃げながら、全てのクリスタルを接続してください。");
        helper.Purpose("全てのクリスタルを接続する (1/7)");
        // 経過時間を表示
        timeCountTransform.DOMoveX(0f, 0.5f);
    }

    // 接続したクリスタルをカウント
    private void CountCrystal()
    {
        // まだ全ては接続していない場合
        if(connectedNum < 7)
        {
            // 目的にも表示
            helper.Purpose("全てのクリスタルを接続する (" + connectedNum + "/7)");
        }
        else // 全てのクリスタルを接続した場合
        {
            // テロップと目的
            helper.Telop ("全てのクリスタルが繋がった！\nスタート地点に脱出用の魔法陣が出現しました。");
            helper.Purpose("スタート地点に戻る");
            // ワープ2を有効化
            warp2.SetActive(true);
        }
    }

    // 一つ目のワープ
    public void Warp1()
    {
        // 動けなくする
        player.MoveStop();
        // フェードアウト
        fader.FadeOut(FadeColor.White, 1.0f, Warp1Func);
    }

    private void Warp1Func()
    {
        // 見えないときに、プレイヤーをワープ なぜかたまにワープしない時があるので
        for (int i = 0; i < 10; i++)
        {
            player.transform.position = new Vector3(0f, 0f, 0f);
            player.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        // 一つ目のワープを消す
        warp1.SetActive(false);
        // フェードイン
        fader.FadeIn(FadeColor.White, 1.0f, () =>
        {
            // フェードインが終わったら動けるよになる
            player.MoveActive();
            // テロップと目的を表示
            helper.Telop("青いクリスタルに近づいてEキーで接続してください。\nそうしたらゲームスタートです！");
            helper.Purpose("青いクリスタルを接続");
        });
    }

    // 二つ目のワープ
    public void Warp2()
    {
        // 動けなくする
        player.MoveStop();
        // 進行をエンディングに
        progress = Progress.Ending;
        // フェードアウト
        fader.FadeOut(FadeColor.White, 1.0f, Warp2Func);
    }

    private void Warp2Func()
    {
        // 見えないときに、プレイヤーをワープ なぜかたまにワープしない時があるので
        for (int i = 0; i < 10; i++)
        {
            player.transform.position = new Vector3(-1.5f, 0f, -20.5f);
            player.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        }
        // 二つ目のワープを消す
        warp2.SetActive(false);
        // タイムとマップはもう表示しない
        timeCountTransform.gameObject.SetActive(false);
        map.SetActive(false);
        // フェードイン
        fader.FadeIn(FadeColor.White, 1.0f, () =>
        {
            // フェードインが終わったら動けるよになる
            player.MoveActive();
            // 三つ目のワープを有効化
            warp3.SetActive(true);
            // 馬が動き出すトリガーを有効化
            endUmaTriggers.SetActive(true);
            // テロップと目的を表示
            helper.Telop("この道を抜けた先にゴールの魔法陣があります。\n止まるんじゃねぇぞ...！");
            helper.Purpose("ゴールまで走り抜ける");
        });
    }

    // 三つ目のワープ　ゲームクリア！
    public void Warp3()
    {
        // 動けなくする
        player.MoveStop();
        // 振り向く
        player.transform.DORotate(new Vector3(0, 180f, 0), 0.4f);
        player.transform.GetChild(0).DOLocalRotate(Vector3.zero, 0.4f);
        // 進行をクリアに
        progress = Progress.Clear;
        // タイトル画面の状態をクリア状態に
        TitleManager.state = TitleState.Cleared;
        // タイトルのリザルトにクリアタイムを送る
        Result.currentClearedTime = timeCount;
        // タイトルに遷移
        SceneChangeFadeOut(FadeColor.White, Scene.Title, 2.0f);
    }


    // ゲームオーバー時の処理
    public void GameOver()
    {
        // 入力を受け付けなくする
        MyInput.invalidAnyKey = true;
        // 全ての馬を止める
        GameObject[] umas = GameObject.FindGameObjectsWithTag("Uma");
        // 全てのUIを非表示にする
        helper.gameObject.SetActive(false);
        gameUI.gameObject.SetActive(false);
        for (int i = 0; i < umas.Length; i++)
        {
            umas[i].GetComponent<UmaPatrol>().StopMove();
        }
        // 少し遅らせてエフェクトを実行
        Delay(0.5f, () =>
        {
            // ゲームオーバーのエフェクトを表示
            gameOverEffect.SetActive(true);
            // 効果音を鳴らす
            soundEffecter.Play(gameOverSound, SoundEffectPitch.x1);
        });

        TitleManager.state = TitleState.Failed;
        // 2.5秒後にフェードアウトしてシーン遷移
        Delay(3.0f, () =>
        {
            SceneChangeFadeOut(FadeColor.Black, Scene.Title, 1.5f);
        });
    }

    private void Update()
    {
        // メインのゲーム中だけタイムをカウントする
        if(progress == Progress.MainGame)
        {
            timeCount += Time.deltaTime;
            timeCountText.text = string.Format("Time : {0:F2} sec", timeCount);
        }
    }
}

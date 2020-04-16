using UnityEngine;
using UnityEngine.UI;
using NCMB;
using DG.Tweening;
using System.Collections.Generic;

// ランキングを取得・登録し、表示するクラス
public class LeaderBoard : Utility
{
    // 登録したスコアのベストを保存するキー、ローカルのベストスコアを保存するキー
    private readonly string KEY_UPLOADTIME = "uploadtime", KEY_BESTTIME = "besttime";
    // ベストタイム
    private float bestTime = -1f;

    // ベストタイムを表示するテキスト
    [SerializeField] private Text yourBestTime;
    // ランカーのプレハブ
    [SerializeField] private GameObject ranker_prefab;
    // ランカーを表示するボード
    [SerializeField] private Transform rankerBoard;

    // アップロードに使用する入力フォーム
    [SerializeField] private RectTransform rankingUploader_UI;
    // 名前を入力するところ
    [SerializeField] private InputField nameField;

    // ランキングの画面が開かれているかどうか
    private bool isOpen = false;

    private void Start()
    {
        // NCMBのキーを設定
        NCMBAPIKey.Set();

        // ランキングを更新
        GetRanking();
    }

     // ベストタイムが更新されている場合は更新
    public void UpdateBestTime()
    {
        // ベストタイムを読み出す
        bestTime = PlayerPrefs.GetFloat(KEY_BESTTIME, -1f);
        // 今回やったのがハードモードでクリアしていたら、タイムを取得
        float currentTime = -1f;
        if (Difficulty.difficult == Difficult.Hard && TitleManager.state == TitleState.Cleared)
        {
            currentTime = Result.currentClearedTime;
        }
        // 今回の結果があり、且つベストタイムもすでにあり、且つ今回の結果の方が早い時、ベストタイムを更新。
        // 今回の結果があり、ベストタイムがまだ無い場合は、ベストタイムを更新
        if (currentTime > 0f)
        {
            if (bestTime > 0f)
            {
                if (currentTime < bestTime)
                {
                    bestTime = currentTime;
                }
            }
            else
            {
                bestTime = currentTime;
            }
            PlayerPrefs.SetFloat(KEY_BESTTIME, bestTime);
        }
        // 今回の結果もなく、ベストタイムもまだ無い場合は、-----を表示
        else
        {
            if (bestTime < 0)
            {
                yourBestTime.text = "あなたの記録 : ----- sec";
                return;
            }
        }
        // ベストタイムがあれば、それを表示
        yourBestTime.text = string.Format("あなたの記録 : {0:F2} sec", bestTime);
    }

    // ランキングを取得
    private void GetRanking()
    {
        // すでにある場合、一旦全部削除
        for (int r = 0; r < rankerBoard.transform.childCount; r++)
        {
            Destroy(rankerBoard.transform.GetChild(r).gameObject);
        }
        // 検索
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("Ranking");
        query.OrderByAscending("Time");
        query.Limit = 10;
        query.FindAsync((List<NCMBObject> objList, NCMBException e) =>
        {
            if (e != null)
            {
                //検索失敗時
                helper.Telop("ランキング更新にエラーが発生しました。m(_ _)m");
            }
            else
            {
                //検索成功時
                // ランカーを表示していく
                int c = 1;
                foreach (NCMBObject obj in objList)
                {
                    float s = (float)System.Convert.ToDouble(obj["Time"]);
                    string n = System.Convert.ToString(obj["Name"]);
                    GameObject go_ranker = Instantiate(ranker_prefab, rankerBoard.transform);
                    go_ranker.GetComponent<Ranker>().SetRanker(c, n, s);
                    c++;
                }
            }
        });
    }

    // ランキングにアップロードを試みる
    public void UploadRunking()
    {
        // ベストタイムが無い場合、アップロードできない
        if (bestTime < 0)
        {
            helper.Telop("まだハードモードをクリアしていません。(';')");
        }
        // ベストタイムがある場合
        else
        {
            // すでにアップロードされているタイムを取得
            float uploadedTime = PlayerPrefs.GetFloat(KEY_UPLOADTIME, -1f);
            // 名前を入力していないとダメ
            if (string.IsNullOrEmpty(nameField.text))
            {
                helper.Telop("名前を入力してください。(';')");
                return;
            }
            // アップロードされたタイムがある場合、それを更新していないとダメ
            if (uploadedTime > 0f && uploadedTime <= bestTime)
            {
                helper.Telop("タイムを更新してください。(';')");
                return;
            }
            // アップロードされたタイムが無いか、されててもタイムを更新していればアップロード
            UploadScore();
        }
        // 閉じる
        CloseRankingMenu();
    }

    // タイムをアップロードする
    private void UploadScore()
    {
        // 入力された名前を取得
        string upName = nameField.text;
        // 検索
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("Ranking");
        query.WhereEqualTo("Name", upName);
        query.FindAsync((List<NCMBObject> objList, NCMBException e) =>
        {
            //検索成功したら
            if (e == null)
            {
                //未登録の場合、新規登録
                if (objList.Count == 0)
                {
                    NCMBObject obj = new NCMBObject("Ranking");
                    obj["Name"] = upName;
                    obj["Time"] = bestTime;
                    obj.SaveAsync((NCMBException ee) =>
                    {
                        if (ee == null)
                        {
                            helper.Telop("ランキングにタイムを登録しました。＼(^o^)／");
                            PlayerPrefs.SetFloat(KEY_UPLOADTIME, bestTime);
                            GetRanking();
                        }
                        else
                        {
                            helper.Telop("ランキング更新にエラーが発生しました。m(_ _)m");
                        }
                    });
                }
                // すでに登録されてる場合
                else
                {
                    // サーバーのタイムを参照
                    float cloudScore = (float)System.Convert.ToDouble(objList[0]["Time"]);
                    // サーバーのタイムより早いとアップロードできる
                    if (bestTime < cloudScore)
                    {
                        objList[0]["Time"] = bestTime;
                        objList[0].SaveAsync((NCMBException ee) =>
                        {
                            if (ee == null)
                            {
                                helper.Telop("ランキングのタイムを更新しました。＼(^o^)／");
                                PlayerPrefs.SetFloat(KEY_UPLOADTIME, bestTime);
                                GetRanking();
                            }
                            else
                            {
                                helper.Telop("ランキング更新にエラーが発生しました。m(_ _)m");
                            }
                        });
                    }
                    // サーバーの方がタイムが早い
                    else
                    {
                        GetRanking();
                        helper.Telop("サーバーにあるタイムの方が良いです。d(^_^o)");
                    }
                }
            }
            else
            {
                helper.Telop("ランキング更新にエラーが発生しました。m(_ _)m");
            }
        });
    }

    // ランキング登録のフォームを表示する
    public void OpenRankingMenu()
    {
        // 開かれていない時だけ実行できる
        if (!isOpen)
        {
            isOpen = true;
            // にゅっとアニメーションして開く
            rankingUploader_UI.DOScale(Vector3.one, 0.2f);
            // プレイヤーは動けなくし、カーソルを表示
            player.MoveStop();
            CursorVisible();
        }
    }

    // フォームを閉じる
    public void CloseRankingMenu()
    {
        // にゅっと閉じる
        rankingUploader_UI.DOScale(Vector3.zero, 0.2f);
        // カーソルを非表示に
        CursorIsVisible();
        // 閉じ終わった後に
        Delay(0.2f, () =>
        {
            // プレイヤーを動けるように
            player.MoveActive();
            // またランキングを開けるように
            isOpen = false;
        });
    }

    // ランキングを更新
    public void ReloadRanking()
    {
        // ランキングを閉じる
        CloseRankingMenu();
        // ランキングを更新する
        GetRanking();
    }
}

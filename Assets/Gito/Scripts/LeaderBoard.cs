using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NCMB;
using DG.Tweening;

public class LeaderBoard : MonoBehaviour {

    private const string CODE_BESTTIME = "weioriojjieko";

    [SerializeField] private Text your_text;
    [SerializeField] private GameObject Ranker, RankerParent;

    private float thisTime, bestTime;

    private GameHelper helper;

    [SerializeField] private Transform rankingUI;
    [SerializeField] private InputField name_field;

    public bool isRanking = false;

    private void Start () {
        helper = GetComponent<GameHelper> ();
        if (TitleTrigger.mode == 1) {
            thisTime = TitleManager.score_time;
        }
        bestTime = PlayerPrefs.GetFloat (CODE_BESTTIME, -1f);

        GetRanking ();

        if (thisTime > 0) {
            if (bestTime > 0) {
                if (thisTime < bestTime) {
                    bestTime = thisTime;
                }
            } else {
                bestTime = thisTime;
            }
            PlayerPrefs.SetFloat (CODE_BESTTIME, bestTime);
        } else {
            if (bestTime < 0) {
                your_text.text = "あなたの記録 : ----- sec";
                return;
            }
        }
        your_text.text = string.Format ("あなたの記録 : {0:F2} sec", bestTime);
    }

    private void Update () {
        if (Input.GetKey (KeyCode.RightShift) && Input.GetKey (KeyCode.D) && Input.GetKey (KeyCode.L) && Input.GetKey (KeyCode.Y)) {
            //Debug.Log ("aaa");
            PlayerPrefs.DeleteAll ();
            bestTime = -1f;
            your_text.text = "あなたの記録 : ----- sec";
        }

        if (isRanking) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void ReloadRanking () {
        CloseRankingMenu ();
        GetRanking ();
    }

    public void UploadRunking () {
        if(bestTime < 0) {
            CloseRankingMenu ();
            helper.TelopDisplay ("まだハードモードをクリアしていません。(';')");
        } else {
            if (string.IsNullOrEmpty (name_field.text)) {
                helper.TelopDisplay ("名前を入力してください。(';')");
            } else {
                UploadScore ();
                CloseRankingMenu ();
            }
        }
    }

    public void OpenRankingMenu () {
        isRanking = true;
        rankingUI.DOScale (Vector3.one, 0.2f);
        helper.moveAble = false;
    }

    public void CloseRankingMenu () {
        isRanking = false;
        rankingUI.DOScale (Vector3.zero, 0.2f);
        Invoke ("LateMoveTrue", 0.2f);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LateMoveTrue () {
        helper.moveAble = true;
    }


    private void GetRanking () {
        for (int r = 0 ; r < RankerParent.transform.childCount ; r++) {
            Destroy (RankerParent.transform.GetChild (r).gameObject);
        }
        // データストアの「HighScore」クラスから検索
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject> ("Ranking");
        query.OrderByAscending ("Time");
        query.Limit = 10;
        query.FindAsync ((List<NCMBObject> objList, NCMBException e) => {
            if (e != null) {
                //検索失敗時の処理
                helper.TelopDisplay ("ランキング更新にエラーが発生しました。m(_ _)m");
            } else {
                //検索成功時の処理
                //取得したレコードをHighScoreクラスとして保存
                int c = 0;
                foreach (NCMBObject obj in objList) {
                    float s = (float)System.Convert.ToDouble (obj["Time"]);
                    string n = System.Convert.ToString (obj["Name"]);
                    GameObject go_ranker = Instantiate (Ranker, RankerParent.transform);
                    go_ranker.GetComponent<RankerItem> ().SetRanker (c + 1, n, s);
                    c++;
                }
            }
        });
    }

    public void UploadScore () {
        string upName = name_field.text;
        // データストアの「HighScore」クラスから、Nameをキーにして検索
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject> ("Ranking");
        query.WhereEqualTo ("Name", upName);
        query.FindAsync ((List<NCMBObject> objList, NCMBException e) => {
            //検索成功したら
            if (e == null) {
                //未登録
                if (objList.Count == 0) {
                    NCMBObject obj = new NCMBObject ("Ranking");
                    obj["Name"] = upName;
                    obj["Time"] = bestTime;
                    obj.SaveAsync ((NCMBException ee) => {
                        if (ee == null) {
                            helper.TelopDisplay ("ランキングにタイムを登録しました。＼(^o^)／");
                            GetRanking ();
                        } else {
                            helper.TelopDisplay ("ランキング更新にエラーが発生しました。m(_ _)m");
                        }
                    });
                } else {
                    float cloudScore = (float)System.Convert.ToDouble (objList[0]["Time"]);
                    if (bestTime < cloudScore) {
                        objList[0]["Time"] = bestTime;
                        objList[0].SaveAsync ((NCMBException ee) => {
                            if (ee == null) {
                                helper.TelopDisplay ("ランキングのタイムを更新しました。＼(^o^)／");
                                GetRanking ();
                            } else {
                                helper.TelopDisplay ("ランキング更新にエラーが発生しました。m(_ _)m");
                            }
                        });
                    } else {
                        GetRanking ();
                        helper.TelopDisplay ("サーバーにあるタイムの方が良いです。d(^_^o)");
                    }
                }
            } else {
                helper.TelopDisplay ("ランキング更新にエラーが発生しました。m(_ _)m");
            }
        });
    }
}

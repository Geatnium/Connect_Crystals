using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleTrigger : MonoBehaviour {

    [SerializeField] private GameObject Ekey;
    [SerializeField] private Text ekey_text;

    private TitleManager manager;
    private LeaderBoard leaderBoard;
    private GameHelper helper;

    private bool warpAble = false, rankingAble = false;

    public static int mode = 0;

    private void Start () {
        manager = GameObject.FindWithTag ("Manager").GetComponent<TitleManager> ();
        leaderBoard = GameObject.FindWithTag ("Manager").GetComponent<LeaderBoard> ();
        helper = GameObject.FindWithTag ("Manager").GetComponent<GameHelper> ();
    }

private void Update () {
        if (!helper.moveAble) {
            return;
        }

        if (warpAble) {
            if (Input.GetButtonDown ("Action")) {
                manager.GoGameStage ();
                warpAble = false;
                Ekey.SetActive (false);
            }
        }

        if (rankingAble && !leaderBoard.isRanking) {
            Ekey.SetActive (true);
            ekey_text.text = "登録/更新";
            if (Input.GetButtonDown ("Action")) {
                leaderBoard.OpenRankingMenu ();
                Ekey.SetActive (false);
            }
        }
    }

    public void OnTriggerEnter (Collider other) {
        if (other.CompareTag ("WarpEasy")) {
            mode = 0;
            warpAble = true;
            Ekey.SetActive (true);
            ekey_text.text = "イージーモードをプレイ";
            helper.TelopDisplay ("イージーモードは敵の位置がマップに表示されます。\nクリアタイムはランキングに登録できません。");
        }
        if (other.CompareTag ("WarpHard")) {
            mode = 1;
            warpAble = true;
            Ekey.SetActive (true);
            ekey_text.text = "ハードモードをプレイ";
            helper.TelopDisplay ("ハードモードは敵の位置がマップに表示されません。\nクリアタイムをランキングに登録することができます。");
        }
        if (other.CompareTag ("Ranking")) {
            rankingAble = true;
        }
    }

    public void OnTriggerExit (Collider other) {
        if (other.CompareTag ("WarpEasy") || other.CompareTag("WarpHard")) {
            warpAble = false;
            Ekey.SetActive (false);
        }
        if (other.CompareTag ("Ranking")) {
            rankingAble = false;
            Ekey.SetActive (false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    [SerializeField] private GameObject Uma, Map, Guard, How, TimeCount, GameOverUma, Warp1, Warp2, Warp3, EndUmaTrigger;

    [SerializeField] private Transform player;

    [SerializeField] private Uma[] umas;

    [SerializeField] private AudioSource bgm;

    private GameHelper helper;

    public int crystals = 0;

    public bool isPlaying = false, isCatched = false, isWarping = false, isEnding1 = false, isClear = false;

    private float time = 0;

    private Text time_text;

    private void Start () {
        TitleManager.score_time = -1;

        helper = GetComponent<GameHelper> ();

        time_text = TimeCount.GetComponentInChildren<Text> ();

        Map.SetActive (false);
        Invoke ("StartTelop", 2.5f);

        helper.WhiteFadeIn ();
    }

    private void StartTelop () {
        helper.TelopDisplay ("道なりに進んでください。\n先にある魔法陣で転移します。");
        helper.PurposeDisplay ("道なりに進み、魔法陣に向かう");
    }

    private void Update () {
        if (isPlaying) {
            time += Time.deltaTime;
            time_text.text = string.Format ("Time : {0:F2} sec", time);
        }
    }

    [SerializeField] private Vector3 warpPos1, warpRot1, warpPos2, warpRot2;

    public void Worp_1 () {
        StartCoroutine (WarpCor_1 ());
    }

    private IEnumerator WarpCor_1 () {
        helper.WhiteFadeOut ();
        yield return new WaitForSeconds (2f);
        Destroy (Warp1);
        warpPos1.y = 0.16f;
        player.position = warpPos1;
        player.rotation = Quaternion.Euler (warpRot1);
        helper.WhiteFadeIn ();
        yield return new WaitForSeconds (2f);
        helper.TelopDisplay ("青いクリスタルに近づいてEキーで接続してください。\nそうしたらゲームスタートです！");
        helper.PurposeDisplay ("青いクリスタルを接続");
    }

    public void UmaSpawn () {
        GameObject go_uma = Instantiate (Uma);
        go_uma.name = "SpawnedUma";
        crystals++;
        if (crystals == 1) {
            bgm.Play ();
            helper.PurposeDisplay ("全てのクリスタルを接続する(1/7)");
            helper.TelopDisplay ("ゲームスタート！\n敵から逃げながら、全てのクリスタルを接続してください。");
            TimeCount.GetComponent<RectTransform> ().DOLocalMoveX (-515f, 0.5f);
            isPlaying = true;
            Map.SetActive (true);
            Destroy (Guard);
        } else {
            helper.PurposeChange ("全てのクリスタルを接続する(" + crystals + "/7)");
            if (crystals == 7) {
                Warp2.SetActive (true);
                helper.PurposeDisplay ("スタート地点に戻る");
                helper.TelopDisplay ("全てのクリスタルが繋がった！\nスタート地点に魔法陣が出現しました。");
            }
        }
    }

    public void Worp_2 () {
        StartCoroutine (WarpCor_2 ());
    }

    private IEnumerator WarpCor_2 () {
        isPlaying = false;
        isWarping = true;
        helper.WhiteFadeOut ();
        yield return new WaitForSeconds (2f);
        warpPos2.y = 0.16f;
        player.position = warpPos2;
        player.rotation = Quaternion.Euler (warpRot2);
        helper.WhiteFadeIn ();
        while (true) {
            GameObject destroyUma = GameObject.Find ("SpawnedUma");
            if(destroyUma != null) {
                Destroy (destroyUma);
                yield return null;
            } else {
                break;
            }
        }
        Destroy (Warp2);
        Warp3.SetActive (true);
        EndUmaTrigger.SetActive (true);
        Map.SetActive (false);
        yield return new WaitForSeconds (2f);
        isPlaying = true;
        isWarping = false;
        isEnding1 = true;
        helper.TelopDisplay ("この道を抜けた先にゴールの魔法陣があります。\n止まるんじゃねぇぞ...！");
        helper.PurposeDisplay ("ゴールまで走り抜ける");
    }

    public void Catched () {
        isPlaying = false;
        isCatched = true;
        helper.Catched ();
        TimeCount.SetActive (false);
        Map.SetActive (false);
        How.SetActive (false);
        StartCoroutine (GameOverCor ());
    }

    private IEnumerator GameOverCor () {
        yield return new WaitForSeconds (0.5f);
        GameOverUma.SetActive (true);
        yield return new WaitForSeconds (2f);
        helper.BlackFadeOut ();
        yield return new WaitForSeconds (2.1f);
        TitleManager.state = 2;
        SceneManager.LoadScene ("TitleScene");
    }

    public void Warp_3 () {
        isClear = true;
        StartCoroutine (ClearWarp ());
    }

    private IEnumerator ClearWarp () {
        helper.WhiteFadeOut ();
        player.DORotate (new Vector3 (0, 180f, 0), 0.5f);
        yield return new WaitForSeconds (2.1f);
        TitleManager.state = 1;
        TitleManager.score_time = time;
        SceneManager.LoadScene ("TitleScene");
    }

    public void PauseGoTitle () {
        Time.timeScale = 1f;
        TitleManager.state = 0;
        SceneManager.LoadScene ("TitleScene");
    }

}

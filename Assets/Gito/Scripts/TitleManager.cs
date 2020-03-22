using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class TitleManager : MonoBehaviour {

    private GameHelper helper;

    [SerializeField] private GameObject Clear, GameOver, How, ParticleEffect;
    [SerializeField] private TextMeshProUGUI score_text;

    //0-特になし, 1-クリア, 2-ゲームオーバー
    public static int state = 0;

    public static float score_time = -1;
    
    [SerializeField] private AudioSource warpAudio;

    private void Start () {
        helper = GetComponent<GameHelper> ();
        helper.BlackFadeIn ();
        helper.moveAble = false;

        if(state == 1) {
            Clear.SetActive (true);
            score_text.text = string.Format ("Time : {0:F2} sec", score_time);
        }else if(state == 2) {
            GameOver.SetActive (true);
        }
    }

    float t = 0;
    bool b = false;
    private void Update () {
        ParticleEffect.SetActive(GameHelper.particle);

        if (b) {
            return;
        }
        t += Time.deltaTime;
        if (t < 6f) {
            helper.moveAble = false;
            return;
        }
        b = true;
        helper.moveAble = true;
        How.SetActive (true);
    }

    public void GoGameStage () {
        StartCoroutine (FadeChange ());
    }

    private IEnumerator FadeChange () {
        warpAudio.Play ();
        helper.WhiteFadeOut ();
        yield return new WaitForSeconds (2f);
        SceneManager.LoadScene ("GameScene");
    }

}

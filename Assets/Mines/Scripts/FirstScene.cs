using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

// 起動後の最初の画面の処理
public class FirstScene : Utility
{
    // 馬のオブジェクト
    [SerializeField] private GameObject uma;
    // ポストエフェクト
    [SerializeField] private PostProcessProfile profile;
    // press any key のテキスト
    [SerializeField] private Text press;
    // press any key　が点滅する速さ
    [SerializeField] private float press_blinkTime = 1.0f;
    // すでにクリックされたか、クリック可能か
    private bool clicked = false, clickAble = false;

    private void Start()
    {
        // 起動時はHD画質でウィンドウモード
        Screen.SetResolution(1280, 720, false);

        // ポストエフェクトの設定値
        profile.GetSetting<Bloom>().active = true;
        profile.GetSetting<Bloom>().intensity.value = 25f;
        profile.GetSetting<MotionBlur>().active = true;
        profile.GetSetting<ColorGrading>().brightness.value = -40f;

        // 馬のアニメーションは行わない
        uma.GetComponent<UmaAnimation>().MoveAnim(false);

        // フェードイン　クリックを可能にし文字を点滅
        fader.FadeIn(FadeColor.Black, 5f, () =>
        {
            StartCoroutine(PressBlink());
            clickAble = true;

        });
    }

    private void Update()
    {
        // 何かのボタンが押されたら
        if (Input.anyKey && !clicked && clickAble)
        {
            // 一回だけ可能
            clicked = true;
            // 馬のアニメーションを実行
            uma.GetComponent<UmaAnimation>().MoveAnim(true);
            // フェードアウトしてシーン遷移
            SceneChangeFadeOut(FadeColor.Black, Scene.Title, 5f);
            // 文字を消す
            press.gameObject.SetActive(false);
        }
    }

    // 文字を点滅させるコルーチン
    private IEnumerator PressBlink()
    {
        while (true)
        {
            press.DOFade(1f, press_blinkTime);
            yield return new WaitForSeconds(press_blinkTime);
            press.DOFade(0f, press_blinkTime);
            yield return new WaitForSeconds(press_blinkTime);
        }
    }
}

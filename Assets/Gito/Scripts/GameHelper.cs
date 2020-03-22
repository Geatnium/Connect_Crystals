using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Rendering.PostProcessing;

public class GameHelper : MonoBehaviour {

    [SerializeField] private PlayerController player;

    [SerializeField] private GameObject Telop, Purpose, BlackOut, Pause;

    private RectTransform telop_transform, purpose_transform;
    private Text telop_text, purpose_text;
    private Image telop_image, purpose_image, black_image;

    private float anim_speed = 0.5f;
    private float display_time = 5f;

    public bool moveAble = true;

    [SerializeField] private PostProcessProfile profile;

    public static bool postProcess = true, particle = true;
    public static float bright = 10f, sense = 50f;

    [SerializeField] private Toggle post_toggle, particle_toggle, fullScreen_toggle;
    [SerializeField] private Slider bright_slider, sense_slider;
    [SerializeField] private Text bright_text, sense_text;

    private ColorGrading colorGrading;
    private Bloom bloom;
    private MotionBlur motionBlur;

    private void Awake () {
        DOTween.defaultEaseType = Ease.Linear;

        black_image = BlackOut.GetComponent<Image> ();
        telop_transform = Telop.GetComponent<RectTransform> ();
        purpose_transform = Purpose.GetComponent<RectTransform> ();
        telop_image = Telop.GetComponent<Image> ();
        purpose_image = Purpose.GetComponent<Image> ();
        telop_text = Telop.GetComponentInChildren<Text> ();
        purpose_text = Purpose.GetComponentInChildren<Text> ();
    }

    private void Start () {
        fullScreen_toggle.isOn = Screen.fullScreen;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        PostProcessInit ();
    }

    public void BlackFadeIn () {
        StartCoroutine (StartFadeIn ());
        black_image.color = new Color (0, 0, 0, 1);
    }

    public void WhiteFadeIn () {
        StartCoroutine (StartFadeIn ());
        black_image.color = new Color (1, 1, 1, 1);
    }

    public void BlackFadeOut () {
        StartCoroutine (StartFadeOut ());
        black_image.color = new Color (0, 0, 0, 0);
    }

    public void WhiteFadeOut () {
        StartCoroutine (StartFadeOut ());
        black_image.color = new Color (1, 1, 1, 0);
    }

    private IEnumerator StartFadeIn () {
        moveAble = false;
        black_image.DOFade (1f, 0);
        BlackOut.SetActive (true);
        black_image.DOFade (0f, 2f);
        yield return new WaitForSeconds (2f);
        Camera.main.transform.localEulerAngles = Vector3.zero;
        BlackOut.SetActive (false);
        moveAble = true;
        yield break;
    }

    private IEnumerator StartFadeOut () {
        moveAble = false;
        BlackOut.SetActive (true);
        black_image.DOFade (1f, 2f);
        yield return new WaitForSeconds (2f);
        Camera.main.transform.localEulerAngles = Vector3.zero;
        BlackOut.SetActive (true);
        yield break;
    }

    public void TelopDisplay (string message) {
        CancelInvoke ("DownTelop");
        telop_transform.DOMoveY (0, 0);
        telop_image.DOFade (0f, 0f);
        telop_text.DOFade (0f, 0f);
        telop_text.text = message;

        telop_transform.DOMoveY (Screen.height / 10.0f, anim_speed);
        telop_image.DOFade (0.5f, anim_speed);
        telop_text.DOFade (1f, anim_speed);

        Invoke ("DownTelop", display_time);
    }

    private void DownTelop () {
        telop_transform.DOMoveY (0, anim_speed);
        telop_image.DOFade (0f, anim_speed);
        telop_text.DOFade (0f, anim_speed);
    }

    private string m;
    public void PurposeDisplay (string message) {
        purpose_image.DOFade (0f, anim_speed);
        purpose_text.DOFade (0f, anim_speed);
        m = message;
        purpose_transform.DOLocalMoveX (-650f, anim_speed);

        Invoke ("PurposeDisplay2", anim_speed + 0.5f);
    }

    public void PurposeChange (string message) {
        purpose_text.text = "目的 : " + message;
    }

    private void PurposeDisplay2 () {
        purpose_image.DOFade (0.5f, anim_speed);
        purpose_text.DOFade (1f, anim_speed);
        purpose_text.text = "目的 : " + m;
        purpose_transform.DOLocalMoveX (-427.5f, anim_speed);
    }

    public void TelopPurposeDelete () {
        Purpose.SetActive (false);
        Telop.SetActive (false);
    }

    public void Catched () {
        Telop.SetActive (false);
        Purpose.SetActive (false);
        moveAble = false;
    }

    public void DoPause () {
        if (Pause.activeSelf) {
            PauseClose ();
        } else {
            PauseOpen ();
        }
    }

    private void PauseOpen () {
        Time.timeScale = 0;
        moveAble = false;
        Pause.SetActive (true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        StartCoroutine (PauseUpdate ());
    }

    private void PauseClose () {
        Time.timeScale = 1f;
        moveAble = true;
        Pause.SetActive (false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void GameExit()
    {
        Application.Quit();
    }

    private void PostProcessInit () {
        colorGrading = profile.GetSetting<ColorGrading> ();
        bloom = profile.GetSetting<Bloom> ();
        motionBlur = profile.GetSetting<MotionBlur> ();

        bloom.intensity.value = 4f;

        post_toggle.isOn = postProcess;
        particle_toggle.isOn = particle;
        bright_slider.value = bright;
        sense_slider.value = sense;

        bloom.active = postProcess;
        motionBlur.active = postProcess;
        colorGrading.brightness.value = bright;
        player.mouse_sense = sense;

    }

    private IEnumerator PauseUpdate () {
        PostProcessInit ();

        while (true) {
            if (!Pause.activeSelf) {
                yield break;
            }

            postProcess = post_toggle.isOn;
            particle = particle_toggle.isOn;
            bloom.active = postProcess;
            motionBlur.active = postProcess;

            bright = bright_slider.value;
            colorGrading.brightness.value = bright;

            sense = sense_slider.value;
            player.mouse_sense = sense;

            bright_text.text = string.Format ("{0:F1}", bright);
            sense_text.text = string.Format ("{0:F1}", sense);

            yield return null;
        }
    }

    public void ToggleFullScreen(bool fullScreen)
    {
        if (fullScreen)
        {
            Screen.SetResolution(1920, 1080, true);
        }
        else
        {
            Screen.SetResolution(1280, 720, false);
        }
    }
}

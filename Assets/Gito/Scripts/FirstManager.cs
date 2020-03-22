using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;

public class FirstManager : MonoBehaviour {

    [SerializeField] private Light pointLight;
    [SerializeField] private float intensity, openTime;
    [SerializeField] Text press, ear;

    [SerializeField] private Transform root;
    [SerializeField] private float anim_speed;

    [SerializeField] private PostProcessProfile profile;

    private bool able = false, going = false;

    private void Start () {
        Screen.SetResolution(1280, 720, false);

        profile.GetSetting<Bloom> ().active = true;
        profile.GetSetting<Bloom> ().intensity.value = 42f;
        profile.GetSetting<MotionBlur> ().active = true;
        profile.GetSetting<ColorGrading> ().brightness.value = -40f;

        StartCoroutine (Opening ());
    }

    private void Update () {
        if (going) {
            float sin1 = Mathf.Sin (2f * Mathf.PI * anim_speed * Time.time);
            float sin2 = Mathf.Sin (2f * Mathf.PI * anim_speed * Time.time / 2);

            root.localPosition = new Vector3 (sin2 * 0.3f, sin1 * 0.15f, 0);
            return;
        }
        if (able && !going) {
            float sin = Mathf.Sin (2f * Mathf.PI * 0.5f * Time.time) * 0.5f;
            press.color = new Color (1, 1, 1, sin + 0.5f);
            if (Input.anyKey && !going) {
                going = true;
                press.color = Color.clear;
                ear.DOFade (0f, openTime);
                StartCoroutine (Going ());
            }
        }
    }

    private IEnumerator Opening () {
        yield return new WaitForSeconds (1f);
        pointLight.DOIntensity (intensity, openTime);
        ear.DOFade (0.5f, openTime);
        DOTween.To (
            () => RenderSettings.reflectionIntensity,
            num => RenderSettings.reflectionIntensity = num,
            1f,
            openTime
        );
        yield return new WaitForSeconds (openTime);
        able = true;
    }

    private IEnumerator Going () {
        pointLight.DOIntensity (0, openTime);
        yield return new WaitForSeconds (openTime);
        SceneManager.LoadScene ("TitleScene");
    }
}

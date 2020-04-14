using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Audio;


public class Setting : Utility
{
    // 設定保存のキー
    private readonly string
        KEY_FULLSCREEN = "fullscreen",
        KEY_POSTPROCESS = "postprocess",
        KEY_HOWDISPLAY = "howdisplay",
        KEY_BRIGHTNESS = "brightness",
        KEY_MOUSESENSITIVITY = "mousesensitivity",
        KEY_VOLUME = "volume";

    // 設定変更に使用するUI
    [SerializeField] private Toggle fullScreen_toggle, postProcess_toggle, howDisplay_toggle;
    [SerializeField] private Slider brightness_slider, mouseSensi_slider, volume_slider;
    // ポストプロセスのプロフィル
    [SerializeField] private PostProcessProfile profile;

    [SerializeField] private AudioMixer masterMixer;


    private bool fullScreen
    {
        get { return GetSettingData(KEY_FULLSCREEN, false); }
        set { PlayerPrefs.SetInt(KEY_FULLSCREEN, value ? 1 : 0); }
    }

    private bool postProcess
    {
        get { return GetSettingData(KEY_POSTPROCESS, true); }
        set { PlayerPrefs.SetInt(KEY_POSTPROCESS, value ? 1 : 0); }
    }

    private bool howDisplay
    {
        get { return GetSettingData(KEY_HOWDISPLAY, true); }
        set { PlayerPrefs.SetInt(KEY_HOWDISPLAY, value ? 1 : 0); }
    }

    private float brightness
    {
        get { return GetSettingData(KEY_BRIGHTNESS, 50f); }
        set { PlayerPrefs.SetFloat(KEY_BRIGHTNESS, value); }
    }

    private float mouseSensi
    {
        get { return GetSettingData(KEY_MOUSESENSITIVITY, 50f); }
        set { PlayerPrefs.SetFloat(KEY_MOUSESENSITIVITY, value); }
    }

    private float volume
    {
        get { return GetSettingData(KEY_VOLUME, 50f); }
        set { PlayerPrefs.SetFloat(KEY_VOLUME, value); }
    }

    // シーン開始時に保存されている設定を反映させる
    private void Start()
    {
        fullScreen_toggle.isOn = fullScreen;
        postProcess_toggle.isOn = postProcess;
        howDisplay_toggle.isOn = howDisplay;
        brightness_slider.value = brightness;
        mouseSensi_slider.value = mouseSensi;
        volume_slider.value = volume;

        profile.GetSetting<Bloom>().intensity.value = 4f;

        SetFullScreenMode(fullScreen_toggle.isOn);
        SetPostProcess(postProcess_toggle.isOn);
        SetHowDisplay(howDisplay_toggle.isOn);
        SetBrightness(brightness_slider.value);
        SetMouseSensitivity(mouseSensi_slider.value);
        SetMasterVolume(volume_slider.value);
    }

    // フルスクリーンモードの切り替え
    public void SetFullScreenMode(bool enable)
    {
        fullScreen = enable;
        if (enable)
        {
            Screen.SetResolution(1920, 1080, true);
        }
        else
        {
            Screen.SetResolution(1280, 720, false);
        }
    }

    // ポストエフェクトの切り替え
    public void SetPostProcess(bool enable)
    {
        postProcess = enable;
        profile.GetSetting<Bloom>().active = enable;
        profile.GetSetting<MotionBlur>().active = enable;
        profile.GetSetting<Vignette>().active = enable;
    }

    // 操作方法の表示切り替え
    public void SetHowDisplay(bool enable)
    {
        howDisplay = enable;
        helper.HowDisplaySetting(enable);
    }

    // 明るさの変更
    public void SetBrightness(float value)
    {
        brightness = value;
        profile.GetSetting<ColorGrading>().brightness.value = value;
    }

    // 視点移動感度の変更
    public void SetMouseSensitivity(float value)
    {
        mouseSensi = value;
        player.SetMouseSensitivity(value);
    }

    // 音量の変更
    public void SetMasterVolume(float value)
    {
        volume = value;
        masterMixer.SetFloat("MasterVolume", 20f * Mathf.Log10(value / 100f));
    }

    // 保存されているデータを読み出す
    private bool GetSettingData(string key, bool defaultValue)
    {
        int d = defaultValue ? 1 : 0;
        int g = PlayerPrefs.GetInt(key, d);
        return g == 1;
    }
    private float GetSettingData(string key, float defaultValue)
    {
        return PlayerPrefs.GetFloat(key, defaultValue);
    }
}

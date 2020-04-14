using UnityEngine;

// 効果音のピッチの種類
public enum SoundEffectPitch
{
    x1, x2
}

// 2D効果音のクラス
public class SoundEffecter : MonoBehaviour
{
    // ピッチが1のAudioSource、ピッチが2のAudioSource
    private AudioSource audioSource_pitch1, audioSource_pitch2;

    private void Start()
    {
        audioSource_pitch1 = transform.GetChild(0).GetComponent<AudioSource>();
        audioSource_pitch2 = transform.GetChild(1).GetComponent<AudioSource>();
    }

    // 鳴らす　クリップ、ピッチ
    public void Play(AudioClip clip, SoundEffectPitch pitch)
    {
        // 指定されたピッチによって鳴らすソースを変える
        switch (pitch)
        {
            case SoundEffectPitch.x1:
                audioSource_pitch1.PlayOneShot(clip);
                break;
            case SoundEffectPitch.x2:
                audioSource_pitch2.PlayOneShot(clip);
                break;
            default:
                break;
        }

    }
}


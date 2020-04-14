using UnityEngine;

public class UmaAnimation : MonoBehaviour
{
    private AudioSource audioSource;
    // 足音のクリップ
    [SerializeField] private AudioClip footStep;
    // 足音を鳴らさないか
    private bool isFootStepMute = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // 馬のアニメーションを切り替え
    public void MoveAnim(bool enable)
    {
        Animator anim = GetComponent<Animator>();
        // アニメーションを変える
        anim.SetBool("Move", enable);
        if (enable)
        {
            // アニメーションスピードをある程度ランダムに
            anim.SetFloat("AnimSpeed", 1.0f + Random.value * 0.3f);
        }
    }

    // 足音を消す
    public void FootStepMute(bool enable)
    {
        isFootStepMute = enable;
    }

    // 足音を鳴らす
    public void FootSound()
    {
        if (!isFootStepMute)
        {
            audioSource.PlayOneShot(footStep);
        }
    }
}

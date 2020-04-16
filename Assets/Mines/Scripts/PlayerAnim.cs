using UnityEngine;

// プレイヤーのアニメーション
public class PlayerAnim : MonoBehaviour
{
    private CharacterController controller;
    private Animator animator;
    private AudioSource audioSource;
    [SerializeField] private AudioClip footStep;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public void StopAnim()
    {
        animator.Play("Idle");
    }

    private void Update()
    {
        // プレイヤーの速度によってアニメーションを切り替え
        animator.SetFloat("Velocity", controller.velocity.magnitude);
    }

    // アニメーションイベントからこの関数を呼んでもらう　足音を鳴らす
    public void FootStepSound()
    {
        audioSource.PlayOneShot(footStep);
    }
}

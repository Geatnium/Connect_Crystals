using UnityEngine;
using DG.Tweening;

// ゲームオーバー時のエフェクト
public class GameOverUma : Utility
{
    // 馬が出現する方向
    private enum Direction
    {
        Up, Down
    }
    [SerializeField] private Direction direction;
    // アニメーション
    private UmaAnimation anim;
    // 効果音
    [SerializeField] private AudioClip sound;
    // 出現するときのアニメーションの最終座標
    private Vector3 origPos;

    private void OnEnable()
    {
        // アニメーションを再生
        anim = GetComponent<UmaAnimation>();
        anim.MoveAnim(true);
        // 座標を保存 ここに向けてアニメーション
        origPos = transform.localPosition;
        Vector3 p = origPos;
        // 上方向にアニメーションの時はy座標を3に、下方向は-3に
        p.y = direction == Direction.Up ? 3f : -3f;
        transform.localPosition = p;
        // 0〜1までのランダムな時間遅らせる
        Invoke("RandomLate", Random.value);
    }

    private void RandomLate()
    {
        // 効果音を鳴らしてアニメーションで表示
        soundEffecter.Play(sound, SoundEffectPitch.x1);
        transform.DOLocalMove(origPos, 0.8f);
    }
}

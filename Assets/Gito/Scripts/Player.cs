using UnityEngine;
using DG.Tweening;

// プレイヤーを管理しているクラス
// ここからコントローラーの制御もする
public class Player : MonoBehaviour
{
    private enum PlayerState
    {
        Stop, Move, Captured
    }
    private PlayerState playerState = PlayerState.Stop;
    // プレイヤーコントローラー 最初に取得したらキャッシュする
    private PlayerController _playerController;
    private PlayerController playerController
    {
        get
        {
            if (_playerController == null)
            {
                _playerController = GetComponent<PlayerController>();
            }
            return _playerController;
        }
    }

    // 動けるようにする
    public void MoveActive()
    {
        playerState = PlayerState.Move;
    }

    // 動けないようにする
    public void MoveStop()
    {
        playerState = PlayerState.Stop;
    }

    // 視点移動感度を変える
    public void SetMouseSensitivity(float sensitivity)
    {
        playerController.SetMouseSensi(sensitivity);
    }

    private void Update()
    {
        // 動ける時は、コントローラーのUpdateを呼び出す
        if (playerState == PlayerState.Move)
        {
            playerController.PUpdate();
        }
        else
        {
            playerController.Stopping();
        }
    }

    // 馬のトリガーに触れてしまった時
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Uma"))
        {
            // Moveの状態の時だけ捕まる 
            if (playerState == PlayerState.Move)
            {
                // 状態を捕まったにする
                playerState = PlayerState.Captured;
                // 捕まえた馬の方向を向く
                transform.DOLookAt(other.transform.position, 0.25f);
                transform.GetChild(0).DOLocalRotate(Vector3.zero, 0.25f);
                // マネージャークラスでゲームオーバーの後処理をやってもらう
                GameObject.FindWithTag("Manager").GetComponent<GameManager>().GameOver();
            }
        }
    }
}

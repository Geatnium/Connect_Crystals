using UnityEngine;

// 移動と視点移動を行うクラス
public class PlayerController : MonoBehaviour
{
    // スピード　マウス感度　首のトランスフォーム　コントローラー　回転の変数
    [SerializeField] private float speed;
    private float mouseSensi = 50f;
    private Transform neck;
    private CharacterController controller;
    private Vector3 rot;

    // マウス感度を変更
    public void SetMouseSensi(float sensi)
    {
        mouseSensi = sensi;
    }

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        neck = transform.GetChild(0);
        rot = transform.eulerAngles;
    }

    // 動かせない時
    public void Stopping()
    {
        // コントローラーを無効にする
        if (controller.enabled)
        {
            controller.SimpleMove(Vector3.zero);
        }
        rot.y = transform.eulerAngles.y;
        rot.x = neck.localEulerAngles.x;
        if (rot.x > 180f)
        {
            rot.x = -(360f - rot.x);
        }
    }

    public void PUpdate()
    {
        // コントローラーが無効だったら有効にする
        if (!controller.enabled)
        {
            controller.enabled = true;
        }
        // WASDの入力
        float h = MyInput.GetAxis("Horizontal");
        float v = MyInput.GetAxis("Vertical");
        // Shiftの入力でスピードを変更
        float s = speed * (1 + MyInput.GetAxis("Dash"));
        // 移動方向の計算　単位ベクトル化
        Vector3 dir = Vector3.Normalize(transform.forward * v + transform.right * h);
        // コントローラーで移動
        controller.SimpleMove(dir * s);

        // マウス感度の調整
        float m = mouseSensi * 0.05f;
        // マイスの移動量か矢印キーの入力を取得
        float mX = MyInput.GetAxis("Mouse X") * m
         + MyInput.GetAxis("LeftRight") * m * 400f * Time.deltaTime;
        float mY = MyInput.GetAxis("Mouse Y") * m
         + MyInput.GetAxis("UpDown") * m * 400f * Time.deltaTime;
        // 体や首を回転
        rot = new Vector3(Mathf.Clamp(rot.x - mY, -80, 80), rot.y + mX, 0f);
        transform.eulerAngles = new Vector3(0, rot.y, 0);
        neck.localEulerAngles = new Vector3(rot.x, 0, 0);
    }
}

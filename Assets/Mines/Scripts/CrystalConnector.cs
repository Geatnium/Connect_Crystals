using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

// クリスタルの色の種類
public enum CrystalColor
{
    Blue, Green, Orange, Pink, Purple, Red, Yellow
}

// クリスタルの状態
public enum CrystalState
{
    Nothing, Ready, Connecting, Connected
}

// クリスタルの接続についてのクラス
public class CrystalConnector : Utility
{
    // このクリスタルの状態
    private CrystalState state = CrystalState.Nothing;
    // 状態を取得
    public CrystalState GetState()
    {
        return state;
    }

    // このクリスタルの色
    [SerializeField] private CrystalColor crystalColor;
    // 接続に必要な時間
    [SerializeField] private float connectionNeedTime = 5f;
    // クリスタルの実物
    [SerializeField] private GameObject crystal;
    // チャージのエフェクト、チャージ成功後のエフェクト
    [SerializeField] private ParticleSystem chargeEffect, connectedEffect;
    // 常時なっている音のソース、効果音のソース
    [SerializeField] private AudioSource environment_source, effectSound_source;
    // 接続されていないときの環境音、接続された後の環境音、接続中の音、接続完了した瞬間の音
    [SerializeField] private AudioClip noConnectedSound, connectedSound, chargeSound, connectionSound;

    // クリスタルのマテリアル
    private Material mat;
    // マネージャークラス
    private GameManager manager;
    // ゲージのUI
    private Slider gauge;
    // 進行度
    private float progress = 0f;

    private void Start()
    {
        // 取得
        mat = crystal.GetComponent<MeshRenderer>().material;
        manager = GameObject.FindWithTag("Manager").GetComponent<GameManager>();
        gauge = GameObject.FindWithTag("GameUI").transform.Find("Gauge").GetComponent<Slider>();
        // ゲージの最大を設定
        gauge.maxValue = connectionNeedTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 接続完了後は何もできない
        if (state == CrystalState.Connected) return;
        // プレイヤーが触れた時
        if (other.gameObject.CompareTag("Player"))
        {
            // フラグを立てる
            state = CrystalState.Ready;
            // アクションボタンの内容を表示
            helper.ActionDisplay("接続");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 接続間呂後は何もできない
        if (state == CrystalState.Connected) return;

        // 離れた時
        if (other.gameObject.CompareTag("Player"))
        {
            // 接続中の時はキャンセル処理
            if (state == CrystalState.Connecting)
            {
                CancelConnection();
            }
            // フラグを下げる
            state = CrystalState.Nothing;
            // アクションボタンを非表示
            helper.ActionHidden();
        }
    }

    private void Update()
    {
        // 接続完了後は何もできない
        if (state == CrystalState.Connected) return;

        // トリガーに触れている時
        if (state == CrystalState.Ready)
        {
            // アクションボタンを押したら接続開始
            if (Input.GetButtonDown("Action"))
            {
                StartConnection();
            }
        }
        // 接続中
        if (state == CrystalState.Connecting)
        {
            ConnectingUpdate();
        }
    }

    // 接続開始
    private void StartConnection()
    {
        // 状態を変更
        state = CrystalState.Connecting;
        // アクションを非表示
        helper.ActionHidden();
        // ゲージを表示
        gauge.gameObject.SetActive(true);
        // チャージの効果音を鳴らす
        effectSound_source.PlayOneShot(chargeSound);
        // チャージのエフェクトを開始
        chargeEffect.Play();
    }

    // 接続中の処理
    private void ConnectingUpdate()
    {
        // 進行度を加算
        progress += Time.deltaTime;
        // ゲージに反映
        gauge.value = progress;
        // 進行度が最大になった
        if (progress >= connectionNeedTime)
        {
            SuccessConnection();
        }
    }

    // 接続中に離れたときの処理
    private void CancelConnection()
    {
        // チャージのエフェクトを消す
        StopChargeEffect();
    }

    // 接続成功
    private void SuccessConnection()
    {
        // 状態を変更
        state = CrystalState.Connected;
        // チャージのエフェクトを消す
        StopChargeEffect();
        // キラキラのエフェクトを表示
        connectedEffect.Play();
        // クリスタルを光らせる
        mat.DOFloat(2f, "_EnvironmentLight", 2f);
        mat.DOFloat(2f, "_Emission", 2f);
        // クリスタルの環境音を変える
        environment_source.clip = connectedSound;
        environment_source.Play();
        // 接続成功の効果音を鳴らす
        effectSound_source.PlayOneShot(connectionSound);
        // 管理クラスでの処理
        manager.ConnectCrystal(crystalColor);
    }

    // チャージのエフェクトや効果音、ゲージを消す
    private void StopChargeEffect()
    {
        // 効果音を消す
        effectSound_source.Stop();
        // チャージエフェクトを停止
        chargeEffect.Stop();
        // 進行をリセット
        progress = 0f;
        gauge.value = 0f;
        // ゲージを非表示
        gauge.gameObject.SetActive(false);
    }
}

using UnityEngine;
using UnityEngine.Events;

// トリガーのイベント
public class TriggerEvent : Utility
{
    // やるイベント
    [SerializeField] private UnityEvent events;
    // ボタンで実行するかどうか
    [SerializeField] private bool keyAction = false;
    // ボタンの場合、アクションボタンの内容
    [SerializeField] private string actionMess;
    // ボタンで実行する場合、テロップの内容 なくても良い
    [SerializeField][Multiline] private string telopMess;
    // 実行する際の効果音
    [SerializeField] private AudioClip soundEffectClip;
    // 実行する際の効果音のピッチ
    [SerializeField] private SoundEffectPitch pitch;
    // 一回しか実行できないか
    [SerializeField] private bool oneTime = true;
    // プレイヤーが触れているか　　　　　実行できるか
    private bool isTrigger = false, able = true;

    private void OnTriggerEnter(Collider other)
    {
        // プレイヤーが触れた時
        if (other.gameObject.CompareTag("Player"))
        {
            // ボタンで実行する場合
            if (keyAction)
            {
                // フラグを立てる
                isTrigger = true;
                // アクションボタンの内容を表示
                helper.ActionDisplay(actionMess);
                // テロップがある場合、表示
                if (!string.IsNullOrEmpty(telopMess))
                {
                    helper.Telop(telopMess);
                }
            }
            // 触れた瞬間に実行される
            else
            {
                EventRun();
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        // 離れた時
        if (other.gameObject.CompareTag("Player"))
        {
            // ボタンで実行する時
            if (keyAction)
            {
                // フラグを下げる
                isTrigger = false;
                // アクションボタンを非表示
                helper.ActionHidden();
            }
        }
    }

    private void EventRun()
    {
        // イベントを実行
        events.Invoke();
        // 効果音があれば鳴らす
        if (soundEffectClip != null)
        {
            soundEffecter.Play(soundEffectClip, pitch);
        }
    }

    private void Update()
    {
        // イベントを一回しか実行できないモードの奴は、一回実行したら何もできない
        if (!able) return;

        // トリガーに触れている時
        if (isTrigger)
        {
            // アクションボタンを押したら実行
            if (Input.GetButtonDown("Action"))
            {
                if (oneTime)
                {
                    able = false;
                }
                EventRun();
            }
        }
    }
}

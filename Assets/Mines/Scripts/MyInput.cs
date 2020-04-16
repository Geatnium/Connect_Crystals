using UnityEngine;

// 入力の制限をするために、このクラスを挟んでInputクラスを利用する
public class MyInput : MonoBehaviour
{
    // 全ての入力を無効化するかのフラグ
    // これがtrueだと、入力できなくなる
    public static bool invalidAnyKey = false;

    // 独自のGetButton
    public static bool GetButton(string button)
    {
        // 無効化されている時は、falseを返す
        if (invalidAnyKey)
        {
            return false;
        }
        // 有効の時は通常動作
        else
        {
            return Input.GetButton(button);
        }
    }

    public static bool GetButtonDown(string button)
    {
        if (invalidAnyKey)
        {
            return false;
        }
        else
        {
            return Input.GetButtonDown(button);
        }
    }

    public static bool GetButtonUp(string button)
    {
        if (invalidAnyKey)
        {
            return false;
        }
        else
        {
            return Input.GetButtonUp(button);
        }
    }

    public static float GetAxis(string axis)
    {
        if (invalidAnyKey)
        {
            return 0f;
        }
        else
        {
            return Input.GetAxis(axis);
        }
    }
}

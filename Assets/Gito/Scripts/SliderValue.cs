using UnityEngine;
using UnityEngine.UI;

// スライダーの値をテキストに反映させるクラス
public class SliderValue : MonoBehaviour
{
    private Slider slider;
    private Text text;

    private void Start()
    {
        // 親からスライダーを取得
        slider = transform.parent.GetComponent<Slider>();
        // テキストを取得
        text = GetComponent<Text>();
    }

    private void Update()
    {
        // 常にスライダーの値に更新し続ける
        text.text = slider.value.ToString();
    }
}

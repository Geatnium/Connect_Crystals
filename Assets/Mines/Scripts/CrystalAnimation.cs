using UnityEngine;

// クリスタルのアニメーション
public class CrystalAnimation : MonoBehaviour
{
    // 上下アニメションの周期、回転アニメーションのスピード
    [SerializeField] private float updownTime = 5f, rotateSpeed = 5f;

    // 円周率
    private const float pi = Mathf.PI;

    private void Update()
    {
        // 上下アニメーションはサイン関数でやる
        float y = Mathf.Sin(2f * pi * (1f / updownTime) * Time.time) * 0.05f;
        transform.localPosition = new Vector3(0, y, 0);
        // 回転アニメーション
        transform.Rotate(new Vector3(0, rotateSpeed * Time.deltaTime, 0), Space.Self);
    }
}

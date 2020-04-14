using UnityEngine;
using System.Collections.Generic;

public class Map : MonoBehaviour
{
    // プレイヤーの座標
    [SerializeField] private Transform player;
    // 移動させるマップのトランスフォーム
    [SerializeField] private RectTransform map;
    // マスクをしているマップの親
    private RectTransform mask;
    // クリスタル
    [SerializeField] private CrystalConnector[] crystals;
    // クリスタルのマーカー
    [SerializeField] private RectTransform[] crystalMarks;
    // クリスタルの方向
    [SerializeField] private RectTransform[] crystalAllows;
    // マップに表示する馬のトランスフォームとマーク
    private List<Transform> umas = new List<Transform>();
    private List<RectTransform> umaMarks = new List<RectTransform>();
    // マップに表示する馬のマークのプレファブ
    [SerializeField] private GameObject umaMark;
    // 生成した馬のマークは、これの入れ子にする
    [SerializeField] private Transform umaMarksParent;


    private void Start()
    {
        mask = GetComponent<RectTransform>();
    }

    // マップに表示する馬を追加
    public void AddMapUma(Transform uma)
    {
        umas.Add(uma);
        GameObject mark = Instantiate(umaMark, umaMarksParent);
        umaMarks.Add(mark.GetComponent<RectTransform>());
    }

    private void Update()
    {
        // マップの親をプレイヤーの向きによって回転させる
        mask.localEulerAngles = new Vector3(0, 0, player.eulerAngles.y);
        // プレイヤーの座標によってマップを移動させる
        map.localPosition = new Vector3(-player.position.x * 15.842f, -player.position.z * 15.842f, 0);

        // まだ接続されていないクリスタルの方向を表示する
        // マップの中心
        Vector3 center = mask.position;
        for (int i = 0; i < crystalMarks.Length; i++)
        {
            // 接続済みのクリスタルはもう表示しない
            if (crystals[i].GetState() == CrystalState.Connected)
            {
                crystalAllows[i].gameObject.SetActive(false);
                continue;
            }
            // クリスタルの座標
            Vector3 m = crystalMarks[i].position;
            // クリスタルの方向
            Vector3 dir = m - center;
            // クリスタルまでの距離
            float dis = dir.magnitude;
            // マップにクリスタルが表示された時は、矢印を表示しない
            crystalAllows[i].gameObject.SetActive(dis > 133f * ((float)Screen.width / 1280f));
            // クリスタルとの角度を計算
            float a = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
            // クリスタルの方向によって回転
            crystalAllows[i].eulerAngles = new Vector3(0, 0, a);
        }

        // マップに馬の位置を反映させる
        for (int i = 0; i < umas.Count; i++)
        {
            umaMarks[i].localPosition = new Vector3(umas[i].position.x * 15.842f, umas[i].position.z * 15.842f, 0);
            umaMarks[i].eulerAngles = Vector3.zero;
        }
    }
}

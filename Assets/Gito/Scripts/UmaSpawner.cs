using UnityEngine;

// 馬を生成するクラス
public class UmaSpawner : MonoBehaviour
{
    // 生成するプレファブ
    [SerializeField] private GameObject umaPrefab;
    [SerializeField] private Map map;

    // 生成する
    public void Spawn()
    {
        GameObject goUma = Instantiate(umaPrefab);
        // 馬を動けるようにして、ランダムな地点にワープ
        UmaPatrol umaPatrol = goUma.GetComponent<UmaPatrol>();
        umaPatrol.MoveAble();
        umaPatrol.RandomWarp();
        // 難易度がイージーの時は、マップに馬を表示
        if (Difficulty.difficult == Difficult.Easy)
        {
            map.AddMapUma(goUma.transform);
        }
    }
}

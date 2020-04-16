using UnityEngine;
using UnityEngine.AI;

// 馬の巡回の処理を行うクラス
public class UmaPatrol : MonoBehaviour
{
    // 動けるようにするかしないか
    [SerializeField] private bool moveAble = false;
    // プレイヤーのトランスフォーム
    private Transform player;
    // 巡回ポイントを変更するインターバル
    [SerializeField] private float patrolInterval = 10f;
    private float t = 0f;
    // プレイヤーを見つけられる距離
    [SerializeField] private float distance = 12f;
    // これをtrueにすると、絶対にプレイヤーを追うようになる
    private bool playerFollow = false;

    // アニメーション
    private UmaAnimation anim
    {
        get { return GetComponent<UmaAnimation>(); }
    }

    // エージェント
    private NavMeshAgent agent
    {
        get { return _agent == null ? _agent = GetComponent<NavMeshAgent>() : _agent; }
    }
    private NavMeshAgent _agent;

    // 巡回ポイントの親
    private Transform pointsParent
    {
        get { return _pointsParent == null ? _pointsParent = GameObject.FindWithTag("EnemyPoints").transform : _pointsParent; }
    }
    private Transform _pointsParent;

    // 当たり判定用のコライダー
    private BoxCollider trigger
    {
        get { return GetComponent<BoxCollider>(); }
    }

    private void Start()
    {
        // 取得
        player = GameObject.FindWithTag("Player").transform;
        
        // 動けない状態だったら当たり判定もなし
        if (!moveAble)
        {
            trigger.isTrigger = false;
        }
    }

    private void Update()
    {
        // 動けない状態の場合、速度などを0に
        if (!moveAble)
        {
            agent.velocity = Vector3.zero;
            agent.SetDestination(transform.position);
            // 足音もミュートに
            anim.FootStepMute(true);
            return;
        }

        // 動ける状態だったら
        // 足音のミュート解除
        anim.FootStepMute(false);
        // プレイヤーを必ず追う場合
        if (playerFollow)
        {
            // 目的地をプレイヤーに
            agent.SetDestination(player.position);
            t = patrolInterval - 10f;
        }
        else // 馬から目視できた場合プレイヤーを追い、そうでない場合はランダムに巡回
        {
            // この馬のポジション
            Vector3 origin = transform.position;
            // プレイヤーのポジション
            Vector3 playerPos = player.position;
            // 馬とプレイヤーの高さを仮想的に合わせる
            origin.y = 0.2f;
            playerPos.y = 0.2f;
            // 馬からプレイヤーへの方向
            Vector3 dir = playerPos - origin;
            // 馬からプレイヤーに向けてレイを発射
            Ray ray = new Ray(origin, dir);
            RaycastHit hit;
            // 馬とプレイヤーの間に障害物がなければ、プレイヤーを追う
            if (Physics.Raycast(ray, out hit, distance))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    // 目的地をプレイヤーに
                    agent.SetDestination(player.position);
                    // 10秒間は追い続ける
                    t = patrolInterval - 10f;
                    return;
                }
            }
            // 馬からプレイヤーを見つけられなければ、ランダムな地点を巡回
            // 時間をカウント
            t += Time.deltaTime;
            // 目的地に到着している、または一定時間経過
            if (agent.remainingDistance < 0.8f || t > patrolInterval)
            {
                // 少しの確率でプレイヤーを目的地に設定、それ以外はランダムな地点を目的地に
                int r = Random.Range(0, pointsParent.childCount + 6);
                if (r >= pointsParent.childCount)
                {
                    // 目的地をプレイヤーに設定
                    agent.SetDestination(player.position);
                    // この場合は、10秒間で次に切り替わる
                    t = patrolInterval - 10f;
                }
                else
                {
                    // 目的地をランダムに選ばれたポイントに設定
                    agent.SetDestination(pointsParent.GetChild(r).position);
                    // 時間カウントをリセット
                    t = 0;
                }
            }
        }
    }

    // 動けるようにする
    public void MoveAble()
    {
        moveAble = true;
        trigger.isTrigger = true;
        anim.MoveAnim(true);
    }

    // 動けないようにする
    public void StopMove()
    {
        moveAble = false;
    }

    // 絶対にプレイヤーを追うようにする
    public void AbsolutePlayerFollow()
    {
        MoveAble();
        playerFollow = true;
    }

    // ランダムな地点にワープする
    public void RandomWarp()
    {
        agent.Warp(RandomPointPos());
    }

    // 巡回ポイントをランダムで返す
    private Vector3 RandomPointPos()
    {
        return pointsParent.GetChild(Random.Range(0, pointsParent.childCount)).position;
    }
    //[SerializeField] private GameObject mark;

    //private Transform mark_transform;

    //[SerializeField] private Transform root;
    //[SerializeField] private float anim_speed;
    //private Vector3[] points = new Vector3[17];

    //private NavMeshAgent agent;

    //private Transform player;

    //private bool player_follow = false;

    //public bool FollowAble = true;

    //[SerializeField] private BoxCollider trigger;

    //private GameManager manager;

    //private AudioSource audio;
    //[SerializeField] private AudioClip footStep, gameOver;

    //GameObject go_mark;

    //private void Start () {
    //    manager = GameObject.FindWithTag ("Manager").GetComponent<GameManager> ();
    //    audio = GetComponent<AudioSource> ();

    //    if (FollowAble) {
    //        //if (TitleTrigger.mode == 0) {
    //        //    go_mark = Instantiate (mark, GameObject.Find ("Canvas/MapMask/Map").transform);
    //        //    mark_transform = go_mark.transform;
    //        //}
    //    }

    //    anim_speed += Random.value * 2f - 1f;

    //    for (int u = 0 ; u < 17 ; u++) {
    //        points[u] = GameObject.Find ("Stage/Points/Point" + u).transform.position;
    //    }

    //    agent = GetComponent<NavMeshAgent> ();
    //    player = GameObject.FindWithTag ("Player").transform;

    //    if (!FollowAble) {
    //        trigger.isTrigger = false;
    //        //go_mark.SetActive (false);
    //        return;
    //    }
    //    trigger.isTrigger = true;


    //    agent.Warp (points[Random.Range (0, points.Length - 7)]);
    //}

    //float foot = 0;
    //bool b = false;
    //private void Update () {
    //    if (!FollowAble) {
    //        trigger.isTrigger = false;
    //        return;
    //    }
    //    trigger.isTrigger = true;

    //    float sin1 = Mathf.Sin (2f * Mathf.PI * anim_speed * Time.time);
    //    float sin2 = Mathf.Sin (2f * Mathf.PI * anim_speed * Time.time / 2);

    //    root.localPosition = new Vector3 (sin2 * 0.3f, sin1 * 0.15f, 0);

    //    //if (manager.isCatched || manager.isWarping) {
    //    //    agent.velocity = Vector3.zero;
    //    //    agent.speed = 0;
    //    //    if (manager.isCatched && !b) {
    //    //        b = true;
    //    //        audio.PlayOneShot (gameOver);
    //    //    }
    //    //    return;
    //    //}
    //    //UmaMove ();

    //    //if (!manager.isEnding1) {
    //    //    //if (TitleTrigger.mode == 0) {
    //    //    //    mark_transform.localPosition = new Vector3 (transform.position.x * 15.842f, transform.position.z * 15.842f, 0);
    //    //    //    mark_transform.eulerAngles = Vector3.zero;
    //    //    //}
    //    //}

    //    foot += Time.deltaTime;
    //    if (foot > (1f / anim_speed)) {
    //        foot = 0;
    //        audio.PlayOneShot (footStep);
    //    }

    //}

    //public void UmaEnding () {
    //    agent.Warp (new Vector3 (Random.Range (19.5f, 21.5f), 0, Random.Range (-18f, -21f)));
    //    transform.DOLookAt (player.position, 0);
    //    transform.DOMove (player.position, 3.5f);
    //}

    //private float t = 0, interval = 15f;
    //private void UmaMove () {
    //    //if (manager.isEnding1) {
    //    //    agent.SetDestination (player.position);
    //    //    return;
    //    //}

    //    Vector3 origin = transform.position + new Vector3 (0, 0.2f, 0);
    //    Vector3 dir = player.position - origin;
    //    Ray ray = new Ray (origin, dir);
    //    RaycastHit hit;
    //    float distance = 12f;
    //    Debug.DrawLine (ray.origin, ray.direction * distance, Color.red);
    //    if(Physics.Raycast(ray, out hit, distance)) {
    //        if (hit.collider.CompareTag ("Player")) {
    //            agent.SetDestination (player.position);
    //            t = 13f;
    //            return;
    //        }
    //    }

    //    t += Time.deltaTime;
    //    if (agent.remainingDistance < 0.8f || t > interval) {
    //        int r = Random.Range (0, points.Length + 5);
    //        if (r >= points.Length) {
    //            agent.SetDestination (player.position);
    //        } else {
    //            agent.SetDestination (points[r]);
    //        }
    //        t = 0;
    //    }
    //}
}

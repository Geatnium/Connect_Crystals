using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class Uma : MonoBehaviour {


    [SerializeField] private GameObject mark;

    private Transform mark_transform;

    [SerializeField] private Transform root;
    [SerializeField] private float anim_speed;
    private Vector3[] points = new Vector3[17];

    private NavMeshAgent agent;

    private Transform player;

    private bool player_follow = false;

    public bool FollowAble = true;

    [SerializeField] private BoxCollider trigger;

    private GameManager manager;

    private AudioSource audio;
    [SerializeField] private AudioClip footStep, gameOver;

    GameObject go_mark;

    private void Start () {
        manager = GameObject.FindWithTag ("Manager").GetComponent<GameManager> ();
        audio = GetComponent<AudioSource> ();

        if (FollowAble) {
            if (TitleTrigger.mode == 0) {
                go_mark = Instantiate (mark, GameObject.Find ("Canvas/MapMask/Map").transform);
                mark_transform = go_mark.transform;
            }
        }

        anim_speed += Random.value * 2f - 1f;

        for (int u = 0 ; u < 17 ; u++) {
            points[u] = GameObject.Find ("Stage/Points/Point" + u).transform.position;
        }

        agent = GetComponent<NavMeshAgent> ();
        player = GameObject.FindWithTag ("Player").transform;

        if (!FollowAble) {
            trigger.isTrigger = false;
            //go_mark.SetActive (false);
            return;
        }
        trigger.isTrigger = true;


        agent.Warp (points[Random.Range (0, points.Length - 7)]);
    }

    float foot = 0;
    bool b = false;
    private void Update () {
        if (!FollowAble) {
            trigger.isTrigger = false;
            return;
        }
        trigger.isTrigger = true;

        float sin1 = Mathf.Sin (2f * Mathf.PI * anim_speed * Time.time);
        float sin2 = Mathf.Sin (2f * Mathf.PI * anim_speed * Time.time / 2);

        root.localPosition = new Vector3 (sin2 * 0.3f, sin1 * 0.15f, 0);

        if (manager.isCatched || manager.isWarping) {
            agent.velocity = Vector3.zero;
            agent.speed = 0;
            if (manager.isCatched && !b) {
                b = true;
                audio.PlayOneShot (gameOver);
            }
            return;
        }
        UmaMove ();

        if (!manager.isEnding1) {
            if (TitleTrigger.mode == 0) {
                mark_transform.localPosition = new Vector3 (transform.position.x * 15.842f, transform.position.z * 15.842f, 0);
                mark_transform.eulerAngles = Vector3.zero;
            }
        }

        foot += Time.deltaTime;
        if (foot > (1f / anim_speed)) {
            foot = 0;
            audio.PlayOneShot (footStep);
        }

    }

    public void UmaEnding () {
        agent.Warp (new Vector3 (Random.Range (19.5f, 21.5f), 0, Random.Range (-18f, -21f)));
        transform.DOLookAt (player.position, 0);
        transform.DOMove (player.position, 3.5f);
    }

    private float t = 0, interval = 15f;
    private void UmaMove () {
        if (manager.isEnding1) {
            agent.SetDestination (player.position);
            return;
        }

        Vector3 origin = transform.position + new Vector3 (0, 0.2f, 0);
        Vector3 dir = player.position - origin;
        Ray ray = new Ray (origin, dir);
        RaycastHit hit;
        float distance = 12f;
        Debug.DrawLine (ray.origin, ray.direction * distance, Color.red);
        if(Physics.Raycast(ray, out hit, distance)) {
            if (hit.collider.CompareTag ("Player")) {
                agent.SetDestination (player.position);
                t = 13f;
                return;
            }
        }

        t += Time.deltaTime;
        if (agent.remainingDistance < 0.8f || t > interval) {
            int r = Random.Range (0, points.Length + 5);
            if (r >= points.Length) {
                agent.SetDestination (player.position);
            } else {
                agent.SetDestination (points[r]);
            }
            t = 0;
        }
    }
}

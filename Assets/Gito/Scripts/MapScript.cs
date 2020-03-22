using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScript : MonoBehaviour {

    [SerializeField] private Transform player;

    private RectTransform map;
    [SerializeField] RectTransform mask;

    [SerializeField] private GameObject Allows;


    [SerializeField] private Crystal[] crystals;
    [SerializeField] private GameObject[] Marks;
    [SerializeField] private GameObject[] Dirs;

    private Transform[] marks = new Transform[6], dirs = new Transform[6];

    private void Start () {
        map = GetComponent<RectTransform> ();

        for(int i=0 ;i<crystals.Length ; i++) {
            marks[i] = Marks[i].transform;
            dirs[i] = Dirs[i].transform;
        }
    }

    private void OnEnable () {
        Allows.SetActive (true);
    }

    private void OnDisable () {
        Allows.SetActive (false);
    }

    private void Update () {
        mask.localEulerAngles = new Vector3 (0, 0, player.eulerAngles.y);

        map.localPosition = new Vector3 (-player.position.x * 15.842f,-player.position.z * 15.842f, 0);

        Vector3 center = mask.position;
        for (int i = 0 ; i < crystals.Length ; i++) {
            if (crystals[i].did) {
                Dirs[i].SetActive (false);
                continue;
            }
            Vector3 m = marks[i].position;
            Vector3 dir = m - center;
            float dis = dir.magnitude;
            Dirs[i].SetActive (dis > 133f * ((float)Screen.width / 1280f));
            float a = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg - 90f;
            dirs[i].eulerAngles = new Vector3 (0, 0, a);
        }
    }
}

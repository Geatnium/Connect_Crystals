using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameOverUma : MonoBehaviour {

    [SerializeField] private Transform root;

    private AudioSource se;

    [SerializeField] private AudioClip[] over;

    private int r;

    private float anim_speed;

    private void OnEnable () {
        se = GetComponent<AudioSource> ();
        r = Random.Range (0, 2);
        anim_speed = 3.0f + Random.value * 2f;

        Invoke ("RandomLate", Random.value);
    }

    private void RandomLate () {
        se.PlayOneShot (over[Random.Range (0, over.Length)]);
        if (transform.localPosition.y > 0) {
            transform.DOLocalMoveY (0, 0.28f);
        } else {
            transform.DOLocalMoveY (-0.24f, 0.28f);
        }
    }

    private void Update () {
        float rr;
        if(r == 0) {
            rr = 1;
        } else {
            rr = -1;
        }
        float sin1 = rr * Mathf.Sin (2f * Mathf.PI * anim_speed * Time.time);
        float sin2 = rr * Mathf.Sin (2f * Mathf.PI * anim_speed * Time.time / 2);

        root.localPosition = new Vector3 (sin2 * 0.3f, sin1 * 0.15f, 0);
    }
}

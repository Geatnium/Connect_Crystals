using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour {

    [SerializeField] private float speed;
    public float mouse_sense;

    [SerializeField] private Transform cam;

    private CharacterController controller;

    private Vector3 rot;

    [SerializeField] private GameHelper helper;
    [SerializeField] private GameManager manager;

    private AudioSource audio;
    [SerializeField] private AudioClip footStep;

    private void Start () {
        controller = GetComponent<CharacterController> ();
        audio = GetComponent<AudioSource> ();

        rot = transform.eulerAngles;
    }

    private void Update () {
        Pause ();

        if (!helper.moveAble) {
            rot.y = transform.eulerAngles.y;
            rot.x = cam.localEulerAngles.x;
            if(rot.x > 180f) {
                rot.x = -(360f - rot.x);
            }
            return;
        }

        Move ();
        MouseLook ();
    }

    float foot = 0;
    bool b = false;
    private void Move () {
        float h = Input.GetAxis ("Horizontal");
        float v = Input.GetAxis ("Vertical");
        float s = speed * (1 + Input.GetAxis ("Dash"));
        Vector3 dir = Vector3.Normalize (transform.forward * v + transform.right * h);
        controller.SimpleMove (dir * s);

        float headSinX = Mathf.Cos (s * Mathf.PI * 1.75f * Time.time) * 0.25f * dir.magnitude;
        float headSinY = Mathf.Sin (s * Mathf.PI * 1.75f * Time.time) * 0.4f * dir.magnitude;
        if(headSinY < 0) {
            headSinY *= -1;
        }

        cam.localPosition = new Vector3 (headSinX, 1 + headSinY, 0);

        if (headSinY <=  0.1f && !b) {
            b = true;
            audio.volume = dir.magnitude * s * 0.1f;
            audio.PlayOneShot (footStep);
        } else if (headSinY >= 0.2f) {
            b = false;
        }

    }
    
    private void MouseLook () {
        if(Time.timeScale < 0.1f) {
            return;
        }
        float m = mouse_sense * 0.05f;
        float mX = Input.GetAxis ("Mouse X") * m
         + Input.GetAxis ("LeftRight") * m * 400f * Time.deltaTime;
        float mY = Input.GetAxis ("Mouse Y") * m
         + Input.GetAxis ("UpDown") * m * 400f * Time.deltaTime;
        rot = new Vector3 (Mathf.Clamp (rot.x - mY, -80, 80), rot.y + mX, 0f);
        transform.eulerAngles = new Vector3 (0, rot.y, 0);
        cam.localEulerAngles = new Vector3 (rot.x, 0, 0);
   }

    public void LookUma (Vector3 pos) {
        cam.DOLookAt (pos, 0.2f);
    }

    private void Pause () {
        if (Input.GetButtonDown ("Pause")) {
            helper.DoPause ();
        }
    }
}

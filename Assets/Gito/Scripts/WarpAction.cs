using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarpAction : MonoBehaviour {

    [SerializeField] private GameObject Ekey;
    [SerializeField] private Text ekey_text;
    [SerializeField] private GameHelper helper;

    private bool able = false;

    private Warp warp;

    private void Start () {
    }

    private void Update () {
        if (!helper.moveAble) {
            Ekey.SetActive (false);
            return;
        }

        if (able) {
            if (Input.GetButtonDown ("Action")) {
                warp.Do ();
                able = false;
                Ekey.SetActive (false);
            }
        }
    }


    public void OnTriggerEnter (Collider other) {
        if (other.gameObject.CompareTag ("Warp")) {
            warp = other.gameObject.GetComponent<Warp> ();
            warp.Do ();
        }
    }

}

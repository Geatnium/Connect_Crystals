using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrystalConnection : MonoBehaviour {

    [SerializeField] private GameObject Ekey, Bar;
    [SerializeField] private Text ekey_text;
    [SerializeField] private float Need_Second;
    [SerializeField] private GameHelper helper;
    
    private Slider crystal_bar;

    private bool able = false, doing = false;
    private float progress = 0;

    private Crystal crystal;

    private void Start () {
        crystal_bar = Bar.GetComponent<Slider> ();
    }

    private void Update () {
        if (!helper.moveAble) {
            Bar.SetActive (false);
            Ekey.SetActive (false);
            return;
        }

        Bar.SetActive (doing);

        if (able) {
            if (Input.GetButtonDown ("Action")) {
                doing = true;
                able = false;
                Ekey.SetActive (false);
                crystal.Connecting ();
            }
        }

        if (doing) {
            CrystalConnecting ();
        }
    }

    private void CrystalConnecting () {
        progress += Time.deltaTime;
        crystal_bar.value = progress;
        if (progress > Need_Second) {
            Ekey.SetActive (false);
            able = false;
            doing = false;
            crystal.Success ();
        }
    }

    public void OnTriggerEnter (Collider other) {
        if (other.CompareTag ("Crystal")) {
            crystal = other.gameObject.GetComponent<Crystal> ();
            if (!crystal.did) {
                able = true;
                Ekey.SetActive (true);
                ekey_text.text = "接続する";
            }
        }
    }

    public void OnTriggerExit (Collider other) {
        if (other.CompareTag ("Crystal")) {
            able = false;
            Ekey.SetActive (false);
            doing = false;
            progress = 0;

            if (!crystal.did) {
                crystal.DisConnected ();
            }
        }
    }
}

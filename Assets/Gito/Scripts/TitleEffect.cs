using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class TitleEffect : MonoBehaviour {

    private Material mat;
    
    [SerializeField] private AudioClip opening;
    private AudioSource se;

    private void Start () {
        mat = GetComponent<TextMeshProUGUI> ().fontMaterial;
        se = GetComponent<AudioSource> ();
        StartCoroutine (TitleAnim ());
    }

    private void Update () {

    }

    private IEnumerator TitleAnim () {
        yield return new WaitForSeconds (1.0f);
        mat.DOFloat (0, "_FaceDilate", 3f);
        se.PlayOneShot (opening);
        yield return new WaitForSeconds (1.5f);
        while (true) {
            mat.SetFloat ("_GlowOuter", 0.25f * Mathf.Sin (2f * Mathf.PI * 0.25f * Time.time) + 0.75f);
            yield return null;
        }
    }
}

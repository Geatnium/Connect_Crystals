using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Crystal : MonoBehaviour {

    [SerializeField] private GameObject crystal, Effect1;

    [SerializeField] private float rot_speed, updown_speed;

    [SerializeField] private ParticleSystem effect1, effect2;

    [SerializeField] private AudioSource environment, effect;
    [SerializeField] private AudioClip crystal2, charge, kirakira; 

    public bool did = false;

    private Material mat;

    private GameManager manager;

    void Start () {
        mat = crystal.GetComponent<MeshRenderer> ().material;
        manager = GameObject.FindWithTag ("Manager").GetComponent<GameManager> ();
    }

    void Update () {
        Effect1.SetActive (GameHelper.particle);

        crystal.transform.Rotate (0, rot_speed * Time.deltaTime, 0);

        float sin = Mathf.Sin (2f * Mathf.PI * updown_speed * Time.time);
        crystal.transform.localPosition = new Vector3 (0, sin * 0.05f, 0);

        if (did) {
            if (!effect1.isPlaying) {
                effect1.Play ();
            }
        }
    }



    public void Connecting () {
        effect2.Play ();
        effect.volume = 0.2f;
        effect.PlayOneShot (charge);
    }

    public void DisConnected () {
        if (!did) {
            effect2.Stop ();
            effect.Stop ();
        }
    }

    public void Success () {
        did = true;
        effect2.Stop ();
        effect1.Play ();

        mat.DOFloat (2f, "_EnvironmentLight", 2f);
        mat.DOFloat (2f, "_Emission", 2f);
        manager.UmaSpawn ();
        environment.clip = crystal2;
        environment.Play ();
        effect.Stop ();
        effect.PlayOneShot (kirakira);
    }

}

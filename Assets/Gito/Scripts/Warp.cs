using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Warp : MonoBehaviour {

    public UnityEvent events = new UnityEvent ();

    [SerializeField] private AudioSource warpAudio;

    public void Do () {
        warpAudio.Play ();
        events.Invoke ();
    }

}

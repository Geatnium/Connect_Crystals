using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndUmaActivate : MonoBehaviour {

    [SerializeField] private Uma uma;

    private void Start () {

    }


    private void Update () {

    }

    public void OnTriggerEnter (Collider other) {
        if (other.CompareTag ("Player")) {
            uma.FollowAble = true;
            Destroy (gameObject);
        }
    }
}

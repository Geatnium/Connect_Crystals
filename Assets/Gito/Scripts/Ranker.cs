using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ranker : MonoBehaviour {
    [SerializeField] private Text rank_text, name_text, time_text;

    public void SetRanker (int rank, string name, float time) {
        rank_text.text = rank.ToString ();
        name_text.text = name;
        time_text.text = string.Format ("{0:F2} sec", time);
    }
}

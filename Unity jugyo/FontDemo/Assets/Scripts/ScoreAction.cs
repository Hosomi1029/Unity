using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //uGUIを扱うのに必要
using UnityEngine.SceneManagement; //シーンのロードに必要

public class ScoreAction : MonoBehaviour {

    public Text[] txtRank;
    float Elapsed = 0.0f;

    void Start() {
        for (int idx = 1; idx <= 5; idx++) {
            if (PlayerPrefs.GetFloat( "R" + idx ) >= float.MaxValue) {
                txtRank[ idx - 1 ].text = "_.__s";
            } else {
                txtRank[ idx - 1 ].text = PlayerPrefs.GetFloat( "R" + idx ).ToString( "f2" ) + "s";
            }
        }
    }

    void Update() {
        if (Input.GetButton( "Fire1" )) {
            Elapsed += Time.deltaTime; //画面を押し続けている間ずっと
            if (Elapsed > 3.0f) {
                SceneManager.LoadScene( "Main" );
            }
        }
        if (Input.GetButtonUp( "Fire1" )) {
            Elapsed = 0.0f; //画面押しを離した
        }
    }

}

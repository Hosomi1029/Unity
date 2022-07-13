using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //uGUIを扱うのに必要
using UnityEngine.SceneManagement; //シーンのロードに必要

public class ScoreAction : MonoBehaviour
{
    public Text[] txtRank;
    private float Elapsed;

    // Start is called before the first frame update
    void Start()
    {
        for (int idx = 1; idx <= 5; idx++)
        {
            if (PlayerPrefs.GetInt("R" + idx) <= 0 )
            {
                txtRank[idx - 1].text = "00000";
            }
            else
            {
                txtRank[idx - 1].text = PlayerPrefs.GetInt("R" + idx).ToString("D6");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Elapsed += Time.deltaTime;
        if(Elapsed > 2.0f)
        {
            SceneManager.LoadScene("Title");
        }
        if (Input.GetButton("BtnA"))
        {
            SceneManager.LoadScene("Title");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //uGUIを利用する際に必要

public class GameManager : MonoBehaviour
{
    public Text txtTime; //画面のGUI文字
    float Elapsed; //経過時間
    bool isPlaying = false;//プレイ中かどうかの真偽値
    bool isLapStop; //Lap表示中かどうかの真偽値

    // Start is called before the first frame update
    void Start()
    {
        Elapsed = 0.0f; //ゼロクリア
        txtTime.text = "0.00s";
        isPlaying = false; //プレイ前
        isLapStop = false; //Lap中ではない
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape ) && isPlaying)
        {
            isLapStop = !isLapStop;
        }
        if (Input.GetMouseButtonDown(0))
        {
            isPlaying = !isPlaying;
        }
        if (isPlaying)
        {
            Elapsed += Time.deltaTime;
            if (!isLapStop)
            {
                txtTime.text = Elapsed.ToString("f2") + "s";
            }
        }
        else
        {
            if(Input.GetMouseButtonDown(1))
            {
                Elapsed = 0.0f;
                txtTime.text = "0.00s";
                isLapStop = false;
            }
        }
    }
}

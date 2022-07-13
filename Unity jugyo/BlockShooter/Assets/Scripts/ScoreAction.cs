using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //uGUIを用いるのに必要

public class ScoreAction : MonoBehaviour
{
    float Elapsed; //経過時間
    //bool TargetIsRed; //ターゲットは赤か？
    public enum TargetColor
    {
        RED,
        BLUE,
        GREEN
    }
    TargetColor tc;
    string color;
    public int Score; //スコア
    public int Reward = 100; //加点ポイント
    public int Penalty = -25; //減点ポイント
    public float Interval = 10; //赤青の切り替え間隔（既定値10秒）
    public Text txtMessage; //メッセージ表示
    public Text txtScore; //スコア表示

    // Start is called before the first frame update
    void Start()
    {
        txtScore.text = "SCORE : 0";
        tc = TargetColor.RED; //赤から開始
        Score = 0; //スコア
    }

    void GameStart()
    {
        enabled = true; //スクリプトを稼働させる
    }
    void OnDestroyBox(string boxColorName)
    {
        //破壊された箱の色名を元に判定して得点計算・表示する
        Score += (boxColorName == GetTargetColorName()) ? Reward : Penalty;
        txtScore.text = "SCORE : " + Score;
    }
    string GetTargetColorName()
    { //真偽値をもとに文字列を返す関数
        //return TargetIsRed ? "Red" : "Blue";
        switch(tc)
        {
            case TargetColor.RED:
                color = "Red";
                break;
            case TargetColor.BLUE:
                color = "Blue";
                break;
            case TargetColor.GREEN:
                color = "Green";
                break;
        }
        return color;

    }

    void TimeUp()
    {
        enabled = false; //スクリプトを停止
    }

    // Update is called once per frame
    void Update()
    {
        Elapsed += Time.deltaTime;
        if (Elapsed > Interval)
        {
            //TargetIsRed = !TargetIsRed; //赤青切り替え
            tc++;
            if(tc > TargetColor.GREEN)
            {
                tc = TargetColor.RED;
            }
            
            Elapsed = 0.0f;
        }
        //ターゲットを字と色で指示
        txtMessage.text = "Shoot " + GetTargetColorName() + " Boxes";
        //txtMessage.color = TargetIsRed ? Color.red : Color.blue;
        switch (tc)
        {
            case TargetColor.RED:
                txtMessage.color = Color.red;
                break;
            case TargetColor.BLUE:
                txtMessage.color = Color.blue;
                break;
            case TargetColor.GREEN:
                txtMessage.color = Color.green;
                break;
        }

    }
}

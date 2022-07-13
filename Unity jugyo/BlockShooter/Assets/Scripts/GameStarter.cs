using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameStarter : MonoBehaviour
{
    ScoreAction SA; //隣のスクリプトScoreAction
    TimeAction TA; //隣のスクリプトTimeAction
    float Elapsed = 3.5f; //3.5 → 3.0の間は何も表示しない
    string CountDown = "";

    void Start()
    {
        SA = GetComponent<ScoreAction>() as ScoreAction;
        TA = GetComponent<TimeAction>() as TimeAction;
        SA.txtMessage.text = "";
        SA.txtScore.text = "";
        TA.txtNavigate.text = "";
        TA.imgFill.fillAmount = 0.0f;
    }
    void Update()
    {
        Elapsed -= Time.deltaTime;
        if (Elapsed <= 0.0f)
        {
            BroadcastMessage("GameStart",
            SendMessageOptions.DontRequireReceiver); //ゲームの開始を放送する
            enabled = false; //このスクリプトは停止
        }
        else if (Elapsed < 3.0f)
        { //0.5秒経過したらカウントダウン表示を行う
            CountDown = "" + Mathf.Ceil(Elapsed); //切り上げた整数部を表示する
            SA.txtMessage.text = CountDown;
            //経過時間 － 切り捨てた整数部 ＝ 小数部（1.0～0.0）これを不透明度に使う
            SA.txtMessage.color = new Color(1, 1, 1, Elapsed - Mathf.Floor(Elapsed));
            //文字の大きさにも使っちゃいました。だんだん小さくなります。
            SA.txtMessage.fontSize = Mathf.FloorToInt((1 + (Elapsed - Mathf.Floor(Elapsed))) * 60);
        }
    }
}
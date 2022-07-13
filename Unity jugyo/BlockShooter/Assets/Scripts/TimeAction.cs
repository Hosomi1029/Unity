using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; //シーン切り替えに必要

public class TimeAction : MonoBehaviour
{
    public float TimeLimit = 60.0f; //制限時間
    float Elapsed; //経過時間
    bool isPlaying; //プレイ中か？の真偽値
    ScoreAction SA; //隣のスクリプトScoreAction
    bool CanLoad = false; //タイトルをロードできるかどうか
    public Text txtNavigate;
    float LongPush = 0.0f;
    public Image imgFill;

    // Start is called before the first frame update
    void Start()
    {
        Elapsed = 0.0f;
        isPlaying = true;
        //隣のスクリプトScoreActionを取得する
        SA = GetComponent<ScoreAction>() as ScoreAction;
        txtNavigate.text = "";
    }

    void GameStart()
    {
        enabled = true; //スクリプトを稼働させる
    }

    // Update is called once per frame
    void Update()
    {
        //長押し3秒の検出
        if (Input.GetMouseButton(0) && CanLoad)
        {
            LongPush += Time.deltaTime;
            if (LongPush > 3.0f)
            {
                //シーンTitleをロードする
                SceneManager.LoadScene("Title");
            }
        }
        else
        {
            LongPush = 0.0f;
        }
        imgFill.fillAmount = LongPush / 3.0f;

        Elapsed += Time.deltaTime;
        if (isPlaying)
        {
            if (Elapsed >= TimeLimit)
            {
                //制限時間を超えたらTimeUpを放送する
                BroadcastMessage("TimeUp", SendMessageOptions.DontRequireReceiver);
                isPlaying = false; //プレイ中を偽にする
                Elapsed = 0.0f;
                SA.txtMessage.color = Color.white;
                SA.txtMessage.text = "TIME UP";
                SA.txtScore.enabled = false;
            }
        }
        else
        {
            if (Elapsed > 4.5f)
            { // > 4.5
                Elapsed = 3.5f;
            }
            else if (Elapsed > 4.3f)
            { // 4.3---4.5
                txtNavigate.text = "";
            }
            else if (Elapsed > 3.5f)
            { //3.5---4.3
                CanLoad = true; //3.5秒以降はタイトルシーンをロード可能にする
                txtNavigate.text = "Long push 3 sec to TITLE";
                SA.txtMessage.text = "Your score is " + SA.Score;
            }
            else if (Elapsed > 3.0f)
            { // 3.0---3.5
                SA.txtMessage.text = "";
            }
        }
    }
}


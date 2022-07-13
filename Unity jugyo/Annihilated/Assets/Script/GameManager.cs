using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //uGUIの利用に必要
using UnityEngine.SceneManagement; //シーンのロードに必要

public class GameManager : MonoBehaviour
{
    int Score; //スコア
    public AudioClip SE_Clear; //クリアサウンド
    public AudioClip SE_Over; //オーバーサウンド
    public AudioClip SE_Tada; //タイトルサウンド
    public Text txtMessage;
    public Text txtScore;
    public Image imgFillBack;
    public Image imgFill;
    public enum STS
    {
        PLAY,
        CLEAR,
        OVER,
    }
    static public STS GameStatus; //ゲームステータス
    AudioSource MyAudio; //自身の音源
    private float Elapsed;


    // Start is called before the first frame update
    void Start()
    {
        GameStatus = STS.PLAY;
        imgFillBack.gameObject.SetActive(false);
        imgFill.fillAmount = 0.0f;
        MyAudio = GetComponent<AudioSource>(); //自身の音源を取得
        txtScore.text = "SCORE:0";
        txtMessage.text = "START!";
        Invoke("TextVanish", 1.5f);
    }

    void TextVanish()
    {
        txtMessage.text = "";
    }

    //スコア加算
    void ScoreUp()
    {
        if (GameStatus == STS.PLAY)
        {
            Score+=100; //プレイ中ならゴースト退治数をカウントアップ
            txtScore.text = "SCORE:"+Score;
        }
    }

    //ゲームクリア処理
    void GameOver()
    {
        GameStatus = STS.OVER;
        txtMessage.text = "    GAMEOVER\nSCORE:" + Score;
        imgFillBack.gameObject.SetActive(true);
        imgFill.fillAmount = 0.0f;
        //全ゴーストを処遇（停止） **************
        GameObject[] Enemys = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject Stored in Enemys)
        {
            Stored.SendMessage("DoNotMove",
            SendMessageOptions.DontRequireReceiver);
        }
        //ランキング管理処理 *******************
        int[] Rank = new int[6]; //ランキング
        for (int idx = 1; idx <= 5; idx++)
        {
            Rank[idx] = PlayerPrefs.GetInt("R" + idx); //ランキングデータ読み込み
        }
        int newRank = 0; //まず今回のタイムを0位と仮定する
        for (int idx = 5; idx > 0; idx--)
        { //逆順 5...1
            if (Rank[idx] > Score)
            {
                newRank = idx; //新しいランクとして判定する
            }
        }
        if (newRank != 0)
        { //0位のままでなかったらランクイン確定
            for (int idx = 5; idx > newRank; idx--)
            {
                Rank[idx] = Rank[idx - 1]; //繰り下げ処理
            }
            Rank[newRank] = Score; //新ランクに登録
            for (int idx = 1; idx <= 5; idx++)
            {
                PlayerPrefs.SetInt("R" + idx, Rank[idx]); //データ領域に保存
            }
        }
        MyAudio.Stop(); //BGM停止
        MyAudio.PlayOneShot(SE_Over); //終了サウンド鳴動
    }

    // Update is called once per frame
    void Update()
    {
        switch (GameStatus)
        {
            case STS.PLAY:
                break;
            default:
                //Ｂボタンを押下中は経過時間を加算
                if (Input.GetButton("BtnA"))
                {
                    Elapsed += Time.deltaTime;
                    if (Elapsed > 2.0f)
                    {
                        SceneManager.LoadScene("Ranking");
                    }
                    imgFill.fillAmount = Elapsed / 2.0f;
                }
                //Ｂボタンを離すと最初から
                if (Input.GetButtonUp("BtnA"))
                {
                    imgFill.fillAmount = 0;
                    Elapsed = 0.0f;
                }
                break;
        }
    }
}

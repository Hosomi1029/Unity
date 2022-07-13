using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //uGUIの利用に必要
using UnityEngine.SceneManagement; //シーンのロードに必要

public class Gamemanager : MonoBehaviour
{
    int GhostCnt; //ゴースト討伐数
    public int GhostAll = 30; //倒すべきゴースト数
    public AudioClip SE_Clear; //クリアサウンド
    public AudioClip SE_Over; //オーバーサウンド
    public AudioClip SE_Tada; //タイトルサウンド
    public AudioClip SE_Vanish; //ゴースト退治サウンド
    public Text txtMessage;
    public Text txtTime;
    public Text txtCount;
    public Image imgFillBack;
    public Image imgFill;
    public float LimitTime = 90.0f; //制限時間
    public enum STS
    {
        PLAY,
        CLEAR,
        OVER,
        TIMEUP
    }
    static public STS GameStatus; //ゲームステータス
    AudioSource MyAudio; //自身の音源
    float Elapsed; //経過時間


    // Start is called before the first frame update
    void Start()
    {
        GameStatus = STS.PLAY;
        imgFillBack.gameObject.SetActive(false);
        imgFill.fillAmount = 0.0f;
        MyAudio = GetComponent<AudioSource>(); //自身の音源を取得
        txtTime.text = "0.00s";
        txtCount.text = "0/" + GhostAll;
        txtMessage.text = "START!";
        Invoke("TextVanish", 1.5f);
    }

    void TextVanish()
    {
        txtMessage.text = "";
    }

    //ゴーストと弾の接触処理
    void OnHitBullet()
    {
        if (GameStatus == STS.PLAY)
        {
            GhostCnt++; //プレイ中ならゴースト退治数をカウントアップ
            MyAudio.PlayOneShot(SE_Vanish); //退治サウンド鳴動
            txtCount.text = GhostCnt + "/" + GhostAll;
            if (GhostCnt >= GhostAll)
            {
                SendMessage("GameClear",
                SendMessageOptions.DontRequireReceiver); //ゲームクリアの宣告
            }
        }
    }

    //ゴーストとプレイヤーの接触処理
    void OnHitGhost()
    {
        GameStatus = STS.OVER;
        txtMessage.text = "GAME OVER";
        txtTime.text = "";
        GameOver(); //ゲームオーバー処理
    }

    //ゲームクリア処理
    void GameClear()
    {
        GameStatus = STS.CLEAR;
        txtTime.text = "";
        txtMessage.text = "Clear! " + Elapsed.ToString("f2") + "s";
        imgFillBack.gameObject.SetActive(true);
        imgFill.fillAmount = 0.0f;
        //全ゴーストを処遇（撤去） **************
        GameObject[] Ghosts = GameObject.FindGameObjectsWithTag("Ghost");
        foreach (GameObject Stored in Ghosts)
        {
            Destroy(Stored);
        }
        //ランキング管理処理 *******************
        float[] Rank = new float[6]; //ランキング
        for (int idx = 1; idx <= 5; idx++)
        {
            Rank[idx] = PlayerPrefs.GetFloat("R" + idx); //ランキングデータ読み込み
        }
        int newRank = 0; //まず今回のタイムを0位と仮定する
        for (int idx = 5; idx > 0; idx--)
        { //逆順 5...1
            if (Rank[idx] > Elapsed)
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
            Rank[newRank] = Elapsed; //新ランクに登録
            for (int idx = 1; idx <= 5; idx++)
            {
                PlayerPrefs.SetFloat("R" + idx, Rank[idx]); //データ領域に保存
            }
        }
        Elapsed = 0.0f;
        MyAudio.Stop(); // BGM鳴動停止;
        MyAudio.PlayOneShot(SE_Clear); //クリアサウンド
    }

    //タイムアップ処理
    void TimeUp()
    {
        GameStatus = STS.TIMEUP;
        txtMessage.text = "TIME UP! ";
        txtTime.text = LimitTime.ToString("f2") + "s";
        GameOver(); //ゲームオーバー処理
    }
    //ゲームオーバー処理
    void GameOver()
    {
        imgFillBack.gameObject.SetActive(true);
        imgFill.fillAmount = 0.0f;
        Elapsed = 0.0f;
        //全ゴーストを処遇（停止） **************
        GameObject[] Ghosts = GameObject.FindGameObjectsWithTag("Ghost");
        foreach (GameObject Stored in Ghosts)
        {
            Stored.SendMessage("DoNotMove",
            SendMessageOptions.DontRequireReceiver);
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
                Elapsed += Time.deltaTime;
                if (Elapsed < LimitTime)
                {
                    txtTime.text = Elapsed.ToString("f2") + "s";
                }
                else
                {
                    SendMessage("TimeUp", SendMessageOptions.DontRequireReceiver);
                }
                break;
            default:
                //Ｂボタンを押下中は経過時間を加算
                if (Input.GetButton("BtnB"))
                {
                    Elapsed += Time.deltaTime;
                    if (Elapsed > 2.0f)
                    {
                        SceneManager.LoadScene("Title");
                    }
                    imgFill.fillAmount = Elapsed / 2.0f;
                }
                //Ｂボタンを離すと最初から
                if (Input.GetButtonUp("BtnB"))
                {
                    imgFill.fillAmount = 0;
                    Elapsed = 0.0f;
                }
                break;
        }
    }
}

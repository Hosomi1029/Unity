using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //uGUIを利用するのに必要

public class GameManager : MonoBehaviour
{
    public enum STS
    {
        TITLE,
        PLAY,
        CLEAR,
        OVER
    }
    static public STS GameStatus; //ゲームの状態
    GameObject Player; //プレイヤー
    AudioSource myAudio; //自身の音源
    float Elapsed; //経過時間
    int myScore; //スコア
    public float LimitTime = 60.0f; // 制限時間の初期値
    public AudioClip SE_Clear; // タイムアップサウンド
    public AudioClip SE_Coin; // コイン獲得サウンド
    public AudioClip SE_Over; // ゲームオーバーサウンド
    public AudioClip SE_Spawn; // コイン生成サウンド
    public AudioClip SE_Title; // タイトルサウンド
    public Image imgTitle;
    public Text txtMessage;
    public Text txtScore;
    public Text txtTime;
    public Text txtNavigate;
    public Image imgButtonA;
    public Image imgButtonB;
    public GameObject Coin500Prefab;
    public GameObject Coin100Prefab;
    public GameObject Coin010Prefab;
    public GameObject EnemyPrefab; //敵のプレハブ
    Vector3 EnemyPos = new Vector3(12, 0, 12); //敵の生成位置


    // Start is called before the first frame update
    void Start()
    {
        //自身の音源を取得
        myAudio = GetComponent<AudioSource>();
        //プレイヤーを探しておく
        Player = GameObject.FindGameObjectWithTag("Player");
        //タイトル画面準備処理
        Ready();
        //コイン生成処理を起動
        StartCoroutine("CoinSpawner");
    }

    //コイン生成処理
    IEnumerator CoinSpawner()
    {
        while (true)
        {
            //ランダム時間を待機
            yield return new WaitForSeconds(Random.Range(0.3f, 1.5f));
            if (GameStatus == STS.PLAY)
            {
                float RanX = Random.Range(-15.0f, 15.0f);
                float RanZ = Random.Range(-10.0f, 10.0f);
                Vector3 pos = new Vector3(RanX, 1.0f, RanZ); //生成位置決定
                if (Random.value < 0.2f)
                { //30%の確率で高い位置
                    pos.y = 2.3f;
                    Instantiate(Coin500Prefab, pos, Quaternion.identity);
                }
                else if(Random.value >= 0.2f && Random.value < 0.5f)
                {
                    Instantiate(Coin100Prefab, pos, Quaternion.identity);

                }
                else
                {
                    Instantiate(Coin010Prefab, pos, Quaternion.identity);

                }
                myAudio.PlayOneShot(SE_Spawn); //生成音鳴動
            }
        }
    }
    //スコア変動処理
    void ChangeScore(int Point)
    {
        if (Point > 0)
        {
            myAudio.PlayOneShot(SE_Coin); //獲得音鳴動
        }
        myScore += Point;
        txtScore.text = "SCORE : " + myScore.ToString().PadLeft(6, '0');
    }

    void Ready()
    {
        GameStatus = STS.TITLE;
        myAudio.PlayOneShot(SE_Title); //タイトルサウンド鳴動
        imgTitle.gameObject.SetActive(true);
        txtMessage.text = "";
        txtTime.text = "";
        txtScore.text = "";
        txtNavigate.text = "Push      Button to Start";
        imgButtonA.gameObject.SetActive(true);
        imgButtonB.gameObject.SetActive(false);
        myScore = 0;//スコアをゼロクリア
        Elapsed = 0.0f;
        //プレイヤーに準備を告げる
        Player.SendMessage("Ready");
        //もし敵が存在したら全撤去する（複数を考慮しておく）
        GameObject[] Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject Stored in Enemies)
        {
            Destroy(Stored);
        }
    }
    void GameStart()
    {
        GameStatus = STS.PLAY;
        myAudio.Play(); //BGM鳴動
        txtScore.text = "SCORE : 00000";
        imgTitle.gameObject.SetActive(false);
        txtNavigate.gameObject.SetActive(false);
        Elapsed = 0.0f;
        Instantiate(EnemyPrefab, EnemyPos, Quaternion.identity); //敵を生成
    }

    void GameOver()
    {
        GameStatus = STS.OVER;
        myAudio.Stop(); //BGM停止
        myAudio.PlayOneShot(SE_Over); //ゲームオーバーサウンド鳴動
        txtMessage.text = "GAME OVER";
        txtScore.text = "";
        txtTime.text = "";
        txtNavigate.text = "Push       Button to Title";
        imgButtonA.gameObject.SetActive(false);
        imgButtonB.gameObject.SetActive(true);
        Elapsed = 0.0f;
        ClearCoins(); //コインを全撤去
    }

    void TimeUp()
    {
        GameStatus = STS.CLEAR;
        //敵に移動停止を告げる（複数を考慮しておく）
        GameObject[] Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject Stored in Enemies)
        {
            Stored.SendMessage("DontMove", SendMessageOptions.DontRequireReceiver);
        }
        myAudio.Stop(); //BGM停止
        myAudio.PlayOneShot(SE_Clear); //ゲームクリアサウンド鳴動
        txtMessage.text = "TIME UP!";
        txtTime.text = "0.00s";
        txtNavigate.text = "Push    Button to Title";
        imgButtonA.gameObject.SetActive(false);
        imgButtonB.gameObject.SetActive(true);
        Elapsed = 0.0f;
        //プレイヤーに移動停止を告げる
        Player.SendMessage("DontMove");
        ClearCoins(); //コインを全撤去する
    }

    //コイン全撤去処理
    void ClearCoins()
    {
        GameObject[] Coins = GameObject.FindGameObjectsWithTag("Coin");
        foreach (GameObject Stored in Coins)
        {
            Destroy(Stored);
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (GameStatus)
        {
            case STS.TITLE:
                Elapsed += Time.deltaTime;
                Elapsed %= 1.0f;
                txtNavigate.gameObject.SetActive(Elapsed < 0.8f);
                if (Input.GetButtonDown("Fire1"))
                { //Xbox:A-Btn or LMB
                    GameStart();
                }
                break;
            case STS.PLAY:
                Elapsed += Time.deltaTime;
                if (Elapsed >= LimitTime)
                {
                    TimeUp();
                }
                else
                {
                    txtTime.text = (LimitTime - Elapsed).ToString("f2") + "s";
                }
                break;

            default:
                Elapsed += Time.deltaTime;
                Elapsed %= 1.0f;
                txtNavigate.gameObject.SetActive(Elapsed < 0.8f);
                if (Input.GetButtonDown("Fire2"))
                { //Xbox:B-Btn or RMB
                    Ready();
                }

                break;
        }

    }
}

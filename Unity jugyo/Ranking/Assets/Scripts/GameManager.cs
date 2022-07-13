using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //uGUIを扱うのに必要
using UnityEngine.SceneManagement; //シーンのロードに必要

public class GameManager : MonoBehaviour {

    public Text txtMessage;
    public enum STS {
        INIT,PLAYING,CLEARED
    }
    STS GameStatus;
    float Elapsed; //経過時間
    float LongPush; //長押し時間
    float Bias = 1.8f;
    float[] Rank = new float[6]; // 作業エリア

    void Start() {
        // アプリのデータ領域が存在するか
        if (PlayerPrefs.HasKey("R1"))
        {
            Debug.Log("データ領域を読み込みました。");
            for (int idx = 1; idx <= 5; idx++)
            {
                Rank[idx] = PlayerPrefs.GetFloat("R" + idx); // データ領域読み込み
            }
        }
        else
        {
            Debug.Log("データ領域を初期化しました。");
            for (int idx = 1; idx <= 5; idx++)
            {
                Rank[idx] = float.MaxValue;
                PlayerPrefs.SetFloat("R" + idx, float.MaxValue); // 最大値を格納する
            }
        }
        Ready();
    }

    void Ready() {
        GameStatus = STS.INIT;
        Elapsed = 0.0f;
        LongPush = 0.0f;
        txtMessage.text = "";
        txtMessage.transform.localPosition = Vector3.zero;
        GameObject[] AllBalls = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject Stored in AllBalls) {
            Stored.SendMessage("Ready");
        }
        Physics.gravity = new Vector3( 0, -9.81f, 0 );
    }

    void ClearedAction() {
        txtMessage.text = "GOAL! " + Elapsed.ToString("f2") + "s.  Please touch to Ranking.";
        GameStatus = STS.CLEARED;
        int newRank = 0; //まず今回のタイムを0位と仮定する
        for (int idx = 5; idx > 0; idx--)
        { //逆順 5...1
            if (Rank[idx] > Elapsed)
            { // 不等号（＊）
                newRank = idx; // 新しいランクとして判定する
            }
        }
        if (newRank != 0)
        { // 0位のままでなかったらランクイン確定
            for (int idx = 5; idx > newRank; idx--)
            { // 不等号（＊）
                Rank[idx] = Rank[idx - 1]; // 繰り下げ処理
            }
            Rank[newRank] = Elapsed; // 新ランクに登録
            for (int idx = 1; idx <= 5; idx++)
            {
                PlayerPrefs.SetFloat("R" + idx, Rank[idx]); // データ領域に保存
            }
        }
    }

    void LongPushDetect() {
        if (Input.GetButton( "Fire1" )) {
            LongPush += Time.deltaTime; //画面を押し続けている間ずっと
            if (LongPush > 3.0f) {
                Ready();
            }
        }
        if (Input.GetButtonUp( "Fire1" )) {
            LongPush = 0.0f; //画面押しを離した
        }
    }

    void Update() {
        switch (GameStatus) {
            case STS.INIT:
                //開発用：データ領域の初期化
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    PlayerPrefs.DeleteAll();
                    Debug.Log("データ領域を削除しました。");
                }
                Elapsed += Time.deltaTime;
                Elapsed %= 1.0f;
                txtMessage.text = ( Elapsed < 0.8f ) ? "Touch to START" : "";
                if (Input.GetButtonDown( "Fire1" )) {
                    GameStatus = STS.PLAYING; //画面タッチを検出してゲーム開始
                    txtMessage.text = "Long push 3 sec to Title.";
                    txtMessage.transform.localPosition = new Vector3(0,-290,0);
                    GameObject.FindGameObjectWithTag( "Finish" ).SendMessage( "Ready" );
                    Elapsed = 0.0f;
                }
                break;
            case STS.PLAYING:
                Elapsed += Time.deltaTime;
                Vector3 Dir = new Vector3( 0.0f, -1.0f, 0.0f );
                if (Application.platform == RuntimePlatform.IPhonePlayer ||
                    Application.platform == RuntimePlatform.Android) {
                    Dir.x = Input.acceleration.x * Bias;
                    Dir.z = Input.acceleration.y * Bias;
                } else {
                    Dir.x = Input.GetAxis( "Horizontal" );
                    Dir.z = Input.GetAxis( "Vertical" );
                };
                Physics.gravity = 9.81f * Dir.normalized;
                LongPushDetect();
                break;
            case STS.CLEARED:
                //LongPushDetect();
                if (Input.GetButtonDown("Fire1"))
                {
                    SceneManager.LoadScene("Ranking");
                }
                break;
            default:
                break;
        }
    }
}
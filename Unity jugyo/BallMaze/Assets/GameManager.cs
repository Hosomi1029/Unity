using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;//リロードに必要

public class GameManager : MonoBehaviour
{
    public Text txtMessage;
    public Text txtNavi;
    public AudioSource play;
    public AudioSource clear;
    public enum STS
    {
        TITLE,  //タイトル画面
        PLAY,   //プレイ画面
        CLEAR   //クリア画面
    }
    STS GameStatus; //ゲームの状態
    float Elapsed; //経過時間
    float LongPush = 0.0f;
    int AllBalls; //ボールの総数
    // Start is called before the first frame update
    void Start()
    {
    
      //ボールの総数を求める
        AllBalls = GameObject.FindGameObjectsWithTag("Player").Length;
        GoalAction.Cnt = 0; //Goalへの侵入ボール数をゼロクリア
        GameStatus = STS.TITLE; //タイトル画面
        txtMessage.text = "Ball Maze";
        Elapsed = 0.0f; //ゼロクリア
        LongPush = 0.0f; //ゼロクリア
    }

    // Update is called once per frame
    void Update()
    {
        Elapsed += Time.deltaTime; //経過時間を加算
        switch (GameStatus)
        {
            case STS.TITLE: //タイトル画面
                Elapsed %= 1.0f;
                txtNavi.text = (Elapsed < 0.8f) ? "Long push 3sec to START" : "";
                if (Input.GetMouseButton(0))
                {
                    LongPush += Time.deltaTime;
                    //画面を３秒長押しでゲーム開始
                    if (LongPush > 3.0f)
                    {
                        GameStatus = STS.PLAY;
                        txtMessage.text = "";
                        txtNavi.text = "";
                        Elapsed = 0.0f; //プレイ時間初期化
                        LongPush = 0.0f; //ゼロクリア
                        play.Play();
                    }
                }
                else
                {
                    LongPush = 0.0f; //指を離した
                }

                break;
            case STS.PLAY: //プレイ画面
               
                Vector3 Dir = new Vector3(0, -1, 0);
                Dir.x = Input.GetAxis("Horizontal");
                Dir.z = Input.GetAxis("Vertical");
                Physics.gravity = 9.81f * Dir.normalized;
                if (GoalAction.Cnt >= AllBalls)
                {
                    Debug.Log("Cleared!");
                    GameStatus = STS.CLEAR;
                    txtMessage.text = "GOAL! " + Elapsed.ToString("f2") + "s";
                    Physics.gravity = new Vector3(0, -9.81f, 0); //重力リセット
                    LongPush = 0.0f; //長押しクリア
                    play.Stop();
                    clear.Play();
                }
                break;
            case STS.CLEAR: //クリア画面
                Elapsed %= 1.0f;
                txtNavi.text = (Elapsed < 0.8f) ? "Long push 3sec to TITLE" : "";
                if (Input.GetMouseButton(0))
                {
                    LongPush += Time.deltaTime;
                    //画面を３秒長押しで現在シーンをリロード
                    if (LongPush > 3.0f)
                    {
                        clear.Stop();
                        SceneManager.LoadScene(gameObject.scene.name);
                    }
                }
                else
                {
                    LongPush = 0.0f; //指を離した
                }
                break;
            default: //状態エラー
                break;
        }
    }
}

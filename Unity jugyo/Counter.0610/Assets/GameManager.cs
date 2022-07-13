using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//uGUIを利用する際に必要


public class GameManager : MonoBehaviour
{

    public Text txtCount;//画面のGUI文字
    //public  画面のUIに変化をつけられるようにする
    int Cnt;//カウント領域
            // Start is called before the first frame update
    public AudioClip SE_Plus; //加算サウンド
    public AudioClip SE_Minus; //減算サウンド
    AudioSource MyAudio; //自身の音源

    void Start()
    {
        MyAudio = GetComponent<AudioSource>();
        Debug.Log("Hello,Unity!");
        PushClear();
        //txtCount.text txtCountのtextを対象とする
        //ToString データ型を文字列盤に変換して送信
    }

    //クリアボタンを押した処理
    public void PushClear()
    {
        Cnt = 0; //カウント領域をゼロクリア
        DisplayCount(); //カウント表示
    }

    //プラスボタンを押した処理
    public void PushPlus()
    {
        Cnt++; //カウント領域を１加算
        DisplayCount(); //カウント表示
        MyAudio.PlayOneShot(SE_Plus); //加算サウンド鳴動
    }
    //マイナスボタンを押した処理
    public void PushMinus()
    {

        Cnt--;
        if (Cnt < 0)
        {
            Cnt = 0;
        }
        DisplayCount(); //カウント表示
        MyAudio.PlayOneShot(SE_Minus); //減算サウンド鳴動
    }

    void DisplayCount()
    {
        txtCount.text = Cnt.ToString().PadLeft(4, '0'); //画面に転記する
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}

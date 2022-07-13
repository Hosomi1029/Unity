using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; //ナビメッシュを利用するのに必要

public class PlayerAction : MonoBehaviour
{
    public GameObject patPower; //パワー状態の視覚効果
    Animator myAnim; //自身のアニメーター
    public NavMeshAgent myNav; //自身のナビメッシュ
    bool isDamage = false; //ゴーストと接触した状態か？の真偽値

    // Start is called before the first frame update
    void Start()
    {
        myNav = GetComponent<NavMeshAgent>();
        myAnim = GetComponent<Animator>();
        PowerDown(); //パワーダウン
    }

    //ゴースト接触処理
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ghost" && !isDamage)
        {
            //ゴーストのFollow値を元に勝ち負けを判定
            if (other.gameObject.GetComponent<GhostAction>().Follow)
            {
                isDamage = true;
                myAnim.SetTrigger("Down"); //ステートマシンにダウン通告
                myAnim.SetBool("Run", false); //走行をやめさせる
                myNav.enabled = false; //ナビメッシュを切る
                PowerDown(); //パワーダウン
                             //ゴースト接触を管理者に通告
                GameObject.Find("GameController").BroadcastMessage("HitGhost",
                SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                //100点プラス
                ValueHolder.score += 100;
                //ゴーストにダメージを通達
                other.gameObject.SendMessage("OnDamage",
                SendMessageOptions.DontRequireReceiver);
            }
        }

    }

    //パワーアップ
    void PowerUp()
    {
        patPower.SetActive(true); //視覚効果をオンにする
        Invoke("PowerDown", 15.0f); //パワーダウンを予約
        GameObject.Find("GameController").SendMessage("PowerUp", 15.0f,
        SendMessageOptions.DontRequireReceiver);
    }

    //パワーダウン
    void PowerDown()
    {
        patPower.SetActive(false); //視覚効果をオフにする
    }

    //ゲームクリア処理
    void GameClear()
    {
        myNav.enabled = false; //ナビメッシュを切って
        myAnim.SetTrigger("Clear"); //勝利のポーズ
        PowerDown(); //パワーダウン
    }

    // Update is called once per frame
    void Update()
    {
        if (myNav.enabled)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            //現在位置に長さ１のジョイスティック方向を加えた位置が目的地
            Vector3 NewPos = new Vector3(h, 0, v) + transform.position;
            if (myNav.enabled)
            {
                myNav.SetDestination(NewPos); //ナビメッシュが有効なら目的地へ
            }
            //ナビメッシュの速度を取得してアニメーターに指示する。
            myAnim.SetBool("Run", (myNav.velocity.magnitude > 0.01f) ? true : false);
        }
    }
}

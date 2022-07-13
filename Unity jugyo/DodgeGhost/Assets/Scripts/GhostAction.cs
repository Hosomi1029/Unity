using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; //ナビメッシュを利用するのに必要

public class GhostAction : MonoBehaviour
{
    GameObject Player; //プレイヤー
    NavMeshAgent myNavi; //自身のナビメッシュ
    Color myColor; //自身の元の色
    public bool Follow = true; //追いかけるモードかどうか

    // Start is called before the first frame update
    void Start()
    {
        myNavi = GetComponent<NavMeshAgent>(); //自身のナビメッシュを取得
        Player = GameObject.FindGameObjectWithTag("Player"); //プレイヤーを取得
        myColor = GetComponent<MeshRenderer>().material.GetColor("_in_Color");
    }

    void DontMove()
    {
        myNavi.enabled = false; //マネージャーからの指示でナビメッシュをオフ
    }

    //パワーアップを告げられ逃げる処理
    void PowerUp(float PowerTime)
    {
        Follow = false;
        GetComponent<MeshRenderer>().material.SetColor("_in_Color", Color.blue);
        Invoke("DoFollow", PowerTime); //指定秒数後には元通り追いかけ始める
    }

    //追いかける指示
    void DoFollow()
    {
        Follow = true;
        GetComponent<MeshRenderer>().material.SetColor("_in_Color", myColor);
    }

    //ダメージを受信
    void OnDamage()
    {
        myNavi.enabled = false;
        transform.position = new Vector3(0, 1, 1); //生誕の地へ移動
        DoFollow();//追いかける指示
        myNavi.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (myNavi.enabled)
        {
            if (Follow)
            {
                //行先はプレイヤーの座標
                myNavi.destination = Player.transform.position;
                myNavi.speed = 1.8f; //通常の速度
            }
            else
            {
                //行先はプレイヤーの正反対方向
                myNavi.destination = transform.position +
                (transform.position - Player.transform.position);
                myNavi.speed = 0.9f; //逃げ足は少し遅く
            }
        }
    }
}

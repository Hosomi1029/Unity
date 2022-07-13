using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // ナビメッシュを利用するのに必要

public class EnemyAction : MonoBehaviour {

    GameObject Player; // プレイヤー
    GameObject Manager; // マネージャー
    NavMeshAgent myNavi; // 自身のナビメッシュ
    Animator myAnim; // 自身のアニメーター

    void Start() {
        Player = GameObject.FindGameObjectWithTag( "Player" );
        Manager = GameObject.FindGameObjectWithTag( "GameController" );
        myNavi = GetComponent<NavMeshAgent>();
        myAnim = GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other) {
        //プレイヤーと接触したらゲームオーバー
        if (other.gameObject == Player) {
            Player.SendMessage( "OnHitEnemy",
                SendMessageOptions.DontRequireReceiver );
            Manager.SendMessage( "GameOver",
                SendMessageOptions.DontRequireReceiver );
            DontMove();
        }
    }

    void DontMove() {
        myNavi.enabled = false; //ナビメッシュをオフにする
        myAnim.SetFloat( "Speed", 0 );
    }

    void Update() {
        //３つの条件が揃えばキャラクターを追う指定をする
        if (Player && myAnim && myNavi.enabled) {
            myNavi.destination = Player.transform.position;
            //ナビメッシュの速度をアニメーターのSpeedに与える
            myAnim.SetFloat( "Speed", myNavi.velocity.magnitude );
        }
    }
}

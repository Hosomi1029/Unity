using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalAction : MonoBehaviour {

    int AllBall;     // シーン内のボールの総数
    int BallCnt;    // ゴールに侵入中のボール数
    bool Cleared;  // クリアしたフラグ

    void Awake() {
        // ボールの総数を特定
        AllBall = GameObject.FindGameObjectsWithTag("Player").Length;
    }

    void Ready() {
        Cleared = false;
        BallCnt = 0;
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            BallCnt++;
            if (!Cleared && BallCnt == AllBall) {
                Cleared = true;   //ゲームクリア成立
                // ゲームクリアを報告する
                GameObject.Find("Maze").SendMessage("ClearedAction");
            }
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") {
            BallCnt--;
        }
    }


}

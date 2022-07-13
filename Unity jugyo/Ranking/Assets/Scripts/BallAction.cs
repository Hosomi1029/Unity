using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallAction : MonoBehaviour {
    Rigidbody myRB; // 自身のリジッドボディを代入する入れ物
    Vector3 InitPos;

    void Awake() {
        myRB = GetComponent<Rigidbody>(); // リジッドボディに命令したいので取得する
        InitPos = transform.position;
    }

    void Ready() {
        myRB.velocity = Vector3.zero;
        myRB.Sleep();
        transform.rotation = Quaternion.identity;
        transform.position = InitPos;
    }

    void Update() {
        myRB.WakeUp();
    }

}

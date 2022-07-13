using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAction : MonoBehaviour {

    public float DelayLevel = 3.0f; //遅延する割合
    GameObject P; //プレイヤー
    public Vector3 OffSet = new Vector3( 0, 1.4f, 0 ); //ターゲットの少し上
    public Vector3 CamDir = new Vector3( 0, 4, -5.5f ); //ターゲットから見たカメラ方向

    void Start() {
        P = GameObject.FindGameObjectWithTag( "Player" );
    }

    void Update() {
        if (!P) {
            return; //プレイヤー不在なら動かさない。
        }
        //カメラの位置。プレイヤー位置から算出する。
        Vector3 NewDir = P.transform.TransformDirection( CamDir );
        Vector3 NewPos = Vector3.Slerp(
             transform.position, //現状のカメラ位置
             P.transform.position + NewDir, //行きたいカメラ位置
             Time.deltaTime * DelayLevel ); //その差の割合（0～1）
        transform.position = NewPos;


        //カメラの回転。注視点はプレイヤーの少し上を見る。
        transform.LookAt( P.transform.position + OffSet );
    }

}

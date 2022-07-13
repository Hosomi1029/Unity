using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    float h; // 水平軸
    float v; // 垂直軸
    Vector3 Dir; //移動方向
    Animator myAnim; //自身のアニメーター
    Rigidbody myRB; //自身のリジッドボディ（物理挙動）

    void Start() {
        myRB = GetComponent<Rigidbody>(); //自身のリジッドボディを取得
        Ready();
    }

    void Ready() {
        transform.position = Vector3.zero; //位置をリセット
        transform.rotation = Quaternion.identity; //回転向きをリセット
        enabled = true;
        myAnim = GetComponent<Animator>(); //自身のアニメーターを取得
        myAnim.SetTrigger( "Ready" );
    }

    bool canJump = true;

    IEnumerator JumpMotion() {
        canJump = false;
        myAnim.SetTrigger( "Jump" );
        // ここで0.27秒を待機しないと、しゃがむ前に離陸します。
        yield return new WaitForSeconds( 0.27f );
        myRB.AddForce( new Vector3( 0, 4.2f, 0 ), ForceMode.Impulse );
        canJump = true;
    }

    public LayerMask GroundLayer; //指定レイヤー

    // 接地をブール値で返す関数
    bool IsGround() {
        RaycastHit hit;
        // 下向きに球を投げて指定レイヤーと当たるか？
        return Physics.SphereCast( transform.position + Vector3.up * 0.5f,
            0.02f, -Vector3.up, out hit, 0.52f, GroundLayer );
    }

    void DontMove() {
        enabled = false;
        myAnim.SetFloat( "Speed", 0 );
        myAnim.SetFloat( "Direction", 0 );
    }

    void OnHitEnemy() {
        myAnim.SetTrigger( "Down" );
        DontMove();
    }


    void Update() {

        if (GameManager.GameStatus != GameManager.STS.PLAY) {
            //ゲームの状態がプレイ中で無ければ操作不可能にする
            return;
        }

        if (Input.GetButtonDown( "Fire1" ) && IsGround() && canJump) {
            StartCoroutine( "JumpMotion" ); // ジャンプのコルーチン処理を起動
        }

        h = Input.GetAxis( "Horizontal" ); //入力デバイスの水平軸
        v = Input.GetAxis( "Vertical" ); //入力デバイスの垂直軸
        myAnim.SetFloat( "Speed", v );
        myAnim.SetFloat( "Direction", h );
        Dir = new Vector3( 0, 0, v );
        Dir = transform.TransformDirection( Dir );
        Dir *= ( v > 0.1f ) ? 3.5f : 0.8f;
        transform.localPosition += Dir * Time.deltaTime; // キャラクターを移動
        transform.Rotate( 0, h * 90.0f * Time.deltaTime, 0 ); // キャラクターを回転
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAction : MonoBehaviour
{
    public GameObject Bullet; //弾丸のプレハブ
    public AudioClip SE_Fire; //射出音
    public AudioClip SE_Over; //タイムアップ音
    AudioSource myAudio;
    public float Speed = 40.0f; //初速度
 public Vector3 screenPoint; //スクリーン座標
 public Vector3 worldPoint; //ワールド座標

    // Start is called before the first frame update
    void Start()
    {
        myAudio = GetComponent<AudioSource>();
        myAudio.Play(); //BGMを鳴動
    }

    void GameStart()
    {
        enabled = true; //スクリプトを稼働させる
    }

    void TimeUp()
    {
        enabled = false; //スクリプトを停止
        myAudio.Stop(); //BGMを停止
        myAudio.PlayOneShot(SE_Over); //タイムアップ鳴動
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //クリック位置の座標
            screenPoint = Input.mousePosition; //XとYはスクリーン座標
            screenPoint.z = 9.0f; //Zはワールド座標で、カメラからの距離
                                  //クリック座標をカメラから見たUnity空間での座標worldPointに変換する
            worldPoint = GetComponent<Camera>().ScreenToWorldPoint(screenPoint);
            Debug.DrawLine(transform.position, worldPoint, Color.green);
            //その座標とカメラ位置との２つで向きを求める
            Vector3 Dir = (worldPoint - transform.position).normalized;
            //カメラ位置に弾丸をインスタンス生成しつつ、Bへ代入する
            GameObject B = Instantiate(Bullet, transform.position, transform.rotation) as GameObject;
            B.GetComponent<Rigidbody>().velocity = Dir * Speed; //弾丸に初速度を与える
            myAudio.PlayOneShot(SE_Fire); //射出音鳴動        
        }
    }
}

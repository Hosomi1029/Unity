using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAction : MonoBehaviour
{
    GameObject Player; //プレイヤー
    Vector3 OffSet = new Vector3(0, 10, -3); //離れる距離

    // Start is called before the first frame update
    void Start()
    {
        //プレイヤーを取得
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Player)
        {
            //プレイヤー取得がＯＫなら、指定距離だけ離れた位置へ移動する
            transform.position = Player.transform.position + OffSet;
        }
    }
}

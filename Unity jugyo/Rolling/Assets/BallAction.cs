using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallAction : MonoBehaviour
{
    Rigidbody MyRB;  //自身のRigidbody

    // Start is called before the first frame update
    void Start()
    {
        MyRB = GetComponent<Rigidbody>();//自身のRigidbodyを取得
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal"); //左右キー方向の値（-1～0～1）
        float v = Input.GetAxis("Vertical"); //上下キー方向の値（-1～0～1）
        Vector3 Dir = new Vector3(h, 0, v); //方向ベクトルを作る
        MyRB.AddForce(Dir * 4); //力を加えている
    }
}

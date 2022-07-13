using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityAction : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 G = new Vector3(0, -1, 0); //鉛直下方向ベクトルを作る
        G.x = Input.GetAxis("Horizontal"); //左右キー方向の値（-1～0～1）で修正
        G.z = Input.GetAxis("Vertical"); //上下キー方向の値（-1～0～1）で修正
        Physics.gravity = 9.81f * G.normalized; //物理演算の重力方向に渡す
    }
}

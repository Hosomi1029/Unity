using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallAction : MonoBehaviour
{
    int HitCnt = 0; //ヒット回数
    void OnCollisionEnter(Collision other)
    {
        HitCnt++; //１加算
        Debug.Log("Hit_" + HitCnt + "/" + other.gameObject.name) ;
    }



        // Start is called before the first frame update
        void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

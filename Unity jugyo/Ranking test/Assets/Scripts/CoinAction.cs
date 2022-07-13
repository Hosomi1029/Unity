using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinAction : MonoBehaviour {

    GameObject Manager; //マネージャー
    
    void Start() {
        Manager = GameObject.FindGameObjectWithTag( "GameController" );
        Destroy( gameObject, 10.0f ); //自身を10秒で撤去
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            //自身の名前の５文字目から3文字を整数に変換してPriceに代入
            int Price = int.Parse( gameObject.name.Substring( 4, 3 ) );
            Manager.SendMessage( "ChangeScore", Price,
                SendMessageOptions.DontRequireReceiver );
            Destroy( gameObject );
        }
    }

    void Update() {
        //自身を回す
        transform.Rotate( Vector3.up * Time.deltaTime * 90.0f );
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAction : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Box")
        {
            //相手が箱だったらOnDamageをメッセージ通告する
            other.gameObject.SendMessage("OnDamage", SendMessageOptions.DontRequireReceiver);
        }
        Destroy(gameObject); //何かに当たれば自身（弾丸）を撤去
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

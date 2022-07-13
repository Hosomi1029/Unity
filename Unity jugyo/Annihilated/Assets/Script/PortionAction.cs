using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortionAction : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            //プレイヤーにダメージを送信
            other.gameObject.SendMessage("Heal",
            SendMessageOptions.DontRequireReceiver);

            Destroy(gameObject);
        }

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * 90.0f);
    }
}

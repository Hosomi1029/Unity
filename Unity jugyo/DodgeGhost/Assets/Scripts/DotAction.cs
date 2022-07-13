using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotAction : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (gameObject.name.Substring(0, 5) == "Power")
            {
                other.gameObject.SendMessage("PowerUp",
                SendMessageOptions.DontRequireReceiver);
                ValueHolder.score += 50; //加点
            }
            else
            {
                ValueHolder.score += 10; //加点
            }
            Destroy(gameObject); //自身を破棄
        }
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

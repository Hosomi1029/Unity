using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalAction : MonoBehaviour
{
    static public int Cnt; //自身（Goal）に侵入しているボール数
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Cnt++;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Cnt--;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

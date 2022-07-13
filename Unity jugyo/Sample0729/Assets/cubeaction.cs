using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cubeaction : MonoBehaviour
{
    float Elapsed = 0.0f;
    public Text txtBlink;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Elapsed += Time.deltaTime;
        Elapsed %= 2.0f;

        if (Elapsed < 1.6f)
        {
            txtBlink.text = "aaaaaaaaaaaaaa";
        }
        else
        {
            txtBlink.text = "";
        }
        transform.position = new Vector3(
            Mathf.Cos(Time.time* Mathf.PI)*3,
            Mathf.Sin(Time.time* Mathf.PI)*2, 0);
    }
}

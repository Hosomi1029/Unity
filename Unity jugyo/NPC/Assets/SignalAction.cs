using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; //ナビメッシュを使うのに必要

public class SignalAction : MonoBehaviour
{
    public GameObject signalY;
    public GameObject signalR;
    public GameObject signalG;
    int signalStatus;
    float Elapsed = 0.0f;
    Color ColorR = new Color(1, 0, 0, 1.0f);
    Color ColorG = new Color(0, 1, 0, 1.0f);
    Color ColorY = new Color(0.8f, 0.8f, 0, 1.0f);
    Color ColorN = new Color(0.5f, 0.5f, 0.5f, 1.0f);

    // Start is called before the first frame update
    void Start()
    {
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerAction>())
        {
            if (signalStatus == 2)
            {
                other.GetComponent<NavMeshAgent>().enabled = false;
            }
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<PlayerAction>())
        {
            if (signalStatus != 2)
            {
                other.GetComponent<NavMeshAgent>().enabled = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Elapsed += Time.deltaTime;
        Elapsed %= 18.0f;
        if (Elapsed < 8.0f)
        {
            signalStatus = 0;
            signalR.GetComponent<Renderer>().material.color = ColorN;
            signalG.GetComponent<Renderer>().material.color = ColorG;
            signalY.GetComponent<Renderer>().material.color = ColorN;
        }
        else if (Elapsed < 10.0f)
        {
            signalStatus = 1;
            signalR.GetComponent<Renderer>().material.color = ColorN;
            signalG.GetComponent<Renderer>().material.color = ColorN;
            signalY.GetComponent<Renderer>().material.color = ColorY;
        }
        else
        {
            signalStatus = 2;
            signalR.GetComponent<Renderer>().material.color = ColorR;
            signalG.GetComponent<Renderer>().material.color = ColorN;
            signalY.GetComponent<Renderer>().material.color = ColorN;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; //ナビメッシュを使うのに必要

public class DoorAction : MonoBehaviour
{
    public GameObject Door;
    bool isOpen = false;
    public float PosClose = 3.0f;
    public float PosOpen = 4.6f;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerAction>())
        {
            isOpen = true;
            other.gameObject.GetComponent<NavMeshAgent>().enabled = false;
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (Door.transform.position.z >= PosOpen)
        {
            other.gameObject.GetComponent<NavMeshAgent>().enabled = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerAction>())
        {
            isOpen = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpen && Door.transform.position.z <= PosOpen)
        {
            Door.transform.position += new Vector3(0, 0, 3 * Time.deltaTime);
        }
        if (!isOpen && Door.transform.position.z >= PosClose)
        {
            Door.transform.position -= new Vector3(0, 0, 3 * Time.deltaTime);
        }
    }
}

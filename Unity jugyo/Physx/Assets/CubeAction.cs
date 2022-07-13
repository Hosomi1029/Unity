using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeAction : MonoBehaviour
{
    GameObject Target;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enter");
        
        other.gameObject.transform.position = Target.transform.position; 
    }
    void OnTriggerStay(Collider other)
    {
        Debug.Log("Stay");
    }
    void OnTriggerExit(Collider other)
    {
        Debug.Log("Exit");
    }

    // Start is called before the first frame update
    void Start()
    {
        Target = GameObject.FindGameObjectWithTag("Respawn");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

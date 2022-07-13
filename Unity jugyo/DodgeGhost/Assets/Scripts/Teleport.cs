using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Teleport : MonoBehaviour
{

    public Teleport tp;
    public GameObject Player;
    NavMeshAgent PlayerNav;
    public bool TPcan;
    Vector3 trans;

    // Start is called before the first frame update
    void Start()
    {
        PlayerNav = Player.GetComponent<NavMeshAgent>();
        trans = tp.transform.position;
        TPcan = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (TPcan) {
                tp.TPcan = false;
                PlayerNav.enabled = false;
                other.gameObject.transform.position = trans;
                PlayerNav.enabled = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(!TPcan) TPcan = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

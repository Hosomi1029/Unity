using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapAction : MonoBehaviour {

    public Vector3 RespawnPos = new Vector3(-8, 0.5f, -8);

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            other.gameObject.transform.position = RespawnPos;
        }
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraAction : MonoBehaviour
{
    GameObject Player;
    Vector3 CamPos = new Vector3(0.0f, 3.0f, -2.5f);
    Vector3 OffSet = new Vector3(0.0f, 1.5f, 0.0f);
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }
    void LateUpdate()
    {
        transform.position = Player.transform.position + CamPos;
        transform.LookAt(Player.transform.position + OffSet);
    }
}
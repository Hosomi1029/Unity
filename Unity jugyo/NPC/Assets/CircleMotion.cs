using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMotion : MonoBehaviour
{
    Vector3 InitPos;
    // Start is called before the first frame update
    void Start()
    {
        InitPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(
         InitPos.x + Mathf.Cos(Time.time), 0,
         InitPos.z + Mathf.Sin(Time.time));
    }
}

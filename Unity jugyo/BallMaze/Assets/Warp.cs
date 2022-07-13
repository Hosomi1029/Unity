using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MonoBehaviour
{
    public Vector3 WarpPosition = new Vector3(-8, 0.5f, -8);    // Start is called before the first frame update
    void Start()
    {
        
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.transform.position = WarpPosition;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

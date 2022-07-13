using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public float dx;
    public AudioSource gate;
    public GameObject warp;

    // Start is called before the first frame update
    void Start()
    {
        gate = GetComponent<AudioSource>();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Key.Cnt == 1 && Key2.Cnt == 1 && Key3.Cnt == 1)
        {
            if(this.gameObject.transform.position.x<=-2)
            {
                gate.Play();
                this.gameObject.transform.position += new Vector3(dx, 0, 0);
                Destroy(warp);
            }
        }
    }
}

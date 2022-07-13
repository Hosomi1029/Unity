using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key3 : MonoBehaviour
{
    static public int Cnt;
    public AudioSource open;
    public AudioSource close;

    // Start is called before the first frame update
    void Start()
    {
        //open = GetComponent<AudioSource>();
        //close = GetComponent<AudioSource>();

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            open.Play();
            Cnt++;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            close.Play();
            Cnt--;
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}

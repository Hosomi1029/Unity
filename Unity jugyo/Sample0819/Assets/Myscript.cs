using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Myscript : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        int Dice = Random.Range(1, 7);
        Debug.Log(Dice);
        for (int i = 1; i <= 20; i++)
        {
            float Ren = Random.Range(-3.5f, 3.5f);
            Debug.Log(Ren);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

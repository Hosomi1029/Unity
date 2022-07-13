using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class BallAction : MonoBehaviour
{
    float Elapsed = 0.0f;

   
    // Start is called before the first frame update
    void Start()
    {
        ChangeColor();
    }
    void ChangeColor()
    {
        float R = Random.value;      //小数型の変数Rに0.0～1.0までの乱数を代入 
        float G = Random.value; //小数型の変数Gに0.0～1.0までの乱数を代入 
        float B = Random.value; //小数型の変数Bに0.0～1.0までの乱数を代入 
        GetComponent<Renderer>().material.color = new Color(R, G, B);

    }
    // Update is called once per frame
    void Update()
    {
        Elapsed += Time.deltaTime;
        if(Elapsed > 3.0f)
        {
            Elapsed = 0;

            ChangeColor();

        }
        if (Input.GetMouseButtonDown(0))
        {
            ChangeColor();
        }

    }
}

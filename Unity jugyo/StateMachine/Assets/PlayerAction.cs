using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    static public bool CanJump;
    Animator myAnim; //自身のアニメーター

    // Start is called before the first frame update
    void Start()
    {
        CanJump = true;
        //自身のアニメータ―を取得
        myAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float zengo;
        zengo = Input.GetAxis("Vertical");
        myAnim.SetFloat("Speed", zengo);

        if(Input.GetButtonDown("BtnA") && CanJump)
        {
            myAnim.SetTrigger("Jump");
        }

        if (Input.GetAxis("TriggerR") < 0.7f)
        {
            myAnim.SetBool("Run", false);
        }
        else
        {
            myAnim.SetBool("Run", true);
            if (Input.GetButtonDown("BumperL"))
            {
                myAnim.SetTrigger("Slide");
            }
        }
    }
}

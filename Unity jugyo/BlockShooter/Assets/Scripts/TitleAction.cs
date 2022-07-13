using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //uGUIを用いるのに必要
using UnityEngine.SceneManagement; //シーン切り替えに必要

public class TitleAction : MonoBehaviour
{
    float LongPush = 0.0f;
    float Elapsed = 0.0f;
    public Text txtNavigate;
    public Image imgFill;
    void Update()
    {
        imgFill.fillAmount = LongPush / 3.0f;
        //メッセージの点滅運営
        Elapsed += Time.deltaTime;
        Elapsed %= 1.0f;
        txtNavigate.text = (Elapsed < 0.8f) ? "Long push 3 sec to START" : "";
        //長押し3秒の検出
        if (Input.GetMouseButton(0))
        {
            LongPush += Time.deltaTime;
            if (LongPush > 3.0f)
            {
                SceneManager.LoadScene("Main");
            }
        }
        else
        {
            LongPush = 0.0f;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; //シーンのロードに必要

public class TItleAction : MonoBehaviour
{
    public Text txtNavi;
    float Elapsed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Elapsed += Time.deltaTime;
        Elapsed %= 1.0f;
        txtNavi.gameObject.SetActive(Elapsed < 0.8f);

        if(Input.GetButtonDown("BtnA"))
        {
            SceneManager.LoadScene("Main");
        }
    }
}

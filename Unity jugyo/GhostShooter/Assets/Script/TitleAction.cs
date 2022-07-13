using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //uGUIの利用に必要
using UnityEngine.SceneManagement; //シーンのロードに必要

public class TitleAction : MonoBehaviour
{
    public Image imgFill;
    public Text[] txtRank;
    float Elapsed;

    // Start is called before the first frame update
    void Start()
    {
        imgFill.fillAmount = 0;
        Elapsed = 0.0f;
        for (int idx = 1; idx <= 5; idx++)
        {
            txtRank[idx - 1].text = "_.__s"; //全ランク表示を初期化
        }
        if (PlayerPrefs.HasKey("R1"))
        {
            //アプリのデータ領域を取得して画面に転記
            for (int idx = 1; idx <= 5; idx++)
            {
                if (PlayerPrefs.GetFloat("R" + idx) < float.MaxValue)
                {
                    txtRank[idx - 1].text = PlayerPrefs.GetFloat("R" + idx).ToString("f2") + "s";
                }
            }
        }
        else
        {
            //アプリのデータ領域を最大値で初期化
            for (int idx = 1; idx <= 5; idx++)
            {
                PlayerPrefs.SetFloat("R" + idx, float.MaxValue);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Esc押下でアプリのデータ領域を強制初期化
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PlayerPrefs.DeleteAll();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        //Ａボタンを押下中は経過時間を加算
        if (Input.GetButton("BtnA"))
        {
            Elapsed += Time.deltaTime;
            if (Elapsed > 2.0f)
            {
                SceneManager.LoadScene("Main");
            }
            imgFill.fillAmount = Elapsed / 2.0f;
        }
        //Ａボタンを離すと最初から
        if (Input.GetButtonUp("BtnA"))
        {
            imgFill.fillAmount = 0;
            Elapsed = 0.0f;
        }
    }
}

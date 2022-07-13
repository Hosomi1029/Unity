using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //uGUIを利用するのに必要
using UnityEngine.SceneManagement; //シーンの移動に必要

public class TitleAction : MonoBehaviour
{
    public Text txtNavigate;
    public GameObject Ghost; //ゴースト
    Vector3 StartPos; //ゴーストの開始位置
    float Elapsed = 0;

    // Start is called before the first frame update
    void Start()
    {
        // ゴーストの開始位置を退避
        StartPos = Ghost.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //メッセージのブリンク制御
        Elapsed += Time.deltaTime;
        Elapsed %= 1.0f;
        txtNavigate.gameObject.SetActive(Elapsed < 0.8f);
        //Ａボタン押下でゲームへ
        if (Input.GetButtonDown("Fire1"))
        {
            SceneManager.LoadScene("Main");
        }
        //ゴーストを円運動させる
        Vector3 Pos = Vector3.zero;
        Pos.z = Mathf.Cos(Time.time * 2);
        Pos.y = Mathf.Sin(Time.time * 5);
        Ghost.transform.position = StartPos + Pos;
    }
}

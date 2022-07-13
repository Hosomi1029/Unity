using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //uGUIを用いるのに必要
using XInputDotNetPure; // XInputDotNetの利用

public class MainAction : MonoBehaviour
{
    public Text txtMsg;
    string EscText; //初期メッセージの退避エリア
    public Image imgJoyL;
    public Text txtJoyLX;
    public Text txtJoyLY;
    Vector2 PosJoyL; //左スティックの初期位置

    public Image imgJoyR;
    public Text txtJoyRX;
    public Text txtJoyRY;
    Vector2 PosJoyR; //右スティックの初期位置

    public Image imgTrigL;
    public Image imgTrigR;
    public Text txtTrigL;
    public Text txtTrigR;

    public Image imgPad;
    public Text txtPadX;
    public Text txtPadY;
    Vector2 PosPad; //方向パッドの初期位置

    float Elapsed = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        //初期メッセージの退避
        EscText = txtMsg.text;
        //左スティックの初期位置を退避
        PosJoyL = new Vector2(
        imgJoyL.rectTransform.position.x,
        imgJoyL.rectTransform.position.y);

        //右スティックの初期位置を退避
        PosJoyR = new Vector2(
        imgJoyR.rectTransform.position.x,
        imgJoyR.rectTransform.position.y);

        //方向パッドの初期位置を退避
        PosPad = new Vector2(
        imgPad.rectTransform.position.x,
        imgPad.rectTransform.position.y);
    }

    
    // Update is called once per frame
    void Update()
    {
        //左スティック
        float LX = Input.GetAxis("Horizontal");
        float LY = Input.GetAxis("Vertical");
        txtJoyLX.text = LX.ToString("f2");
        txtJoyLY.text = LY.ToString("f2");
        imgJoyL.rectTransform.position = PosJoyL + new Vector2(LX, LY);

        //右スティック
        float RX = Input.GetAxis("HorizontalR");
        float RY = Input.GetAxis("VerticalR");
        txtJoyRX.text = RX.ToString("f2");
        txtJoyRY.text = RY.ToString("f2");
        imgJoyR.rectTransform.position = PosJoyR + new Vector2(RX, RY);

        // 左右トリガー
        txtTrigL.text = Input.GetAxis("TrigL").ToString("f2");
        txtTrigR.text = Input.GetAxis("TrigR").ToString("f2");
        imgTrigL.rectTransform.rotation = new Quaternion(0, 0, Input.GetAxis("TrigL") * -0.3f, 1);
        imgTrigR.rectTransform.rotation = new Quaternion(0, 0, Input.GetAxis("TrigR") * 0.3f, 1);

        // 方向パッド
        float PX = Input.GetAxis("PadX");
        float PY = Input.GetAxis("PadY");
        txtPadX.text = PX.ToString("f2");
        txtPadY.text = PY.ToString("f2");
        imgPad.rectTransform.position = PosPad + new Vector2(PX, PY);

        Elapsed += Time.deltaTime;
        if (Elapsed < 0.3f)
        {
            //バイブレーション機能１
            GamePad.SetVibration((PlayerIndex)0, 1, 1);
        }
        else
        {
            //バイブレーション機能２
            GamePad.SetVibration((PlayerIndex)0,
            Input.GetAxis("TrigL"),
            Input.GetAxis("TrigR"));
        }

        if (Input.GetButton("btnA"))
        {
            txtMsg.text = "A";
        }
        else if (Input.GetButton("btnB"))
        {
            txtMsg.text = "B";
        }
        else if (Input.GetButton("btnX"))
        {
            txtMsg.text = "X";
        }
        else if (Input.GetButton("btnY"))
        {
            txtMsg.text = "Y";
        }
        else if (Input.GetButton("btnL"))
        {
            txtMsg.text = "L";
        }
        else if (Input.GetButton("btnR"))
        {
            txtMsg.text = "R";
        }
        else if (Input.GetButton("btnBack"))
        {
            txtMsg.text = "Back";
        }
        else if (Input.GetButton("btnStart"))
        {
            txtMsg.text = "Start";
        }
        else if (Input.GetButton("btnJoyL"))
        {
            txtMsg.text = "JoyL";
        }
        else if (Input.GetButton("btnJoyR"))
        {
            txtMsg.text = "JoyR";
        }

        else
        {
            txtMsg.text = EscText;
        }
    }
}

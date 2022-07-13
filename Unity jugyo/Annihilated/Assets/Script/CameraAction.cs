using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAction : MonoBehaviour
{
    GameObject Player; //プレイヤー
    private const float YAngle_MIN = -89.0f;   //カメラのY方向の最小角度
    private const float YAngle_MAX = 7.0f;     //カメラのY方向の最大角度
    private float currentX = 0.0f;  //カメラをX方向に回転させる角度
    private float currentY = 0.0f;  //カメラをY方向に回転させる角度
    public Vector3 CamDir = new Vector3(0, 10, -15); //カメラの方向
    public Vector3 LookPos = new Vector3(0, 1.5f, 0); //注視点
    public float camsp;
    private Vector3 lookAt;
    public float Nearest = 2.5f; //撮影の最短距離
    private float distance = 10.0f;    //キャラクターとカメラ間の角度
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player"); //プレイヤーを取得
    }


    void Update()
    {
        currentX += Input.GetAxis("HorizontalR");
        currentY += Input.GetAxis("VerticalR");
        currentY = Mathf.Clamp(currentY, YAngle_MIN, YAngle_MAX);
        //CamDir = new Vector3(currentY, currentX);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //カメラの新座標をプレイヤー位置から算出する。
        //Vector3 NewDir = Player.transform.TransformDirection(CamDir);
        //Vector3 NewPos = Vector3.Lerp(
        //transform.position, //現状のカメラ位置
        //Player.transform.position + NewDir, //行きたいカメラ位置
        //Time.fixedDeltaTime * 3.0f); //その差の割合（0～1）
        ////注視点から新座標までの途中に何か当たるか検査
        //Ray CamRay = new Ray(Player.transform.position, NewDir);
        //RaycastHit hitInfo;
        //if (Physics.Raycast(CamRay, out hitInfo, NewDir.magnitude))
        //{
        //    if (hitInfo.distance > Nearest)
        //    {
        //        //カメラは当たった壁の少し手前へ
        //        transform.position = hitInfo.point - NewDir.normalized * 0.2f;
        //    }
        //    else
        //    {
        //        //カメラは最短距離へ
        //        transform.position = Player.transform.position + NewDir.normalized * Nearest;
        //    }
        //}
        //else
        //{
        //    transform.position = NewPos; //カメラは新座標へ
        //}
        lookAt = Player.transform.position + LookPos;

        //カメラ旋回処理
        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(-currentY, currentX, 0);

        transform.position = lookAt + rotation * dir ;   //カメラの位置を変更
        transform.LookAt(lookAt);   //カメラをLookAtの方向に向けさせる
        //カメラを注視点向きに回転
    }
}

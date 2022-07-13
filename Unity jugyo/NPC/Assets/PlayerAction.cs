using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAction : MonoBehaviour
{
    NavMeshAgent myAgent; //自身のナビメッシュ
    Animator myAnim; //自身のアニメーターコントローラー
    Camera Cam; //カメラ
    int TargetID = 0;
    public GameObject PoleBlue;
    public GameObject PoleRed;
    public GameObject PoleGreen;

    // Start is called before the first frame update
    void Start()
    {
        //カメラオブジェクトを探してくる
        GameObject CamObj = GameObject.FindGameObjectWithTag("MainCamera");
        //カメラコンポーネントを取得
        Cam = CamObj.GetComponent<Camera>();
        //アニメーターコンポーネントを取得
        myAnim = GetComponent<Animator>();
        //ナビメッシュコンポーネントを取得
        myAgent = GetComponent<NavMeshAgent>();
        //仮の行き先座標
        myAgent.SetDestination(new Vector3(4, 0, 4));
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Substring(0, 4) == "Pole")
        {
            TargetID++; //目的地IDを変更
            TargetID %= 3;
            if (other.gameObject.name == "PoleGreen")
            {
                myAnim.SetTrigger("Jump");
                myAgent.speed = 0.7f;
                Invoke("SpeedUp", 1.6f);
            }
        }
    }

    void SpeedUp()
    {
        myAgent.speed = 3.5f;
    }

    // Update is called once per frame
    void Update()
    {
        //ナビメッシュが有効なら左クリック位置へ向かわせる
        if (Input.GetMouseButtonDown(0) && myAgent.enabled)
        {
            RaycastHit hitInfo;
            Ray screenRay = Cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(screenRay, out hitInfo))
            {
                myAgent.SetDestination(hitInfo.point);
            }
        } 
            
            //ナビメッシュの移動速度をアニメーターのSpeedへ送る
            if (myAgent.enabled)
        {
            //目的地を切り替える
            if (TargetID == 0)
            {
                myAgent.SetDestination(PoleRed.transform.position);
            }
            else if (TargetID == 1)
            {
                myAgent.SetDestination(PoleBlue.transform.position);
            }
            else if (TargetID == 2)
            {
                myAgent.SetDestination(PoleGreen.transform.position);
            }
            myAnim.SetFloat("Speed", myAgent.velocity.magnitude);
        }
        else
        {
            myAnim.SetFloat("Speed", 0);
        }
    }
}

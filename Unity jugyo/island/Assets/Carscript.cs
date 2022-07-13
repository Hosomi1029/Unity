using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Carscript : MonoBehaviour
{
    NavMeshAgent myAgent; //自身のナビメッシュ
    int TargetID = 0;
    public GameObject Pole1;
    public GameObject Pole2;
    public GameObject Pole3;
    public GameObject Pole4;
    public GameObject Pole5;

    // Start is called before the first frame update
    void Start()
    {
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
            TargetID %= 4;
        }
    }


// Update is called once per frame
void Update()
    {
        //目的地を切り替える
        if (TargetID == 0)
        {
            myAgent.SetDestination(Pole1.transform.position);
        }
        else if (TargetID == 1)
        {
            myAgent.SetDestination(Pole2.transform.position);
        }
        else if (TargetID == 2)
        {
            myAgent.SetDestination(Pole3.transform.position);
        }
        else if (TargetID == 3)
        {
            myAgent.SetDestination(Pole4.transform.position);
        }
        else if (TargetID == 4)
        {
            myAgent.SetDestination(Pole5.transform.position);
        }

    }
}

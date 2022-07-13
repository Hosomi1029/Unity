using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; //NavMeshの利用に必要

public class GhostAction : MonoBehaviour
{
    NavMeshAgent myNavi; //自身のナビメッシュ
    Vector3 myDestination; //自身の終着地
    int PatternID; //自身の行動パターン
    bool isDead = false; //自身の死亡フラグ
    public GameObject EffectPrefab; //エフェクトプレハブ
    GameObject Manager; //マネージャー
    GameObject Player; //プレイヤー
    Renderer myRenderer; //自身のレンダラー
    public float AngryDistance = 15.0f; //怒る距離
    public Material angryMat; //怒ったマテリアル
    public Material normalMat; //元々のマテリアル

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //プレイヤーにダメージを送信
            other.gameObject.SendMessage("OnDamage",
            SendMessageOptions.DontRequireReceiver);
        }
        if (other.gameObject.tag == "Bullet" && myNavi.enabled && !isDead)
        {
            isDead = true;
            //撃たれたことをマネージャーに通達
            Manager.SendMessage("OnHitBullet",
            SendMessageOptions.DontRequireReceiver);
            //死亡エフェクトを設置
            GameObject Fx = Instantiate(EffectPrefab,
            transform.position, Quaternion.identity) as GameObject;
            Destroy(Fx, 1.0f); //エフェクトの寿命は1秒
            Destroy(gameObject); //自身（ゴースト）を撤去
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //プレイヤーを取得
        Player = GameObject.FindGameObjectWithTag("Player");
        //自身のレンダラーを保管
        myRenderer = GetComponent<Renderer>();
        //マネージャーを取得
        Manager = GameObject.FindGameObjectWithTag("GameController");
        //自身のナビメッシュを取得
        myNavi = GetComponent<NavMeshAgent>();
        //行動パターンIDを乱数で決定
        PatternID = Random.Range(0, 4);
        //自身の出発地を算出 ***********************
        Vector3 Pos = new Vector3(0, 1.5f, 0); //高さ1.5固定
        if (PatternID % 2 == 0)
        {
            Pos.x = Random.Range(-34.0f, 34.0f);
            Pos.z = (PatternID < 2) ? 22 : -22;
        }
        else
        {
            Pos.x = (PatternID < 2) ? 34 : -34;
            Pos.z = Random.Range(-22.0f, 22.0f);
        }
        //自身を出発地へ移動 **********************
        myNavi.enabled = false;
        transform.position = Pos;
        myNavi.enabled = true;
        //自身の終着地を算出 **********************
        if (PatternID % 2 == 0)
        {
            Pos.x = Random.Range(-34.0f, 34.0f);
            Pos.z = (PatternID < 2) ? -22 : 22;
        }
        else
        {
            Pos.x = (PatternID < 2) ? -34 : 34;
            Pos.z = Random.Range(-22.0f, 22.0f);
        }
        myDestination = Pos; //自身の終着地
    }

    void DoNotMove()
    {
        myNavi.enabled = false; //ナビメッシュをオフにする
    }

    // Update is called once per frame
    void Update()
    {
        if (!myNavi.enabled)
        {
            return;
        }

        //プレイヤーとの距離を計算する
        float D = Vector3.Distance(transform.position, Player.transform.position);
        if (D < AngryDistance)
        {
            myNavi.speed = 8.5f; //速足で
            myRenderer.material = angryMat; //怒った顔
            myNavi.SetDestination(Player.transform.position); //向かうはPlayer
        }
        else
        {
            myNavi.speed = 4.0f; //少し遅く
            myRenderer.material = normalMat; //元々の顔
            myNavi.SetDestination(myDestination); //向かうは終着地
        }

        if (Vector3.Distance(myDestination, transform.position) < 0.25f)
        {
            Destroy(gameObject); //終着地の周囲0.25に至ると自身を撤去
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI; //後ほどＵＩ要素を扱う際に必要です。


public class Warg : MonoBehaviour
{
    NavMeshAgent MyNavi; //自身のナビメッシュ
    Vector3 myDestination; //自身の終着地
    Animator MyAnim; //自身のアニメーター
    int PatternID; //自身の行動パターン
    public GameObject EXEP;
    public GameObject Portion;
    GameObject Manager; //マネージャー
    GameObject Player; //プレイヤー
    PlayerAction PA; //プレイヤーアクション
    bool isDead = false; //自身の死亡フラグ
    public float deathTime = 3.0f; //死亡後に消えるまでの時間
    public float D; //プレイヤーとの距離
    public float AttackDis;//攻撃開始距離
    public float TrackingDis;//追跡開始距離

    // Start is called before the first frame update
    void Start()
    {
        //自身のヘルス表示を取得
        MyNavi = GetComponent<NavMeshAgent>();
        MyAnim = GetComponent<Animator>();
        Manager = GameObject.FindGameObjectWithTag("GameController");
        //プレイヤーを見つけてPlayerActionを取得する
        Player = GameObject.FindGameObjectWithTag("Player");
        if (Player != null)
        {
            PA = Player.GetComponent<PlayerAction>();
        }
        //行動パターンIDを乱数で決定
        PatternID = Random.Range(0, 4);
        //自身の出発地を算出 ***********************
        Vector3 Pos = new Vector3(0, 0, 0); //高さ1.5固定
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
        MyNavi.enabled = false;
        transform.position = Pos;
        MyNavi.enabled = true;
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


    void OnTriggerEnter(Collider other)
    {
        //死んでなくて、刺さったオブジェクトがタグSwordで
        //タグSwordでプレイヤーがアタック期間中であれば
        if (other.gameObject.tag == "Sword" && !isDead && PA.CanAttack && !PA.isDead)
        {
            isDead = true; //死亡判定
            MyAnim.SetTrigger("Down"); //死亡モーション
            MyNavi.enabled = false; // ナビメッシュ切る
            MyAnim.SetFloat("Speed", 0); //移動はしない
                                         //爆発エフェクトを設置
            GameObject Fx = Instantiate(EXEP, transform.position,
            transform.rotation) as GameObject;
            if (Random.Range(1.0f, 10.0f) < 2.0f)
            {
                GameObject Por = Instantiate(Portion, transform.position,
                transform.rotation) as GameObject;
            }
            Manager.gameObject.SendMessage("ScoreUp",
            SendMessageOptions.DontRequireReceiver);
            Destroy(Fx, 2.0f); //爆発エフェクトを撤去
            Destroy(gameObject); //deathTime後に撤去
        }
    }

    void DoNotMove()
    {
        MyNavi.enabled = false; //ナビメッシュをオフにする
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead || !Player)
        {
            return; //プレイヤーがいないか、自身が死んでたら何もしない
        }
        //プレイヤーが死んでいる
        if (!PA.isDead)
        {
            //プレイヤーとの距離を求める
            D = Vector3.Distance(transform.position, Player.transform.position);
            // プレイヤーとの距離が1以下になった、立ち止まって攻撃開始
            if (D <= AttackDis)
            {
                MyNavi.enabled = false; //ナビ停止
                MyAnim.SetFloat("Speed", 0); //移動はしない
                MyAnim.SetBool("Attack", true); //攻撃開始
            }
            // プレイヤーとの距離が５以下になった、追いかけ始める
            else if (D <= TrackingDis)
            {
                MyNavi.enabled = true; //追いかける
                MyNavi.destination = Player.transform.position; //ターゲットを指示
                MyAnim.SetFloat("Speed", MyNavi.velocity.magnitude); //走行モーション
                MyAnim.SetBool("Attack", false); //攻撃は停止
            }
            else
            {
                MyNavi.enabled = true; //追いかける
                MyNavi.speed = 4.0f; //少し遅く
                MyNavi.SetDestination(myDestination); //向かうは終着地
                MyAnim.SetFloat("Speed", MyNavi.velocity.magnitude); //走行モーション
                MyAnim.SetBool("Attack", false); //攻撃は停止

            }


            if (Vector3.Distance(myDestination, transform.position) < 0.25f)
            {
                Destroy(gameObject); //終着地の周囲0.25に至ると自身を撤去
            }
        }
    }
}

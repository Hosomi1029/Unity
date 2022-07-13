using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; //ナビメッシュの運用に必要

public class EnemyAction : MonoBehaviour
{
    NavMeshAgent MyNavi; //自身のナビメッシュ
    Animator MyAnim; //自身のアニメーター
    GameObject Player; //プレイヤー
    PlayerAction PA; //プレイヤーアクション
    bool isDead = false; //自身の死亡フラグ
    public float deathTime = 3.0f; //死亡後に消えるまでの時間
    public float D; //プレイヤーとの距離

    // Start is called before the first frame update
    void Start()
    {
        MyNavi = GetComponent<NavMeshAgent>();
        MyAnim = GetComponent<Animator>();
        //プレイヤーを見つけてPlayerActionを取得する
        Player = GameObject.FindGameObjectWithTag("Player");
        if (Player != null)
        {
            PA = Player.GetComponent<PlayerAction>();
        }

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
            Destroy(gameObject, deathTime); //deathTime後に撤去
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (isDead || !Player)
        {
            return; //プレイヤーがいないか、自身が死んでたら何もしない
        }
        //プレイヤーが死んでいる
        if (PA.isDead)
        {
            MyNavi.enabled = false; // ナビメッシュ切る
            MyAnim.SetFloat("Speed", 0); //移動はしない
            MyAnim.SetBool("Attack", false); //攻撃は停止
        }
        else
        {
            //プレイヤーとの距離を求める
            D = Vector3.Distance(transform.position, Player.transform.position);
            // プレイヤーとの距離が1以下になった、立ち止まって攻撃開始
            if (D <= 1)
            {
                MyNavi.enabled = false; //ナビ停止
                MyAnim.SetFloat("Speed", 0); //移動はしない
                MyAnim.SetBool("Attack", true); //攻撃開始
            }
            // プレイヤーとの距離が５以下になった、追いかけ始める
            else if (D <= 5)
            {
                MyNavi.enabled = true; //追いかける
                MyNavi.destination = Player.transform.position; //ターゲットを指示
                MyAnim.SetFloat("Speed", MyNavi.velocity.magnitude); //走行モーション
                MyAnim.SetBool("Attack", false); //攻撃は停止
            }
            // プレイヤーと距離が５以上なら呼吸待機
            else if (D > 5)
            {
                MyNavi.enabled = false; //ナビ停止
                MyAnim.SetFloat("Speed", 0); //移動はしない
                MyAnim.SetBool("Attack", false); //攻撃は停止
            }
        }
    }
}

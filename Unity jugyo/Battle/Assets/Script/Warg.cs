using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI; //後ほどＵＩ要素を扱う際に必要です。


public class Warg : MonoBehaviour
{
    NavMeshAgent MyNavi; //自身のナビメッシュ
    Animator MyAnim; //自身のアニメーター
    public GameObject EXEP;
    GameObject Player; //プレイヤー
    PlayerAction PA; //プレイヤーアクション
    bool isDead = false; //自身の死亡フラグ
    public float deathTime = 3.0f; //死亡後に消えるまでの時間
    public float D; //プレイヤーとの距離
    Transform myCanvas; //自身のCanvas
    Image imgHealth; //ヘルスバー
    public Image imgLife; //画面のヘルスバー
    Text txtHealth; //ヘルス文字
    public int MaxHealth = 100; //ヘルスの最大値
    int Health; //自身のヘルス値

    // Start is called before the first frame update
    void Start()
    {
        //自身のヘルス表示を取得
        myCanvas = transform.Find("HealthCanvas");
        imgHealth = myCanvas.transform.Find("imgHealth").GetComponent<Image>();
        txtHealth = myCanvas.transform.Find("txtHealth").GetComponent<Text>();
        Health = MaxHealth; //ヘルスを最大にする

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
            Health -= 20;
        }
        if (Health <= 0)
        {
            isDead = true; //死亡判定
            MyAnim.SetTrigger("Down"); //死亡モーション
            MyNavi.enabled = false; // ナビメッシュ切る
            MyAnim.SetFloat("Speed", 0); //移動はしない
                                         //爆発エフェクトを設置
            GameObject Fx = Instantiate(EXEP, transform.position,
            transform.rotation) as GameObject;
            Destroy(Fx, 2.0f); //爆発エフェクトを撤去
            Destroy(gameObject); //deathTime後に撤去
        }
    }

    void LateUpdate()
    {
        //ヘルスバーを増減して色を決定
        imgHealth.fillAmount = Health / (float)MaxHealth;

        imgLife.fillAmount = Health / (float)MaxHealth;

        if (imgHealth.fillAmount > 0.5f)
        {
            imgHealth.color = Color.green;
        }
        else if (imgHealth.fillAmount > 0.2f)
        {
            imgHealth.color = Color.yellow;
        }
        else
        {
            imgHealth.color = Color.red;
        }
        //ヘルス値を表示
        txtHealth.text = Health.ToString("f0") + "/" + MaxHealth.ToString("f0");
        //常にキャンバスをカメラに向ける
        myCanvas.forward = Camera.main.transform.forward;
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
            if (D <= 2)
            {
                MyNavi.enabled = false; //ナビ停止
                MyAnim.SetFloat("Speed", 0); //移動はしない
                MyAnim.SetBool("Attack", true); //攻撃開始
            }
            // プレイヤーとの距離が５以下になった、追いかけ始める
            else if (D <= 7)
            {
                MyNavi.enabled = true; //追いかける
                MyNavi.destination = Player.transform.position; //ターゲットを指示
                MyAnim.SetFloat("Speed", MyNavi.velocity.magnitude); //走行モーション
                MyAnim.SetBool("Attack", false); //攻撃は停止
            }
            // プレイヤーと距離が５以上なら呼吸待機
            else if (D > 7)
            {
                MyNavi.enabled = false; //ナビ停止
                MyAnim.SetFloat("Speed", 0); //移動はしない
                MyAnim.SetBool("Attack", false); //攻撃は停止
            }
        }
    }
}

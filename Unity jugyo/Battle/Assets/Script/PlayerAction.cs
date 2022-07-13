using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //後ほどＵＩ要素を扱う際に必要です。

public class PlayerAction : MonoBehaviour
{
    public GameObject PatSmoke;
    public GameObject PatStrong;
    public GameObject PatBubble;
    public GameObject PatHeal;
    public bool CanAttack = false;
    public bool isDead = false;
    Animator MyAnim; // 自身のアニメーター
    ParticleSystem.MainModule SmokeMain; //砂煙の本体
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
        MyAnim = GetComponent<Animator>(); // 自身のアニメーターを取得
        SmokeMain = PatSmoke.GetComponent<ParticleSystem>().main;
        PatStrong.SetActive(false);
        PatBubble.SetActive(false);
        PatHeal.SetActive(false);

    }

    void AttackStart()
    {
        CanAttack = true;
    }
    void AttackFinish()
    {
        CanAttack = false;
    }

    void OnTriggerEnter(Collider other)
    {
        //死んでなくて、刺さったオブジェクトがタグWeaponであれば
        if (other.gameObject.tag == "Weapon" && !isDead)
        {
            Health -= 10;
            if (Health <= 0)
            {
                isDead = true; //死亡判定
                MyAnim.SetTrigger("Down"); //ダウンモーション発動
            }
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
        if (isDead)
        {
            return; //自身が死んでたら何もしない
        }
        //スペースキー押下で回復のエフェクトが発生
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PatHeal.SetActive(false);
            PatHeal.SetActive(true);
        }
        // PageUpキーでパワーアップを表現する
        PatStrong.SetActive(Input.GetKey(KeyCode.PageUp));
        // PageDownキーで毒に冒された表現にする
        PatBubble.SetActive(Input.GetKey(KeyCode.PageDown));

        // 入力操作を取得する
        float h = Input.GetAxis("Horizontal"); //左右移動を取得する
        float v = Input.GetAxis("Vertical"); //前後移動を取得する
        Vector3 dir = new Vector3(h, 0, v);

        // 移動方向への量に応じて砂ぼこりを制御する。
        SmokeMain.startSize = dir.sqrMagnitude * 1.5f;

        // 入力方向へ徐々に向いていく（回る）
        if (dir.sqrMagnitude > 0.01f)
        {
            Vector3 LookPos = Vector3.Slerp(transform.forward, dir, 8.0f * Time.deltaTime);
            transform.LookAt(transform.position + LookPos);
        }
        // 入力方向へ移動する
        transform.position += dir.normalized * 4.0f * Time.deltaTime;
        MyAnim.SetFloat("Speed", dir.magnitude);
        if (Input.GetButtonDown("Fire1"))
        {
            MyAnim.SetTrigger("Attack"); // 攻撃モーションの発動
        }
    }
}

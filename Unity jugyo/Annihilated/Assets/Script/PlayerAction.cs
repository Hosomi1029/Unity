using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //後ほどＵＩ要素を扱う際に必要です。
using XInputDotNetPure; //XInputDotNetの利用

public class PlayerAction : MonoBehaviour
{
    public float Walk;
    public float Run;
    public GameObject PatSmoke;
    GameObject Manager; //マネージャー
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
    Vector3 targetDirection;

    // Start is called before the first frame update
    void Start()
    {
        //自身のヘルス表示を取得
        myCanvas = transform.Find("HealthCanvas");
        imgHealth = myCanvas.transform.Find("imgHealth").GetComponent<Image>();
        txtHealth = myCanvas.transform.Find("txtHealth").GetComponent<Text>();
        Health = MaxHealth; //ヘルスを最大にする
        MyAnim = GetComponent<Animator>(); // 自身のアニメーターを取得
        Manager = GameObject.FindGameObjectWithTag("GameController");
        SmokeMain = PatSmoke.GetComponent<ParticleSystem>().main;
        //PatStrong.SetActive(false);
        //PatBubble.SetActive(false);
        PatHeal.SetActive(false);

    }

    void AttackStart()
    {
        //Debug.Log("attackst");
        CanAttack = true;
    }
    void AttackFinish()
    {
        //Debug.Log("attackfi");
        CanAttack = false;
    }

    void OnTriggerEnter(Collider other)
    {
        //死んでなくて、刺さったオブジェクトがタグWeaponであれば
        if (other.gameObject.tag == "Weapon" && !isDead)
        {
            Health -= 10;
            StartCoroutine(Vibration(0.0f, 1.0f, 0.5f)); //バイブレーション処理

            if (Health <= 0)
            {
                isDead = true; //死亡判定
                MyAnim.SetTrigger("Down"); //ダウンモーション発動
                Manager.gameObject.SendMessage("GameOver",
                SendMessageOptions.DontRequireReceiver);

            }
        }

        //回復をとったら
        if (other.gameObject.tag == "Portion" && !isDead)
        {
            Heal();
            Destroy(other.gameObject);
        }
    }

    //バイブレーション処理
    IEnumerator Vibration(float VibL, float VibR, float Duration)
    {
        GamePad.SetVibration(0, VibL, VibR);
        yield return new WaitForSeconds(Duration);
        GamePad.SetVibration(0, 0, 0); //バイブレーション停止
    }

    void Heal()
    {
        PatHeal.SetActive(true);
        Health += 20;
        if (Health >= MaxHealth)
        {
            Health = MaxHealth;
        }
        Invoke("ActiveStop", 1);
    }

     void ActiveStop()
    {
        PatHeal.SetActive(false);
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
        ////スペースキー押下で回復のエフェクトが発生
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    PatHeal.SetActive(false);
        //    PatHeal.SetActive(true);
        //}
        //// PageUpキーでパワーアップを表現する
        //PatStrong.SetActive(Input.GetKey(KeyCode.PageUp));
        //// PageDownキーで毒に冒された表現にする
        //PatBubble.SetActive(Input.GetKey(KeyCode.PageDown));

        // 入力操作を取得する
        float h = Input.GetAxis("Horizontal"); //左右移動を取得する
        float v = Input.GetAxis("Vertical"); //前後移動を取得する

        //カメラの正面方向ベクトルからY成分を除き、正規化してキャラが走る方向を取得
        Vector3 forward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 right = Camera.main.transform.right; //カメラの右方向を取得
        //カメラの方向を考慮したキャラの進行方向を計算
        targetDirection = h * right + v * forward;
        Vector3 dir = new Vector3(h , 0, v);

        // 移動方向への量に応じて砂ぼこりを制御する。
        SmokeMain.startSize = dir.sqrMagnitude * 1.5f;

        // 入力方向へ徐々に向いていく（回る）
        if (dir.sqrMagnitude > 0.01f)
        {
            Vector3 LookPos = Vector3.Slerp(transform.forward, targetDirection, 8.0f * Time.deltaTime);
            transform.LookAt(transform.position + LookPos);
        }
        // 入力方向へ移動する
        if (dir.magnitude >= 0.8)
        {
            transform.position += targetDirection * Run * Time.deltaTime;
        }
        else
        {
            transform.position += targetDirection * Walk * Time.deltaTime;
        }
        MyAnim.SetFloat("Speed", dir.magnitude);
        if (Input.GetButtonDown("BtnX"))
        {
            MyAnim.SetTrigger("Attack"); // 攻撃モーションの発動
        }
    }
}

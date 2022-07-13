using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //uGUIの利用に必要
using XInputDotNetPure; //XInputDotNetの利用

public class PlayerAction : MonoBehaviour
{
    public GameObject BulletPrefab; //弾のプレハブ
    public GameObject SmokePrefab; //煙硝のプレハブ
    public GameObject Body; //回転砲塔
    public GameObject[] Muzzle; //銃口
    public AudioClip SE_None; //無効サウンド
    public AudioClip SE_ReLoad; //装填サウンド
    public AudioClip SE_Shoot; //発砲サウンド
    public float RotSpeed = 60.0f; //回転速度
    public float MovSpeed = 5.0f; //移動速度
    public float TriggerLevel = 0.7f; //トリガーのしきい値
    public float BulletSpeed = 30.0f; //弾の速度
    bool CanFire = true; //発砲可能の真偽値
    AudioSource MyAudio; //自身の音源
    public GameObject LeftArm;
    public GameObject RightArm;
    Vector3 LeftArmPos; //最初の位置
    Vector3 RightArmPos; //最初の位置
    int BulletCnt; //弾カウンター
    public Text txtBulletCnt; //弾数表示
    public int BulletMax = 90; //最大所有弾数
    bool Damaged = false; //ダメージ状態管理
    GameObject Manager; //マネージャー

    // Start is called before the first frame update
    void Start()
    {
        //マネージャーを取得
        Manager = GameObject.FindGameObjectWithTag("GameController");
        //両腕の最初の位置を記憶
        LeftArmPos = LeftArm.transform.localPosition;
        RightArmPos = RightArm.transform.localPosition;
        MyAudio = GetComponent<AudioSource>(); //自身の音源を取得
        ReLoad(); //リロード処理
    }

    //リロード処理
    void ReLoad()
    {
        BulletCnt = BulletMax; //弾を最大値にして表示する。
        txtBulletCnt.text = BulletCnt.ToString().PadLeft(2, '0');
        txtBulletCnt.color = Color.white;
    }

    //発砲処理
    void Fire()
    {
        for (int idx = 0; idx < Muzzle.Length; idx++)
        {
            //銃口に弾を生成する。
            GameObject B = Instantiate(BulletPrefab,
            Muzzle[idx].transform.position, Quaternion.identity);
            //弾に速度を与える。
            B.GetComponent<Rigidbody>().velocity = Body.transform.forward * BulletSpeed;
            //銃口から硝煙を放ち、寿命を与える。
            GameObject S = Instantiate(SmokePrefab,
            Muzzle[idx].transform.position, Muzzle[idx].transform.rotation);
            Destroy(S, 1.0f);
        }
        //ノックバックの位置へ移動
        LeftArm.transform.localPosition += new Vector3(0, 0, -0.15f);
        RightArm.transform.localPosition += new Vector3(0, 0, -0.15f);

        GetComponent<AudioSource>().PlayOneShot(SE_Shoot); //射撃音の鳴動
        StartCoroutine(Vibration(0.0f, 0.8f, 0.18f)); //バイブレーション処理
    }
    //バイブレーション処理
    IEnumerator Vibration(float VibL, float VibR, float Duration)
    {
        GamePad.SetVibration(0, VibL, VibR);
        yield return new WaitForSeconds(Duration);
        GamePad.SetVibration(0, 0, 0); //バイブレーション停止
    }

    //ゴーストとの接触処理
    void OnDamage()
    {
        if (!Damaged)
        {
            Damaged = true;
            //マネージャーにOnHitGhostを伝達
            Manager.SendMessage("OnHitGhost", SendMessageOptions.DontRequireReceiver);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Damaged)
        {
            Body.transform.Rotate(0, 360 * Time.fixedDeltaTime, 0); //砲塔部を自動旋回
        }

        //ノックバックの疑似表現
        LeftArm.transform.localPosition = Vector3.Lerp(
        LeftArm.transform.localPosition, LeftArmPos, 0.1f);
        RightArm.transform.localPosition = Vector3.Lerp(
        RightArm.transform.localPosition, RightArmPos, 0.1f);

        if (Gamemanager.GameStatus != Gamemanager.STS.PLAY)
        {
            return; //プレイ中のみ操作可能
        }

        //右バンパー押下でリロード
        if (Input.GetButtonDown("BumperR"))
        {
            if (BulletCnt < 1)
            {
                ReLoad();
                MyAudio.PlayOneShot(SE_ReLoad); //リロード鳴動
                StartCoroutine(Vibration(1.0f, 1.0f, 0.8f));
            }
            else
            {
                MyAudio.PlayOneShot(SE_None); //無効サウンド鳴動
            }
        }

        float Rt = Input.GetAxis("BtnA"); //右トリガーの回転値
                                              //右トリガーで発砲制御
        if (!CanFire && Rt < TriggerLevel)
        {
            CanFire = true;
        }
        if (CanFire && Rt > TriggerLevel)
        {
            CanFire = false; if (BulletCnt < 1)
            {
                MyAudio.PlayOneShot(SE_None); //無効サウンド鳴動
            }
            else
            {
                Fire(); //発砲処理
                BulletCnt--; //弾数を減算
                if (BulletCnt < 1)
                {
                    //リロード指示を表示
                    txtBulletCnt.text = "R";
                    txtBulletCnt.color = Color.yellow;
                }
                else
                {
                    //残り弾数を表示
                    txtBulletCnt.text = BulletCnt.ToString().PadLeft(2, '0');
                }
            }
        }

        float Rh = Input.GetAxis("HorizontalR"); //右スティックの水平値
        float Rv = Input.GetAxis("VerticalR"); //右スティックの垂直値
        Vector3 DirR = new Vector3(Rh, 0, Rv); //射撃方向を作成
        DirR = transform.TransformDirection(DirR); //車両部を基準に変換
        if (DirR.sqrMagnitude > 0.01f)
        { //0.1以上のスティック操作で反応
          //砲塔部が向く位置を算出
            Vector3 FirePos = transform.position + DirR.normalized;
            FirePos += new Vector3(0, Body.transform.position.y, 0);
            //砲塔部を射撃方向に向ける。
            Body.transform.LookAt(FirePos);
        }

        float h = Input.GetAxis("Horizontal"); //左スティックの水平値
        float v = Input.GetAxis("Vertical"); //左スティックの垂直値
                                             //自身の前方を元に左スティックの前後動作をベクトルにする。
        Vector3 Dir = new Vector3(0, 0, v);
        Dir = transform.TransformDirection(Dir);
        //求めたベクトルを自身の位置に加える。
        transform.position += Dir * MovSpeed * Time.fixedDeltaTime;
        //左スティックの左右動作を自身の回転情報に加える。
        transform.Rotate(0, h * RotSpeed * Time.fixedDeltaTime, 0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxAction : MonoBehaviour
{
    float Elapsed = 0.0f;
    bool Damaged = false; //撃たれたか
    public GameObject Effect; //爆発エフェクトのプレハブ
    public float KillTime = 0.4f; //撃たれてから爆発するまでの遅延時間
    public string ColorName; //自身の色名
    GameObject Manager; //マネージャー
    AudioSource myAudio; //自身の音源

    // Start is called before the first frame update
    void Start()
    {
        Manager = GameObject.Find("GameController"); //マネージャーを探して取得する
        myAudio = GetComponent<AudioSource>(); //自身の音源を取得
    }

    void OnDamage()
    {
        if (!Damaged)
        {
            Damaged = true;
            GetComponent<Rigidbody>().AddForce(Vector3.up * 15.0f, ForceMode.Impulse);
            myAudio.Play(); //命中音鳴動
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Damaged)
        {
            Elapsed += Time.deltaTime; //経過時間を計測
            if (Elapsed > KillTime)
            {
                //撤去直前に自身の色名を添えてOnDestroyBoxをマネージャーに通達。
                Manager.SendMessage("OnDestroyBox", ColorName,
                SendMessageOptions.DontRequireReceiver);

                //爆発エフェクトを設置
                GameObject Fx = Instantiate(Effect, transform.position,
                transform.rotation) as GameObject;
                Destroy(Fx, 2.0f); //爆発エフェクトを撤去
                Destroy(gameObject); //自身（箱）を撤去
            }
        }
    }
}

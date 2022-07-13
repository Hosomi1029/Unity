using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeAction : MonoBehaviour
{
    float Elapsed = 0.0f; //経過時間
    float Level; //点滅レベル
    bool isAlarm; //アラーム中か？
    public AudioClip alarmSE; //アラーム音
    public GameObject EffectPrefab; //エフェクトプレハブ
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Elapsed += Time.deltaTime;
        if (isAlarm)
        { //サイン関数で指定色に変化させ点滅を演出
            Level = Mathf.Abs(Mathf.Sin(40.0f * Time.time));
            GetComponent<Renderer>().material.color = Color.red * Level;
            if (Elapsed > 6.0f)
            {
                //エフェクトをインスタンス生成
                GameObject E = Instantiate(EffectPrefab, transform.position,
                Quaternion.identity) as GameObject;
                Destroy(E, 2.0f); //エフェクトを２秒後に撤去
                Destroy(gameObject, 0.0f); //自身Spikeを即刻撤去
            }
        }
        else if (Elapsed > 5.0f)
        {
            GetComponent<AudioSource>().PlayOneShot(alarmSE, 1.0f);
            isAlarm = true;
        }
    }
}

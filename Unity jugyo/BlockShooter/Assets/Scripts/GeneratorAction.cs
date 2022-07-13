using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorAction : MonoBehaviour
{
    public enum BC
    {
        RED,
        BLUE,
        GREEN
    }

    public float Interval = 1.0f; //発生間隔（秒）
    public GameObject BlueBoxPrefab; //青箱のプレハブ
    public GameObject RedBoxPrefab; //赤箱のプレハブ
    public GameObject GreenBoxPrefab;//緑箱のプレハブ
    GameObject SpawnBox;
    //bool NextIsRed; //次は赤箱か？の真偽
    BC bc;
    float Elapsed; //経過時間

    // Start is called before the first frame update
    void Start()
    {
        //NextIsRed = true; //赤から開始
        bc = BC.RED;
        Elapsed = 0.0f;
    }

    void GameStart()
    {
        enabled = true; //スクリプトを稼働させる
    }

    void TimeUp()
    {
        enabled = false; //スクリプトを停止
    }

    // Update is called once per frame
    void Update()
    {
        Elapsed -= Time.deltaTime;
        if (Elapsed <= 0.0f)
        { //発生までの時間がゼロになったら
            Vector3 Pos = new Vector3(0, 10.0f, 0); //箱を生成するランダム位置Pos
            Pos.x = Random.Range(-8.0f, 8.0f);
            Pos.z = Random.Range(-4.0f, 4.0f);
            //生成するプレハブを割り当てる
            //GameObject SpawnBox = NextIsRed ? RedBoxPrefab : BlueBoxPrefab;
            switch (bc) {
                case BC.RED:
                    SpawnBox = RedBoxPrefab;
                    break;
                case BC.BLUE:
                    SpawnBox = BlueBoxPrefab;
                    break;
                case BC.GREEN:
                    SpawnBox = GreenBoxPrefab;
                    break;
                }
            //箱のプレハブをインスタンス生成する
            Instantiate(SpawnBox, Pos, Random.rotation);
            Elapsed = Interval; //次の発生までの時間を設定
            //NextIsRed = !NextIsRed; //次の発生の為に赤青を反転
            bc++;
            if(SpawnBox == GreenBoxPrefab)
            {
                bc = BC.RED;
            }
        }
    }
}

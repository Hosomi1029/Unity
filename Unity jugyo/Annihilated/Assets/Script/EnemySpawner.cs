using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject WargPrefab; //ゴーストのプレハブ
    public float IntervalMin = 0.5f;
    public float IntervalMax = 2.0f;
    const int ENEMYMAX = 20; //同時に存在できる最大ゴースト数
    bool activate = true;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Spawn");
    }

    void TimeUp()
    {
        activate = false;
    }
    void GameClear()
    {
        activate = false;
    }

    void OnHitGhost()
    {
        activate = false;
    }


    IEnumerator Spawn()
    {
        while (true)
        {
            float Interval = Random.Range(IntervalMin, IntervalMax);
            yield return new WaitForSeconds(Interval);
            // 現在のゴースト数を算出
            int GhostCnt = GameObject.FindGameObjectsWithTag("Enemy").Length;
            if (activate && GhostCnt < ENEMYMAX)
            {
                Instantiate(WargPrefab); // 敵生成
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour
{
    float Elapsed = 0.0f;
    float Theta;
    public float Interval = 1.5f;
    public GameObject CoinPrefab;
    public GameObject SpikePrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Elapsed += Time.deltaTime;
        if (Elapsed >= Interval)
        {
            Elapsed = 0.0f;
            //ランダムの角度シータ（θ）
            Theta = Random.Range(0.0f, Mathf.PI * 2.0f);
            //高さ5で直径12の円周のどこか
            Vector3 pos = new Vector3(Mathf.Cos(Theta) * 6.0f, 5.0f, Mathf.Sin(Theta) * 6.0f);
            GameObject Prefab = (Random.value < 0.3f) ? CoinPrefab : SpikePrefab;
            Instantiate(Prefab, pos, Random.rotation); //インスタンス生成

        }
    }
}

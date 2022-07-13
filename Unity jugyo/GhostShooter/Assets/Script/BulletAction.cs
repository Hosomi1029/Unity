using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAction : MonoBehaviour
{
    public GameObject HitPrefab; //ヒットエフェクトのプレハブ
    void OnTriggerEnter(Collider other)
    {
        GameObject Fx = Instantiate(HitPrefab, transform.position, Quaternion.identity) as GameObject;
        Destroy(Fx, 1.0f); //エフェクトを１秒後に撤去
        Destroy(gameObject); //自身（弾）を撤去
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

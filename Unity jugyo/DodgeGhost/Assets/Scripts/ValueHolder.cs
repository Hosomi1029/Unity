using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueHolder : MonoBehaviour
{
    static public int score;
    static public int stage;
    static public int remain;
    void Awake()
    {
        //別シーンへ移動しても自身を破棄しない
        DontDestroyOnLoad(gameObject);
        score = 0;
        stage = 1;
        remain = 3;
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

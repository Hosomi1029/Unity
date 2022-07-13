using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //uGUIを使う宣言
using UnityEngine.SceneManagement; //シーンマネージャーを使う宣言
using System.IO; //外部ファイルのInput-Outputに必要

public class GameManager : MonoBehaviour
{
    public GameObject ValueKeeperPrefab;
    AudioSource myAudio; //自身の音源
    public Text txtMessage;
    public Text txtNavigate;
    public Text txtScore;
    public Text txtStage;
    public Text txtRemain;
    public AudioClip ClearSE;
    public AudioClip OverSE;
    public AudioClip PowerSE;
    public AudioClip StartSE;
    public GameObject DotPrefab;
    public GameObject PowerPrefab;
    public GameObject GhostPrefab;
    public bool isTestMode = true;
    public string filePath = "CSV/";
    List<int> DotList; //Dotリスト
    Vector3 pos1 = new Vector3(2, 1, -3);
    Vector3 pos2 = new Vector3(4, 1, -3);
    Vector3 pos3 = new Vector3(6, 1, -3);
    public enum STS
    {
        PLAY,
        CLEAR,
        MISS,
        OVER
    }
    STS GameStatus; //ゲームステータス
    float Elapsed; //経過時間
    GameObject Player;
    Camera mainCamera; //メインカメラ
    Camera frontCamera; //フロントカメラ

    // Start is called before the first frame update
    void Start()
    {
        if (!GameObject.FindGameObjectWithTag("Value"))
        {
            Instantiate(ValueKeeperPrefab);
        }
        Player = GameObject.FindGameObjectWithTag("Player");
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        frontCamera = GameObject.Find("FrontCamera").GetComponent<Camera>();
        mainCamera.enabled = true; //メインカメラを有効
        frontCamera.enabled = false; //フロントカメラを無効
        GameStatus = STS.PLAY;
        Elapsed = 0.0f;
        DotGenerator(); //Dot生成処理
        StartCoroutine("GhostGenerator"); //Ghost生成処理
        txtMessage.text = "START!";
        txtNavigate.gameObject.SetActive(false);
        Invoke("TextClear", 1.5f); //1.5秒後にSTART!を消す
        myAudio = GetComponent<AudioSource>(); //自身の音源を取得
        myAudio.PlayOneShot(StartSE); //開始ボイス鳴動
    }

    //Ghost生成処理
    IEnumerator GhostGenerator()
    {
        yield return new WaitForSeconds(2.0f); //プレイ開始後、発生を少し猶予する
        int ghostCnt = ValueHolder.stage; //ステージ数だけゴースト生成する。
        for (int idx = 0; idx < ghostCnt; idx++)
        {
            Instantiate(GhostPrefab, new Vector3(0, 1, 1), Quaternion.identity);
            yield return new WaitForSeconds(3.0f); //３秒間隔でゴースト生成
        }
    }

    //Dot生成処理
    void DotGenerator()
    {
        if (isTestMode)
        {
            Instantiate(DotPrefab, pos1, Quaternion.identity);
            Instantiate(DotPrefab, pos2, Quaternion.identity);
            Instantiate(DotPrefab, pos3, Quaternion.identity);
            return; //テストモードなら3個だけでreturn
        }
        TextAsset csv = Resources.Load(filePath) as TextAsset;
        StringReader reader = new StringReader(csv.text);
        DotList = new List<int>(); //Dotリストを初期化
                                   //１行ずつ読みながらDotリストに格納する
        for (int idx = 0; reader.Peek() > -1; idx++)
        {
            //1行だけ読み込む（1行あたり17個のDotデータ）
            string line = reader.ReadLine();
            //カンマで17個に分離し、文字列配列valuesに代入
            string[] values = line.Split(',');
            foreach (string Stored in values)
            {
                //文字列を整数に変換してDotリストに17個を追加
                DotList.Add(int.Parse(Stored));
            }
        }
        //完成したDotListを元に、Dotを配置する。
        for (int idy = 0; idy < 20; idy++)
        {
            for (int idx = 0; idx < 17; idx++)
            {
                Vector3 pos = new Vector3((2 * idx) - 16, 1, (-2 * idy) + 19);
                if (DotList[idy * 17 + idx] == 1)
                {
                    Instantiate(DotPrefab, pos, Quaternion.identity);
                }
                if (DotList[idy * 17 + idx] == 2)
                {
                    Instantiate(PowerPrefab, pos, Quaternion.identity);
                }

            }
        }
    }

    void TextClear()
    {
        txtMessage.text = "";
    }

    //ゴーストを全て破棄する
    void ClearGhost()
    {
        //【命題】 タグがGhostのオブジェクトを全て見つけてきて破棄する。
        GameObject[] Ghost = GameObject.FindGameObjectsWithTag("Ghost");
        foreach(GameObject Stored in Ghost)
        {
            Destroy(Stored);
        }
    }

    //ゴースト接触処理
    void HitGhost()
    {
        myAudio.PlayOneShot(OverSE);
        txtMessage.enabled = true;
        if (ValueHolder.remain > 1)
        { //残機が１より多い
            GameStatus = STS.MISS;
            txtMessage.text = "Uoops...";
            StartCoroutine("ReLoader"); //リロード処理
        }
        else
        {
            GameStatus = STS.OVER;
            txtNavigate.text = "Push      Button to Title";
            txtMessage.text = "GAME OVER";
        }
    }
    //リロード処理
    IEnumerator ReLoader()
    {
        yield return new WaitForSeconds(5.0f); //５秒待ってから
        ValueHolder.remain -= 1; //残機数減
        SceneManager.LoadScene("Main"); //シーンMainをロード
    }

    // パワーアップ処理
    void PowerUp(float PowerTime)
    {
        myAudio.PlayOneShot(PowerSE);
        //全ゴーストにパワーアップ状態を通告
        GameObject[] Ghosts = GameObject.FindGameObjectsWithTag("Ghost");
        foreach (GameObject Stored in Ghosts)
        {
            Stored.SendMessage("PowerUp", PowerTime,
            SendMessageOptions.DontRequireReceiver);
        }
    }

    // Update is called once per frame
    void Update()
    {
        txtScore.text = "SCORE:" + ValueHolder.score;
        txtStage.text = "STAGE:" + ValueHolder.stage;
        txtRemain.text = "REMAIN:" + ValueHolder.remain;

        switch (GameStatus)
        {
            case STS.PLAY:
                //Dotが全部なくなったら
                if (GameObject.FindGameObjectsWithTag("Dot").Length +
                    GameObject.FindGameObjectsWithTag("Power").Length < 1)
                {
                    GameStatus = STS.CLEAR;
                    Player.SendMessage("GameClear",
                    SendMessageOptions.DontRequireReceiver);
                    mainCamera.enabled = false; //メインカメラをオフにして
                    frontCamera.enabled = true; //フロントカメラに切り替える
                    txtNavigate.text = "Push      Button to Next";
                    txtMessage.text = "CLEAR!";
                    txtMessage.enabled = true;
                    myAudio.PlayOneShot(ClearSE);
                    ClearGhost(); //ゴーストを全て破棄する
                }
                break;
            case STS.CLEAR:
                Elapsed += Time.deltaTime;
                Elapsed %= 1.0f;
                txtNavigate.gameObject.SetActive(Elapsed < 0.8f);
                if (Input.GetButtonDown("Fire2"))
                {
                    ValueHolder.stage += 1; //ステージ番号アップ
                    SceneManager.LoadScene("Main"); //シーンMainをリロード
                }
                break;
            case STS.MISS:
                break;
            case STS.OVER:
                Elapsed += Time.deltaTime;
                Elapsed %= 1.0f;
                txtNavigate.gameObject.SetActive(Elapsed < 0.8f);
                if (Input.GetButtonDown("Fire2"))
                {
                    Destroy(GameObject.FindGameObjectWithTag("Value")); //値管理を破棄
                    SceneManager.LoadScene("Title"); //シーンMainをリロード
                }
                break;
            default:
                break;
        }
    }
}

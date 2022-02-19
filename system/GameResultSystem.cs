using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameResultSystem : MonoBehaviour
{

    enum RESULT_SE
    {
        RESULT_COIN,
        BONUS_COIN
    }

    coin Coin;
    AdSystem adsys;
    [SerializeField] GameObject scatterCoinEffect;//コインをばらまくエフェクト
    Canvas canvas;

    [SerializeField] int coinSwitchFrame;
    int coinSwitchFrameCnt;
    int frameSwCoinNum;//1フレームに減らすコインの量
    [SerializeField] bool coinSw;
    //[SerializeField] GameObject AdButton;
    [SerializeField] int bonusCoinNum;
    [SerializeField] GameObject DispBonusCoin;

    [SerializeField] AudioClip[] resultSE = new AudioClip[2];
    AudioSource AS;

    // Start is called before the first frame update
    void Start()
    {

        AS = GetComponent<AudioSource>();

        //コイン関係オブジェクトを取得
        Coin = GameObject.Find("CoinEvent").GetComponent<coin>();

        scatterCoinEffect = (GameObject)Resources.Load("Effect/Coin rain");

        //カメラを指定
        canvas = GameObject.Find("ResultCanvas").GetComponent<Canvas>();
        canvas.worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();



        //1フレームに減らすコインの量生成

        //広告部分についているものではこの処理をしない
        if (coinSw)
        {

            frameSwCoinNum = Coin.GetnCurrent() / coinSwitchFrame;
            if (frameSwCoinNum <= 0) frameSwCoinNum = 1;
            coinSwitchFrameCnt = coinSwitchFrame;
            //音を鳴らす
            //GetComponent<AudioSource>().Play();
            AS.PlayOneShot(resultSE[(int)RESULT_SE.RESULT_COIN]);
        }

        //ボーナスコインの数算出
        bonusCoinNum = Coin.GetnCurrent();

    }

    // Update is called once per frame
    void Update()
    {
        if(coinSw) CoinSwitch();
    }

    public void LoadCoinBonusAdScene()
    {

        //コインボーナス用adを呼び出す

        //シーン(広告呼び出し)
        SceneManager.LoadScene("bonusAd", LoadSceneMode.Additive);

        //adシステムを読み込み
        //adsys = GameObject.Find("AdSystem").GetComponent<AdSystem>();

        //ボーナス処理を実行
        //adsys.CoinBonus();

        
    }

    //現在のコインを累計コインに移動
    //adの戻るボタン
    public void currentCointoTotalCoin()
    {

        //実値を移動
        Coin.Result();

        //演出
        Camera mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();

        //UI座標で位置を指定し、それをワールド座標に変換して生成する
        Vector2 screenPos = new Vector2(540,1900);
        Vector3 world_position = mainCamera.ScreenToWorldPoint(screenPos);

        //コインをばらまく演出
        GameObject obj = Instantiate(scatterCoinEffect,
            world_position,
            Quaternion.Euler(-90f, 0f, 0f));

        obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, -5);

        //obj.GetComponent<ScatterCoin>().BurstNum(100);

        //ボーナスボタンを消す
        Destroy(GameObject.Find("BonusButton"));

        //ボーナスボタンの位置に増えたコインの量を出す
        //保留
        screenPos = new Vector2(0, 1920/2 - 270);

        GameObject prb = Instantiate(DispBonusCoin,
            new Vector3(0,0,0),
            Quaternion.identity);

        //prb.transform.SetParent(GameObject.Find("ResultCanvas").transform);
        //prb.transform.position = screenPos;

        prb.GetComponent<GetCoinNum>().Display(bonusCoinNum);

        //コイン音を一回鳴らす
        GameObject.Find("GameResultSystem").GetComponent<AudioSource>().Play();

        AudioSource rAS = GameObject.Find("GameResultSystem").GetComponent<AudioSource>();

        rAS.PlayOneShot(resultSE[(int)RESULT_SE.BONUS_COIN]);

        //トータルコインをセーブ
        Coin.Save();

        //広告シーンを閉じる
        SceneManager.UnloadSceneAsync("bonusAd");




    }

    //現在のコインをトータルコインに移す
    void CoinSwitch()
    {

        //フレームごとにカレントコインを減らし、合計コインを増やす
        if (coinSwitchFrameCnt > 0)
        {


            //移動
            if(Coin.GetnCurrent() >= frameSwCoinNum) Coin.Result(frameSwCoinNum);

            coinSwitchFrameCnt--;
            if (coinSwitchFrameCnt <= 0)
            {
                GetComponent<AudioSource>().Stop();

                //端数が残っていたら移動
                Coin.Result();

                //トータルコインをセーブ
                Coin.Save();

            }
        }




    }

    public int GetBonusCoinNum()
    {
        return bonusCoinNum;
    }


}

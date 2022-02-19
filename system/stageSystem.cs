using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class stageSystem : MonoBehaviour
{

    bool stagePlayingFlag;//ステージ動作中かどうか
    bool stageClearFlag;
    bool stageGameoverFlag;
    coin Coin;
    Scene startScene;

    //[SerializeField] int stageItemNum;
    [SerializeField] int playerMiddleScaleNum;
    [SerializeField] int playerLargeScaleNum;
    [SerializeField] int playerMaxScaleNum;
    [SerializeField] int ItemNum_playerMaxScale;

    UIControll uiControll;

    [SerializeField] Text stageNumText;
    [SerializeField] Image stageNumImage;
    [SerializeField] Sprite[] stageNumImages;

    BGM bgm;

    // Start is called before the first frame update
    void Start()
    {

        stagePlayingFlag = false;
        stageClearFlag = false;
        stageGameoverFlag = false;
        Coin = GameObject.Find("CoinEvent").GetComponent<coin>();

        //シーンの呼び出し
        SceneManager.LoadScene("GameStart", LoadSceneMode.Additive);

        uiControll = GameObject.Find("GameSystem").GetComponent<UIControll>();

        bgm = GameObject.Find("BGM").GetComponent<BGM>();

        if (!GameSystem.gameStartFlag)
        {
            Coin = GameObject.Find("CoinEvent").GetComponent<coin>();
            Coin.LoadCoin();

            //タイトルUIセット
            uiControll.SetTitleUI();
            
        }
        else
        {

            Coin.LoadCoin();
            //Coin.Result(0);

            //プレイ中UIセット
            uiControll.SetPlayingUI();
        }


        Application.targetFrameRate = 60;


        stageNumImage = GameObject.Find("startNumImage").GetComponent<Image>();
        SetStageNum();

    }

    // Update is called once per frame
    void Update()
    {
        ////画面をタッチしたら開始
        //if (!stageClearFlag && !stagePlayingFlag && touchSystem.GetTouchFlag())
        //{
        //
        //    stagePlayingFlag = true;
        //    Invoke("StageStart", 0.05f);
        //    //StageStart();
        //}

        //１回目じゃなければ画面タッチスクリプトを動かす
        if (GameSystem.gameStartFlag && !stageClearFlag && !stagePlayingFlag && touchSystem.GetTouchFlag())
        {
        
            stagePlayingFlag = true;
            Invoke("StageStart", 0.05f);
            //StageStart();
        }

        if (!stagePlayingFlag) Coin.Result(0);

    }

    void StageStart()
    {

        //stagePlayingFlag = true;

        //スタートシーン削除
        SceneManager.UnloadSceneAsync("GameStart");

        //タイトル削除
        if (!GameSystem.gameStartFlag)
        {
            //SceneManager.UnloadSceneAsync("Title");
            GameSystem.gameStartFlag = true;
          
        }

        //プレイ中UIセット
        uiControll.SetPlayingUI();

        //プレイヤーの転がる音始動
        player Player = GameObject.Find("player").GetComponent<player>();
        Player.PlayRollingSound(true);

    }

    public bool GetStagePlayingFlag()
    {
        return stagePlayingFlag;
    }

    public bool GetStageGameoverFlag()
    {
        return stageGameoverFlag;
    }

    public bool GetStageClearFlag()
    {
        return stageClearFlag;
    }

    public void StageClear()
    {
        stageClearFlag = true;
    }

    public void StageGameover()
    {
        stageGameoverFlag = true;

        //シーンの呼び出し
        SceneManager.LoadScene("GameOver", LoadSceneMode.Additive);

        bgm.Mute(true);
    }

    public void StageGameoverFlagOn()
    {
        stageGameoverFlag = true;
    }


    public void StageContinue()
    {

        stageGameoverFlag = false;

    }

    //ステージ終了時処理
    public void StageEnd()
    {

        //今ステージで取得したコインをトータルに反映する
        //Coin.Result();

        stagePlayingFlag = false;

        //シーンの呼び出し
        SceneManager.LoadScene("GameResult", LoadSceneMode.Additive);

        //リザルトUIセット
        uiControll.SetResultUI();

        bgm.Mute(true);

    }

    public void AdDelete()
    {
        //広告シーン削除
        SceneManager.UnloadSceneAsync("Ad");
    }

    public int GetPlayerMiddleScaleNum()
    {
        return playerMiddleScaleNum;
    }

    public int GetPlayerLargeScaleNum()
    {
        return playerLargeScaleNum;
    }

    public int GetPlayerMaxScaleNum()
    {
        return playerMaxScaleNum;
    }

    public int GetItemNumPlayerMax() {
        return ItemNum_playerMaxScale;
    }


    public float GetPlayerMiddlePlusScale()
    {
        return  playerMiddleScaleNum / (float)ItemNum_playerMaxScale;
    }

    public float GetPlayerLargePlusScale()
    {
        return (playerLargeScaleNum - playerMiddleScaleNum) / (float)ItemNum_playerMaxScale;
    }

    public float GetPlayerMaxPlusScale()
    {
        return (playerMaxScaleNum - playerLargeScaleNum) / (float)ItemNum_playerMaxScale;
    }

    public void SetStageNum()
    {

        stageNumImage.sprite = stageNumImages[GameSystem.currentStageNum - 1];

    }

    public void EventTrigger_GameStart()
    {

        //画面をタッチしたら開始
        //if (!stageClearFlag && !stagePlayingFlag && touchSystem.GetTouchFlag())
        //{
            Debug.Log("chit");
            stagePlayingFlag = true;
            Invoke("StageStart", 0.05f);
            //StageStart();
        //}

    }


}

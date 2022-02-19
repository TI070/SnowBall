using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIControll : MonoBehaviour
{

    [SerializeField] GameObject currentCoinUI;
    [SerializeField] GameObject totalCoinUI;
    [SerializeField] GameObject StageNumUI;
    [SerializeField] Canvas TitleUI;
    [SerializeField] Canvas GameStartUI;

    [SerializeField] Vector3 titleTCPos;

    [SerializeField] Vector3 playingCCPos;

    [SerializeField] Vector3 resultTCPos;
    [SerializeField] Vector3 resultCCPos;
    [SerializeField] Vector3 skinShopTCPos;

    [SerializeField] Camera TitleCamera;
    [SerializeField] Camera GameStartCamera;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //タイトル時のUI設定
    public void SetTitleUI()
    {

        RectTransform currentCoinPos;
        RectTransform totalCoinPos;

        //移動するUIを取得
        currentCoinUI = GameObject.Find("CoinUICurrent").transform.GetChild(0).gameObject;
        totalCoinUI = GameObject.Find("CoinUITotal").transform.GetChild(0).gameObject;
        StageNumUI = GameObject.Find("stageCanvas");

        StageNumUI.SetActive(true);

        currentCoinPos = currentCoinUI.GetComponent<RectTransform>();
        totalCoinPos = totalCoinUI.GetComponent<RectTransform>();

        totalCoinPos.localPosition = titleTCPos;



        //アクティブ状態を変更
        currentCoinUI.SetActive(false);
        totalCoinUI.SetActive(true);
        StageNumUI.SetActive(true);

        
    }

    //リザルト時のUI設定
    public void SetResultUI()
    {

        RectTransform currentCoinPos;
        RectTransform totalCoinPos;

        //移動するUIを取得
        currentCoinUI = GameObject.Find("CoinUICurrent").transform.GetChild(0).gameObject;
        totalCoinUI = GameObject.Find("CoinUITotal").transform.GetChild(0).gameObject;
        StageNumUI = GameObject.Find("stageCanvas");

        currentCoinUI.SetActive(true);
        totalCoinUI.SetActive(true);
        StageNumUI.SetActive(true);

        currentCoinPos = currentCoinUI.GetComponent<RectTransform>();
        totalCoinPos = totalCoinUI.GetComponent<RectTransform>();

        totalCoinPos.localPosition = resultTCPos;
        currentCoinPos.localPosition = resultCCPos;

        //アクティブ状態を変更
        currentCoinUI.SetActive(true);
        totalCoinUI.SetActive(true);
        StageNumUI.SetActive(false);
    }

    //ステージプレイ中時のUI設定
    public void SetPlayingUI()
    {

        RectTransform currentCoinPos;
        RectTransform totalCoinPos;
        //RectTransform StageUIPos;

        //移動するUIを取得
        currentCoinUI = GameObject.Find("CoinUICurrent").transform.GetChild(0).gameObject;
        totalCoinUI = GameObject.Find("CoinUITotal").transform.GetChild(0).gameObject;
        StageNumUI = GameObject.Find("stageCanvas");

        currentCoinUI.SetActive(true);
        totalCoinUI.SetActive(true);
        //StageNumUI.SetActive(true);

        currentCoinPos = currentCoinUI.GetComponent<RectTransform>();
        totalCoinPos = totalCoinUI.GetComponent<RectTransform>();
        //StageUIPos = StageNumUI.GetComponent<RectTransform>();


        currentCoinPos.localPosition = playingCCPos;
        //StageUIPos.localPosition = new Vector3(-437f, 838f, 0);

        //アクティブ状態を変更
        currentCoinUI.SetActive(true);
        totalCoinUI.SetActive(false);
        StageNumUI.SetActive(true);
    }

    //スキンショップに行く時のUI設定
    public void SetSkinShopUI()
    {
        RectTransform totalCoinPos;
        //表示・非表示UIを取得
        TitleUI = GameObject.Find("TitleCanvas").GetComponent<Canvas>();
        GameStartUI = GameObject.Find("GameStartCanvas").GetComponent<Canvas>();
        totalCoinUI = GameObject.Find("CoinUITotal");
        StageNumUI = GameObject.Find("stageCanvas");

        TitleCamera = GameObject.Find("TitleCamera").GetComponent<Camera>();
        GameStartCamera = GameObject.Find("GameStartCamera").GetComponent<Camera>();

        totalCoinPos = totalCoinUI.GetComponent<RectTransform>();
        totalCoinPos.localPosition = skinShopTCPos;

        TitleUI.renderMode = RenderMode.ScreenSpaceCamera;
        TitleUI.worldCamera = TitleCamera;
        //GameStartUI.renderMode = RenderMode.ScreenSpaceCamera;
        //GameStartUI.worldCamera = GameStartCamera;
        StageNumUI.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;

        //アクティブ状態を変更
        //TitleUI.SetActive(false);
        //GameStartUI.SetActive(false);
        //totalCoinUI.SetActive(false);
        //StageNumUI.SetActive(false);
    }

    //スキンショップから戻る時のUI設定
    public void RemoveSkinShopUI()
    {
        TitleUI.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        GameStartUI.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        StageNumUI.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        //アクティブ状態を変更
        //TitleUI.SetActive(true);
        //GameStartUI.SetActive(true);
        //totalCoinUI.SetActive(true);
        StageNumUI.SetActive(true);
    }
}

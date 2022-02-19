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

    //�^�C�g������UI�ݒ�
    public void SetTitleUI()
    {

        RectTransform currentCoinPos;
        RectTransform totalCoinPos;

        //�ړ�����UI���擾
        currentCoinUI = GameObject.Find("CoinUICurrent").transform.GetChild(0).gameObject;
        totalCoinUI = GameObject.Find("CoinUITotal").transform.GetChild(0).gameObject;
        StageNumUI = GameObject.Find("stageCanvas");

        StageNumUI.SetActive(true);

        currentCoinPos = currentCoinUI.GetComponent<RectTransform>();
        totalCoinPos = totalCoinUI.GetComponent<RectTransform>();

        totalCoinPos.localPosition = titleTCPos;



        //�A�N�e�B�u��Ԃ�ύX
        currentCoinUI.SetActive(false);
        totalCoinUI.SetActive(true);
        StageNumUI.SetActive(true);

        
    }

    //���U���g����UI�ݒ�
    public void SetResultUI()
    {

        RectTransform currentCoinPos;
        RectTransform totalCoinPos;

        //�ړ�����UI���擾
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

        //�A�N�e�B�u��Ԃ�ύX
        currentCoinUI.SetActive(true);
        totalCoinUI.SetActive(true);
        StageNumUI.SetActive(false);
    }

    //�X�e�[�W�v���C������UI�ݒ�
    public void SetPlayingUI()
    {

        RectTransform currentCoinPos;
        RectTransform totalCoinPos;
        //RectTransform StageUIPos;

        //�ړ�����UI���擾
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

        //�A�N�e�B�u��Ԃ�ύX
        currentCoinUI.SetActive(true);
        totalCoinUI.SetActive(false);
        StageNumUI.SetActive(true);
    }

    //�X�L���V���b�v�ɍs������UI�ݒ�
    public void SetSkinShopUI()
    {
        RectTransform totalCoinPos;
        //�\���E��\��UI���擾
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

        //�A�N�e�B�u��Ԃ�ύX
        //TitleUI.SetActive(false);
        //GameStartUI.SetActive(false);
        //totalCoinUI.SetActive(false);
        //StageNumUI.SetActive(false);
    }

    //�X�L���V���b�v����߂鎞��UI�ݒ�
    public void RemoveSkinShopUI()
    {
        TitleUI.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        GameStartUI.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        StageNumUI.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        //�A�N�e�B�u��Ԃ�ύX
        //TitleUI.SetActive(true);
        //GameStartUI.SetActive(true);
        //totalCoinUI.SetActive(true);
        StageNumUI.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class stageSystem : MonoBehaviour
{

    bool stagePlayingFlag;//�X�e�[�W���쒆���ǂ���
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

        //�V�[���̌Ăяo��
        SceneManager.LoadScene("GameStart", LoadSceneMode.Additive);

        uiControll = GameObject.Find("GameSystem").GetComponent<UIControll>();

        bgm = GameObject.Find("BGM").GetComponent<BGM>();

        if (!GameSystem.gameStartFlag)
        {
            Coin = GameObject.Find("CoinEvent").GetComponent<coin>();
            Coin.LoadCoin();

            //�^�C�g��UI�Z�b�g
            uiControll.SetTitleUI();
            
        }
        else
        {

            Coin.LoadCoin();
            //Coin.Result(0);

            //�v���C��UI�Z�b�g
            uiControll.SetPlayingUI();
        }


        Application.targetFrameRate = 60;


        stageNumImage = GameObject.Find("startNumImage").GetComponent<Image>();
        SetStageNum();

    }

    // Update is called once per frame
    void Update()
    {
        ////��ʂ��^�b�`������J�n
        //if (!stageClearFlag && !stagePlayingFlag && touchSystem.GetTouchFlag())
        //{
        //
        //    stagePlayingFlag = true;
        //    Invoke("StageStart", 0.05f);
        //    //StageStart();
        //}

        //�P��ڂ���Ȃ���Ή�ʃ^�b�`�X�N���v�g�𓮂���
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

        //�X�^�[�g�V�[���폜
        SceneManager.UnloadSceneAsync("GameStart");

        //�^�C�g���폜
        if (!GameSystem.gameStartFlag)
        {
            //SceneManager.UnloadSceneAsync("Title");
            GameSystem.gameStartFlag = true;
          
        }

        //�v���C��UI�Z�b�g
        uiControll.SetPlayingUI();

        //�v���C���[�̓]���鉹�n��
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

        //�V�[���̌Ăяo��
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

    //�X�e�[�W�I��������
    public void StageEnd()
    {

        //���X�e�[�W�Ŏ擾�����R�C�����g�[�^���ɔ��f����
        //Coin.Result();

        stagePlayingFlag = false;

        //�V�[���̌Ăяo��
        SceneManager.LoadScene("GameResult", LoadSceneMode.Additive);

        //���U���gUI�Z�b�g
        uiControll.SetResultUI();

        bgm.Mute(true);

    }

    public void AdDelete()
    {
        //�L���V�[���폜
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

        //��ʂ��^�b�`������J�n
        //if (!stageClearFlag && !stagePlayingFlag && touchSystem.GetTouchFlag())
        //{
            Debug.Log("chit");
            stagePlayingFlag = true;
            Invoke("StageStart", 0.05f);
            //StageStart();
        //}

    }


}

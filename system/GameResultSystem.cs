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
    [SerializeField] GameObject scatterCoinEffect;//�R�C�����΂�܂��G�t�F�N�g
    Canvas canvas;

    [SerializeField] int coinSwitchFrame;
    int coinSwitchFrameCnt;
    int frameSwCoinNum;//1�t���[���Ɍ��炷�R�C���̗�
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

        //�R�C���֌W�I�u�W�F�N�g���擾
        Coin = GameObject.Find("CoinEvent").GetComponent<coin>();

        scatterCoinEffect = (GameObject)Resources.Load("Effect/Coin rain");

        //�J�������w��
        canvas = GameObject.Find("ResultCanvas").GetComponent<Canvas>();
        canvas.worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();



        //1�t���[���Ɍ��炷�R�C���̗ʐ���

        //�L�������ɂ��Ă�����̂ł͂��̏��������Ȃ�
        if (coinSw)
        {

            frameSwCoinNum = Coin.GetnCurrent() / coinSwitchFrame;
            if (frameSwCoinNum <= 0) frameSwCoinNum = 1;
            coinSwitchFrameCnt = coinSwitchFrame;
            //����炷
            //GetComponent<AudioSource>().Play();
            AS.PlayOneShot(resultSE[(int)RESULT_SE.RESULT_COIN]);
        }

        //�{�[�i�X�R�C���̐��Z�o
        bonusCoinNum = Coin.GetnCurrent();

    }

    // Update is called once per frame
    void Update()
    {
        if(coinSw) CoinSwitch();
    }

    public void LoadCoinBonusAdScene()
    {

        //�R�C���{�[�i�X�pad���Ăяo��

        //�V�[��(�L���Ăяo��)
        SceneManager.LoadScene("bonusAd", LoadSceneMode.Additive);

        //ad�V�X�e����ǂݍ���
        //adsys = GameObject.Find("AdSystem").GetComponent<AdSystem>();

        //�{�[�i�X���������s
        //adsys.CoinBonus();

        
    }

    //���݂̃R�C����݌v�R�C���Ɉړ�
    //ad�̖߂�{�^��
    public void currentCointoTotalCoin()
    {

        //���l���ړ�
        Coin.Result();

        //���o
        Camera mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();

        //UI���W�ňʒu���w�肵�A��������[���h���W�ɕϊ����Đ�������
        Vector2 screenPos = new Vector2(540,1900);
        Vector3 world_position = mainCamera.ScreenToWorldPoint(screenPos);

        //�R�C�����΂�܂����o
        GameObject obj = Instantiate(scatterCoinEffect,
            world_position,
            Quaternion.Euler(-90f, 0f, 0f));

        obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, -5);

        //obj.GetComponent<ScatterCoin>().BurstNum(100);

        //�{�[�i�X�{�^��������
        Destroy(GameObject.Find("BonusButton"));

        //�{�[�i�X�{�^���̈ʒu�ɑ������R�C���̗ʂ��o��
        //�ۗ�
        screenPos = new Vector2(0, 1920/2 - 270);

        GameObject prb = Instantiate(DispBonusCoin,
            new Vector3(0,0,0),
            Quaternion.identity);

        //prb.transform.SetParent(GameObject.Find("ResultCanvas").transform);
        //prb.transform.position = screenPos;

        prb.GetComponent<GetCoinNum>().Display(bonusCoinNum);

        //�R�C���������炷
        GameObject.Find("GameResultSystem").GetComponent<AudioSource>().Play();

        AudioSource rAS = GameObject.Find("GameResultSystem").GetComponent<AudioSource>();

        rAS.PlayOneShot(resultSE[(int)RESULT_SE.BONUS_COIN]);

        //�g�[�^���R�C�����Z�[�u
        Coin.Save();

        //�L���V�[�������
        SceneManager.UnloadSceneAsync("bonusAd");




    }

    //���݂̃R�C�����g�[�^���R�C���Ɉڂ�
    void CoinSwitch()
    {

        //�t���[�����ƂɃJ�����g�R�C�������炵�A���v�R�C���𑝂₷
        if (coinSwitchFrameCnt > 0)
        {


            //�ړ�
            if(Coin.GetnCurrent() >= frameSwCoinNum) Coin.Result(frameSwCoinNum);

            coinSwitchFrameCnt--;
            if (coinSwitchFrameCnt <= 0)
            {
                GetComponent<AudioSource>().Stop();

                //�[�����c���Ă�����ړ�
                Coin.Result();

                //�g�[�^���R�C�����Z�[�u
                Coin.Save();

            }
        }




    }

    public int GetBonusCoinNum()
    {
        return bonusCoinNum;
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameSystem : MonoBehaviour
{

    [SerializeField] static public int currentStageNum;
    static public bool gameStartFlag = false;
    coin Coin;

    UIControll uiControll;

    static public int stageMax;

    [SerializeField] int setStageMax;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);

        stageMax = setStageMax;

        //�Z�[�u�f�[�^�ǂݍ���
        StageClearDataLoad();

        //�f�o�b�O�p
        debugSystem ds = GameObject.Find("DebugSystem").GetComponent<debugSystem>();
        if (ds.IsUse())
        {
            currentStageNum = ds.GetStageNum();
        }
        //�K�v�ȃV�[�����Ăяo��

        ////���C���V�[���X�e�[�W
        //��ŃN���A�����X�e�[�W�̎�����n�܂�悤�ɂ���
        SceneManager.LoadScene("stage" + currentStageNum);

        //�ǉ��^�C�g��
        SceneManager.LoadScene("Title", LoadSceneMode.Additive);


        //Debug.Break();

        //Application.targetFrameRate = 60;

    }

    // Update is called once per frame
    void Update()
    {
        //�f�o�b�O
        if (Input.GetKeyDown(KeyCode.R))
        {
            StageClearDataReset();
        }

    }

    static void StageClearDataSave()
    {

        PlayerPrefs.SetInt("StageClearData", currentStageNum);
        PlayerPrefs.Save();

    }


    void StageClearDataLoad()
    {
        currentStageNum = PlayerPrefs.GetInt("StageClearData", 1);
        Debug.Log(currentStageNum);
    }


    void StageClearDataReset()
    {
        PlayerPrefs.SetInt("StageClearData", 1);
        PlayerPrefs.Save();
    }


    static public void ToNextStage()
    {

        currentStageNum++;

        if (currentStageNum > stageMax)
        {
            currentStageNum = 1;
        }

        //�f�[�^�̃Z�[�u
        StageClearDataSave();

        

    }

}

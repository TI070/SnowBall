using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skinSystem : MonoBehaviour
{

    public enum SKIN_KIND
    {
        STANDARD,
        ONE_BALL,
        TWO_BALL,
        THREE_BALL,
        FOUR_BALL,
        FIVE_BALL,
        SIX_BALL,
        SEVEN_BALL,
        EIGHT_BALL,
        NINE_BALL,
        TEN_BALL,
        ELEVEN_BALL,
        TWELVE_BALL,
        THIRTEEN_BALL,
        FOURTEEN_BALL,
        FIFTEEN_BALL,
        HALF1_BALL,
        HALF2_BALL,
        HALF3_BALL,
        HALF4_BALL,
        SKINKIND_MAX
    }

    public enum SKINGET_STATUS
    {
        LOCK,
        UNLOCK,
        SELECT,
        STATUS_MAX
    }


    [SerializeField] Material[] skinList = new Material[(int)SKIN_KIND.SKINKIND_MAX];
    [SerializeField] int[] skinPrice = new int[(int)SKIN_KIND.SKINKIND_MAX];

    int[] skinGetStatus = new int[(int)SKIN_KIND.SKINKIND_MAX];

    SKIN_KIND playerSkin;
    player Player;

    // Start is called before the first frame update
    void Start()
    {


        //�I�𒆃X�L����ǂݍ���
        //playerSkin = (SKIN_KIND)PlayerPrefs.GetInt("selectSkin", 0); ;

        //�P�ڂ͕K���J���ς�
        skinGetStatus[0] = (int)SKINGET_STATUS.UNLOCK;

        bool c = true;

        //�X�L���w���f�[�^�̃��[�h
        for (int i = 1;i < (int)SKIN_KIND.SKINKIND_MAX; i++) {
            skinGetStatus[i] = PlayerPrefs.GetInt("skinBuyFlag" + i, 0);

            //�P���I�𒆂���Ȃ������珉����ʂ�I�𒆂ɂ���
            if(skinGetStatus[i] == (int)SKINGET_STATUS.SELECT)
            {

                //�I�𒆃X�L����ǂݍ���
                playerSkin = (SKIN_KIND)i;


                c = false;
            }

        }

        if (c) skinGetStatus[0] = (int)SKINGET_STATUS.SELECT;

    }



    // Update is called once per frame
    void Update()
    {

        //�f�o�b�O
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetPlayerSkin(SKIN_KIND.ONE_BALL);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetPlayerSkin(SKIN_KIND.TWO_BALL);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetPlayerSkin(SKIN_KIND.THREE_BALL);
        }


    }

    public void SetPlayerSkin(SKIN_KIND s_skind)
    {
        playerSkin = s_skind;
        PlayerMaterialSet();
    }

    public void SetPlayer(player s_p)
    {
        Player = s_p;
    }

    public void PlayerMaterialSet()
    {

        Player.GetComponent<Renderer>().sharedMaterial = skinList[(int)playerSkin];

    }


    public int GetSkinPrice(SKIN_KIND s_skind)
    {
        return skinPrice[(int)s_skind];
    }

    public int GetSkinGetStatus(int index)
    {
        return skinGetStatus[index];
    }

    public void SetSkinGetStatus(int index,SKINGET_STATUS s_sgStatus)
    {
        skinGetStatus[index] = (int)s_sgStatus;

        if (s_sgStatus != SKINGET_STATUS.SELECT)
        {

            //�X�L����Ԃ��Z�[�u
            SkinDataSave();

            return;
        }

        //�X�V��Ԃ�SELECT�̏ꍇ�A���̑I�𒆏�Ԃ̂��̂�UNLOCK�ɕύX����
        for (int i = 0; i < (int)SKIN_KIND.SKINKIND_MAX; i++)
        {

            if (i == index) continue;

            if(skinGetStatus[i] == (int)SKINGET_STATUS.SELECT)
            {

                skinGetStatus[i] = (int)SKINGET_STATUS.UNLOCK;
                return;

            }

        }

        //�X�L����Ԃ��Z�[�u
        SkinDataSave();

    }


    public Material GetSkinMaterial(SKIN_KIND s_sKind)
    {

        return skinList[(int)s_sKind];

    }


    public int GetPlayerSkin()
    {
        return (int)playerSkin;
    }

    public void SkinDataSave()
    {



        for (int i = 0; i < (int)SKIN_KIND.SKINKIND_MAX; i++)
        {
            Debug.Log("sgs = " + skinGetStatus[i]);
            PlayerPrefs.SetInt("skinBuyFlag" + i, skinGetStatus[i]);


        }

        PlayerPrefs.Save();

    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdSystem : MonoBehaviour
{

    public enum AD_KIND{

        AD_COINBONUS,
        AD_CONTINUE

    }

    [SerializeField] AD_KIND ADKind;

    [SerializeField] Button rButton;
    [SerializeField] GameResultSystem resultSystem;
    [SerializeField] GameoverSystem gameoverSystem;

    // Start is called before the first frame update
    void Start()
    {
        //�V�[���ɂ���ĕω�
        if(ADKind == AD_KIND.AD_COINBONUS)
        {
            CoinBonus();
            ReturnButtonSet(ADKind);
        }
        else if (ADKind == AD_KIND.AD_CONTINUE)
        {

            ReturnButtonSet(ADKind);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetAdKind(AD_KIND s_adKind)
    {

        ADKind = s_adKind;
    }

    public void CoinBonus()
    {

        //�R�C���֌W�I�u�W�F�N�g���擾
        coin Coin = GameObject.Find("CoinEvent").GetComponent<coin>();

        //�����@��ŏC��
        Coin.Add(GameObject.Find("GameResultSystem").GetComponent<GameResultSystem>().GetBonusCoinNum());

        //


    }

    public void ReturnButtonSet(AD_KIND s_adKind)
    {
        

        //���^�[���{�^���̎擾
        rButton = GameObject.Find("ReturnButton").GetComponent<Button>();

        if (s_adKind == AD_KIND.AD_COINBONUS) rButton.onClick.AddListener(resultSystem.currentCointoTotalCoin);
        else if (s_adKind == AD_KIND.AD_CONTINUE) rButton.onClick.AddListener(gameoverSystem.PlayerContinue);


    }


    void DeleteBonusButton()
    {

        //�L���Đ��{�^���������āA



    }




}

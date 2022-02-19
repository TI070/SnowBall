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
        //シーンによって変化
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

        //コイン関係オブジェクトを取得
        coin Coin = GameObject.Find("CoinEvent").GetComponent<coin>();

        //増加　後で修正
        Coin.Add(GameObject.Find("GameResultSystem").GetComponent<GameResultSystem>().GetBonusCoinNum());

        //


    }

    public void ReturnButtonSet(AD_KIND s_adKind)
    {
        

        //リターンボタンの取得
        rButton = GameObject.Find("ReturnButton").GetComponent<Button>();

        if (s_adKind == AD_KIND.AD_COINBONUS) rButton.onClick.AddListener(resultSystem.currentCointoTotalCoin);
        else if (s_adKind == AD_KIND.AD_CONTINUE) rButton.onClick.AddListener(gameoverSystem.PlayerContinue);


    }


    void DeleteBonusButton()
    {

        //広告再生ボタンを消して、



    }




}

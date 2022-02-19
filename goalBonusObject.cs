using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class goalBonusObject : MonoBehaviour
{

    [SerializeField] float endurance;
    [SerializeField] Sprite BreakSprite;
    [SerializeField] int BonusCoin;
    [SerializeField] Text bonusNumText;
    [SerializeField] int goalNum;

    // Start is called before the first frame update
    void Start()
    {
        bonusNumText.text = "" + BonusCoin;

        //耐久値を計算で出す
        player Player = GameObject.Find("player").GetComponent<player>();
        endurance = Player.transform.localScale.x * 40 / 9 / 10 * goalNum;

        endurance -= 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //破壊に成功したらtrueを返す
    public bool ObjectBreak(float PlayerScale)
    {

        //耐久値を超えていたら処理する
        if (PlayerScale <= endurance) return false;

        //エフェクトを生成



        //自分を破壊(スプライトの変更)
        GetComponent<SpriteRenderer>().sprite = BreakSprite;

        return true;

    }
    
    public int GetBonusCoin()
    {
        return BonusCoin;
    }

    public int GetGoalNum()
    {
        return goalNum;
    }

}

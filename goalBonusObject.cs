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

        //�ϋv�l���v�Z�ŏo��
        player Player = GameObject.Find("player").GetComponent<player>();
        endurance = Player.transform.localScale.x * 40 / 9 / 10 * goalNum;

        endurance -= 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //�j��ɐ���������true��Ԃ�
    public bool ObjectBreak(float PlayerScale)
    {

        //�ϋv�l�𒴂��Ă����珈������
        if (PlayerScale <= endurance) return false;

        //�G�t�F�N�g�𐶐�



        //������j��(�X�v���C�g�̕ύX)
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

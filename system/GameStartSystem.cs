using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStartSystem : MonoBehaviour
{

    Image hand;
    Image yajirushi;

    bool rightSw;

    [SerializeField] float moveSpeed;

    [SerializeField] float hand_rightmax;
    [SerializeField] float hand_leftmax;

    [SerializeField] float startPosX;

    [SerializeField] float offsetX;

    // Start is called before the first frame update
    void Start()
    {

        //ÉCÉÅÅ[ÉWéÊìæ
        hand = GameObject.Find("hand").GetComponent<Image>();
        yajirushi = GameObject.Find("yajirushi").GetComponent<Image>();

        rightSw = true;

        startPosX = hand.rectTransform.anchoredPosition.x;

        hand_rightmax =  yajirushi.rectTransform.sizeDelta.x / 2 + startPosX;
        hand_leftmax = -yajirushi.rectTransform.sizeDelta.x / 2 + startPosX;

        

    }

    // Update is called once per frame
    void Update()
    {

        MovingHand();

    }


    //éËÇìÆÇ©Ç∑
    void MovingHand()
    {


        if (rightSw)
        {

            hand.transform.Translate(new Vector3(moveSpeed,0,0));
            if (hand.rectTransform.anchoredPosition.x > hand_rightmax + offsetX) rightSw = false;

        }
        else
        {

            hand.transform.Translate(new Vector3(-moveSpeed, 0, 0));
            if (hand.rectTransform.anchoredPosition.x < hand_leftmax + offsetX) rightSw = true;

        }
    }


    public void EventTrigger_SetGameStart()
    {

        stageSystem sS = GameObject.Find("StageSystem").GetComponent<stageSystem>();
        sS.EventTrigger_GameStart();

    }


}

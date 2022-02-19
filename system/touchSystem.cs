using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class touchSystem : MonoBehaviour
{

    static bool touchFlag;
    static Vector3 touchPos;
    static TouchPhase touchPhase;


    static bool eventThrough_touchFlag;
    static Vector3 eventThrough_touchPos;
    static TouchPhase eventThrough_touchPhase;

    static public int touchLimitRight;
    static public int touchLimitUp;

    [SerializeField] int setTouchLimitRight;
    [SerializeField] int setTouchLimitUp;

    // Start is called before the first frame update
    void Start()
    {

        DontDestroyOnLoad(this.gameObject);

        touchPhase = TouchPhase.Canceled;
        touchFlag = false;

        eventThrough_touchPhase = TouchPhase.Canceled;
        eventThrough_touchFlag = false;

        touchLimitUp = setTouchLimitRight = 1920;
        touchLimitRight = setTouchLimitRight = 1080;

    }

    // Update is called once per frame
    void Update()
    {

        //�O�t���[���Ƀ^�b�`���O������������ꍇ�ێ����Ă���^�b�`�ʒu��������
        if (touchPhase == TouchPhase.Ended) touchPos = Vector3.zero;


        touchFlag = false;
        touchPhase = TouchPhase.Canceled;

        eventThrough_touchPhase = TouchPhase.Canceled;
        eventThrough_touchFlag = false;

        // �G�f�B�^
        if (Application.isEditor)
        {

            //UI�̏�ł��������������
            EventThroughEdit();

            //UI�̏�ɍڂ��Ă���Ƃ��A�^�b�`���������Ȃ�
            if (EventSystem.current.IsPointerOverGameObject())
            {
                //�}�E�X����̏ꍇ
                return;
            }


            // �������u��
            if (Input.GetMouseButtonDown(0))
            {

                touchFlag = true;
                touchPhase = TouchPhase.Began;

            }

            // �������u��
            else if (Input.GetMouseButtonUp(0))
            {
                touchFlag = true;
                touchPhase = TouchPhase.Ended;


            }

            // �������ςȂ�
            else if (Input.GetMouseButton(0))
            {
                touchFlag = true;
                touchPhase = TouchPhase.Stationary;

            }

            // ���W�擾
            if (touchFlag)
            {
                Vector3 pastPos = touchPos;

                touchPos = Input.mousePosition;

                //�h���b�O�̔���
                if (pastPos != touchPos && touchPhase != TouchPhase.Began && touchPhase != TouchPhase.Ended) touchPhase = TouchPhase.Moved;
            }


            // �[��
        }
        else
        {

            EventThrough_NotEdit();

            if (Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                //���@�ł̃^�b�v����̏ꍇ
                return;
            }

            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                touchPos = touch.position;
                touchPhase = touch.phase;
                touchFlag = true;
            }

            ////
            // �������u��
            if (Input.GetMouseButtonDown(0))
            {
                touchFlag = true;
                touchPhase = TouchPhase.Began;

            }

            // �������u��
            else if (Input.GetMouseButtonUp(0))
            {
                touchFlag = true;
                touchPhase = TouchPhase.Ended;


            }

            // �������ςȂ�
            else if (Input.GetMouseButton(0))
            {
                touchFlag = true;
                touchPhase = TouchPhase.Stationary;

            }

            // ���W�擾
            if (touchFlag)
            {
                Vector3 pastPos = touchPos;

                touchPos = Input.mousePosition;

                //�h���b�O�̔���
                if (pastPos != touchPos && touchPhase != TouchPhase.Began && touchPhase != TouchPhase.Ended) touchPhase = TouchPhase.Moved;
            }
            ////
        }
    }

    static public bool GetTouchFlag()
    {

        //��ʊO���^�b�`���Ă�����false
        if (!CheckTouchPosInScreen()) return false;

        return touchFlag;
    }

    static public Vector3 GetTouchPos()
    {
        ////�␳
        //if (touchLimitRight < touchPos.x) touchPos.x = touchLimitRight;
        //
        //Debug.Log("limitR = " + touchLimitRight);
        //Debug.Log("posX = " + touchPos.x);


        return touchPos;
    }

    static public TouchPhase GetTouchPhase()
    {
        return touchPhase;
    }

    static public bool CheckTouchDown()
    {
        if (touchPhase == TouchPhase.Began) return true;
        else return false;
    }


    static public Vector3 GetWorldTouchPos()
    {

        return Camera.main.ScreenToWorldPoint(touchPos);
    }

    void EventThroughEdit()
    {
        // �������u��
        if (Input.GetMouseButtonDown(0))
        {

            eventThrough_touchFlag = true;
            eventThrough_touchPhase = TouchPhase.Began;

        }

        // �������u��
        else if (Input.GetMouseButtonUp(0))
        {
            eventThrough_touchFlag = true;
            eventThrough_touchPhase = TouchPhase.Ended;


        }

        // �������ςȂ�
        else if (Input.GetMouseButton(0))
        {
            eventThrough_touchFlag = true;
            eventThrough_touchPhase = TouchPhase.Stationary;

        }

        // ���W�擾
        if (eventThrough_touchFlag)
        {
            Vector3 pastPos = touchPos;

            eventThrough_touchPos = Input.mousePosition;

            //�h���b�O�̔���
            if (pastPos != eventThrough_touchPos && eventThrough_touchPhase != TouchPhase.Began && eventThrough_touchPhase != TouchPhase.Ended) eventThrough_touchPhase = TouchPhase.Moved;
        }
    }

    void EventThrough_NotEdit()
    {

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            eventThrough_touchPos = touch.position;
            eventThrough_touchPhase = touch.phase;
            eventThrough_touchFlag = true;
        }

        ////
        // �������u��
        if (Input.GetMouseButtonDown(0))
        {
            eventThrough_touchFlag = true;
            eventThrough_touchPhase = TouchPhase.Began;

        }

        // �������u��
        else if (Input.GetMouseButtonUp(0))
        {
            eventThrough_touchFlag = true;
            eventThrough_touchPhase = TouchPhase.Ended;


        }

        // �������ςȂ�
        else if (Input.GetMouseButton(0))
        {
            eventThrough_touchFlag = true;
            eventThrough_touchPhase = TouchPhase.Stationary;

        }

        // ���W�擾
        if (eventThrough_touchFlag)
        {
            Vector3 pastPos = eventThrough_touchPos;

            eventThrough_touchPos = Input.mousePosition;

            //�h���b�O�̔���
            if (pastPos != eventThrough_touchPos && eventThrough_touchPhase != TouchPhase.Began && eventThrough_touchPhase != TouchPhase.Ended) eventThrough_touchPhase = TouchPhase.Moved;
        }
    }


    static public bool GetEventThrough_TouchFlag()
    {
        return eventThrough_touchFlag;
    }

    static public Vector3 GetEventThrough_TouchPos()
    {
        return eventThrough_touchPos;
    }

    static public TouchPhase GetEventThrough_TouchPhase()
    {
        return eventThrough_touchPhase;
    }

    //�^�b�`�ꏊ���X�N���[���̒��ɓ����Ă�����true��Ԃ�
    static public bool CheckTouchPosInScreen()
    {

        //Debug.Log("touchPos = " + touchPos);
        //Debug.Log("R_Limit = " + touchLimitRight);
        //Debug.Log("U_Limit = " + touchLimitUp);

        //�␳
        if (touchLimitRight < touchPos.x) return false;
        else if (0 > touchPos.x) return false;
        else if (touchLimitUp < touchPos.y) return false;
        else if (0 > touchPos.y) return false;

        return true;
    }



}


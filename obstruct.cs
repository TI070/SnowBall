using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstruct : MonoBehaviour
{

    public enum OBSTRUCT_TYPE {
        BREAK,
        ABSORPTION
    }

    public enum OBSTRUCT_SIZE
    {
        SMALL,
        MIDDLE,
        LARGE
    }

    public enum OBSTRUCT_MOVE
    {

        MOVE_NONE,
        MOVE_HORIZON
    }


    [SerializeField] OBSTRUCT_TYPE obstructType;
    [SerializeField] OBSTRUCT_SIZE obstructSize;
    [SerializeField] float addScale;
    player Player;

    int blinkCount;
    [SerializeField] int blinkCountMax;
    bool blinkSw = false;
    [SerializeField] Color blinkColor;

    [SerializeField] OBSTRUCT_MOVE obstructMove;

    [SerializeField] float moveSpeed;
    [SerializeField] float moveDistance;

    float startPosX;
    float moveCount;

    bool rightMoveSw;
    [SerializeField] float moveRightMax;
    [SerializeField] float moveLeftMax;

    [SerializeField] float scaleLowerLimit;

    bool breakFlag;

    [SerializeField] Sprite BreakSprite;

    // 破壊可能演出
    // グラデーションのポジションを管理する値 0f〜1f
    float currentPointColor = 0.0f;
    Color startColor = new Color(1f, 1f, 1f, 1f);
    [SerializeField] Color goalColor = new Color(1f, 0.58f, 0.58f, 1f);
    public float Value = 0.02f;

    [SerializeField] bool isYukiotoko;

    // Start is called before the first frame update
    void Start()
    {

        Player = GameObject.Find("player").GetComponent<player>();
        blinkCount = 0;

        startPosX = transform.position.x;

        breakFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!breakFlag && CheckThisSize())
        {
            BreakAbleEffect();
        }
        else
        {
            UnBreakAbleEffect();
        }

        if (obstructMove == OBSTRUCT_MOVE.MOVE_HORIZON)
        {
            moveHorizon();
            moveCount += moveSpeed;
        }


    }

    public OBSTRUCT_TYPE GetObstructType()
    {
        return obstructType;
    }

    //破壊したときの雪玉の拡大量
    public float GetAddScale()
    {
        return addScale;
    }

    //自分の大きさが破壊可能かをチェック 可能ならtrueを返す
    bool CheckThisSize()
    {

        //吸収アイテムなら無条件で破壊可能
        if (obstructType == OBSTRUCT_TYPE.ABSORPTION) return true;

        int checkN = (int)Player.GetPlayerSize() - (int)obstructSize;

        //if (Player.GetBounds() < GetComponent<CircleCollider2D>().bounds.size.x) return false;
        if (checkN <= 0) return false;
        else return true;

    }

    //破壊可能演出
    void BreakAbleEffect()
    {
        if (blinkSw)
        {
            currentPointColor += Value;
            if(currentPointColor > 1.0f)
            {
                currentPointColor = 1.0f;
                blinkSw = !blinkSw;
            }
        }
        else
        {
            currentPointColor -= Value;
            if (currentPointColor < 0.0f)
            {
                currentPointColor = 0.0f;
                blinkSw = !blinkSw;
            }
        }

        float r = (goalColor.r - startColor.r) * currentPointColor + startColor.r;
        float g = (goalColor.g - startColor.g) * currentPointColor + startColor.g;
        float b = (goalColor.b - startColor.b) * currentPointColor + startColor.b;
        GetComponent<SpriteRenderer>().color = new Color(r, g, b, 1f);
    }

    void UnBreakAbleEffect()
    {
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
    }


    public OBSTRUCT_SIZE GetObstructSize()
    {
        return obstructSize;
    }

    void moveHorizon()
    {

        //transform.position = new Vector3(startPosX + moveDistance * Mathf.Sin(moveCount), transform.position.y, transform.position.z);

        if(rightMoveSw)
        {
            transform.Translate(new Vector3(moveSpeed,0,0));
            if (moveRightMax < transform.position.x) rightMoveSw = false;
        }
        else
        {
            transform.Translate(new Vector3(-moveSpeed, 0, 0));
            if (moveLeftMax > transform.position.x) rightMoveSw = true;

        }

    }

    public float GetLowerLimit()
    {
        return scaleLowerLimit;
    }


    public void ObstructBreak()
    {

        ////破壊演出
        //スプライトの変更
        GetComponent<SpriteRenderer>().sprite = BreakSprite;

        //当たり判定を消す
        GetComponent<Collider2D>().enabled = false;


        //色をもとに戻す
        GetComponent<SpriteRenderer>().color = new Color(1, 2, 1, 1);

        //破壊フラグ
        breakFlag = true;

    }

    public bool GetBreakFlag()
    {
        return breakFlag;
    }

    public void SnowBreak()
    {

        //破壊フラグ
        breakFlag = true;

        Destroy(this.gameObject);

    }

    public void YukiotokoBreak()
    {

        //破壊フラグ
        breakFlag = true;

        Destroy(this.gameObject);

    }


    public bool CheckIsYukiotoko()
    {
        return isYukiotoko;
    }


}

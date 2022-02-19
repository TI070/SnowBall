using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{

    enum DAMAGE_TYPE{
        CONSTANT,
        DISTANCE,
        RATIO
    }

    public enum PLAYERSIZE_STATE
    {
        SMALL,
        MIDDLE,
        LARGE,
        MAX
    }

    public enum PLAYER_SE
    {
        ROLLING_SNOWBALL,
        GET_SNOW,
        SNOWBALL_BREAK,
        GET_COIN,
        WALL_BREAK,
        TREE_BREAK,
        WALLBREAK_ADD,
        //SKIN_UNLOCK,
        //SKINICON_TOUCH,
        //GAMEOVER,
        //RESULT,
        //BONUS_COIN,
        LASTWALL_BREAK,
        SNOWBALL_BREAK_FORWALL,
        PLAYERSE_MAX
    }

    Vector3 position;
    Vector3 scale;

    [SerializeField] float moveSpeed_X;
    [SerializeField] float moveSpeed_Y;
    float startMoveSpeed_Y;

    [SerializeField] float rotateSpeed;
    [SerializeField] float moveAddScale;

    float startRotateSpeed;

    [SerializeField] coin Coin;
    [SerializeField] gage Gauge;

    [SerializeField] float damageCorrect;

    [SerializeField] DAMAGE_TYPE DamageType;

    stageSystem StageSystem;
    GameObject SnowTrace;
    int traceCount;
    [SerializeField] int traceCountMax;

    [SerializeField] float deathScale;

    GameObject stageObjParent;

    Vector3 stageObjParentScale;

    [SerializeField] Vector3 scaleMax;
    Vector3 startScale;
    Vector3 pastScale;
    Vector3 stageObjParentTargetScale;
    Vector3 stageObjParentPastScale;
    Vector3 stageObjParentStartScale;

    [SerializeField] float stageObjScale_SetTime;

    float toMiddlePlusScale;//ミドル状態になるまでのaddScale
    float toLargePlusScale;//Large状態になるまでのaddScale
    float toMaxPlusScale;//Max状態になるまでのaddScale

    float NeedtoMaxScaleNum;//初期値から限界値になるまでに必要なscaleの値

    float MiddleScaleLine;
    float LargeScaleLine;
    float MaxScaleLine;

    [SerializeField] float speedY_AddCorrect;
    [SerializeField] float rotateY_AddCorrect;


    GameObject[] leavesEffect = new GameObject[3];//葉が散るエフェクト
    GameObject scatterCoinEffect;//コインをばらまくエフェクト
    GameObject iceShardEffect;//氷の破片エフェクト
    GameObject snowExplosion;//死亡時雪爆散エフェクト

    [SerializeField] float traceSizeCorrect;
    [SerializeField] float traceOffset_Y;

    GameObject[] obstructArray;

    [SerializeField] List<GameObject> obstructList = new List<GameObject>();
    List<Vector3> obstructListTargetScale = new List<Vector3>();
    List<Vector3> obstructListStartScale = new List<Vector3>();

    AudioSource AS;

    [SerializeField] AudioClip[] playerSE = new AudioClip[(int)PLAYER_SE.PLAYERSE_MAX];

    [SerializeField] float PlayerMoveThreshold;

    [SerializeField] float reductionRate;

    GameObject blowedTree;
    GameObject blowedYukiotoko;

    [SerializeField] float blowSpeed;//ふっとばし速度
    [SerializeField] float blowrollSpeed;//吹っ飛ばした時の回転速度

    PLAYERSIZE_STATE currentSizeState;
    PLAYERSIZE_STATE pastSizeState;

    [SerializeField] float deathAnimeSecond;

    skinSystem SkinSystem;

    [SerializeField] float ratioDamageCorrect;

    [SerializeField] AudioClip[] iceBreakSE = new AudioClip[10];
    [SerializeField] AudioClip[] iceBreakAddSE = new AudioClip[10];

    [SerializeField] float limitRight;
    [SerializeField] float limitLeft;

    // Start is called before the first frame update
    void Start()
    {
        //初期化
        position = transform.position;
        scale = transform.localScale;

        StageSystem = GameObject.Find("StageSystem").GetComponent<stageSystem>();
        SnowTrace = (GameObject)Resources.Load("SnowTrace");

        //stageObjParent = GameObject.Find("stageObj");
        //stageObjParentScale = stageObjParent.transform.localScale;

        startScale = scale;

        stageObjParentPastScale = stageObjParentScale;
        stageObjParentTargetScale = stageObjParentScale;
        stageObjParentStartScale = stageObjParentScale;

        //現在のサイズから最大サイズを設定する
        scaleMax = startScale * 40 / 9;


        NeedtoMaxScaleNum = scaleMax.x - startScale.x;
        //stagesystemから状態ごとのaddScaleを計算する
        toMiddlePlusScale = NeedtoMaxScaleNum * StageSystem.GetPlayerMiddlePlusScale();
        toLargePlusScale = NeedtoMaxScaleNum * StageSystem.GetPlayerLargePlusScale();
        toMaxPlusScale = NeedtoMaxScaleNum * StageSystem.GetPlayerMaxPlusScale();


        //大きさの閾値を決める
        MiddleScaleLine = startScale.x + (scaleMax.x - startScale.x) / 3;
        LargeScaleLine = startScale.x + (scaleMax.x - startScale.x) * 2 / 3;
        MaxScaleLine = startScale.x + (scaleMax.x - startScale.x);

        startMoveSpeed_Y = moveSpeed_Y;
        startRotateSpeed = rotateSpeed;

        //エフェクトの読み込み
        leavesEffect[0] = (GameObject)Resources.Load("Effect/Tree0");
        leavesEffect[1] = (GameObject)Resources.Load("Effect/Tree1");
        leavesEffect[2] = (GameObject)Resources.Load("Effect/Tree2");

        scatterCoinEffect = (GameObject)Resources.Load("Effect/ScatterCoin");
        iceShardEffect = (GameObject)Resources.Load("Effect/Ice shards");
        snowExplosion = (GameObject)Resources.Load("Effect/Explosion2");

        //障害物リストの取得
        GetObstructArray();

        //音
        AS = GetComponent<AudioSource>();

        blowedTree = (GameObject)Resources.Load("blowedTree");
        blowedYukiotoko = (GameObject)Resources.Load("blowedYukiotoko");

        Coin = GameObject.Find("CoinEvent").GetComponent<coin>();

        currentSizeState = pastSizeState = PLAYERSIZE_STATE.SMALL;

        SkinSystem = GameObject.Find("GameSystem").GetComponent<skinSystem>();
        SkinSystem.SetPlayer(this);

        SkinSystem.PlayerMaterialSet();

    }

    // Update is called once per frame
    void Update()
    {

        //ゲームオーバーなら動作しない
        if (StageSystem.GetStageGameoverFlag()) return;

        //ステージ動作中でなければ動作しない
        if (!StageSystem.GetStagePlayingFlag()) return;

        //移動
        PlayerMove();

        //回転(見た目だけ)
        PlayerRotate();

        ////演出
        //跡をつける
        if (traceCount >= traceCountMax)
        {
            MakeTrace();
            traceCount = 0;
        }
        else traceCount++;

        //生存判定
        LifeCheck();

        //オブジェクトの大きさを変える
        //SetObjParentShrink();

        //大きさゲージ変更
        ScaleGaugeSet();

        //大きさによるスピードの変動
        SpeedChangeforScale();

        //大きさによる回転速度
        //RotateSpeedChangeforSpeed();

        //デバッグ
        if (Input.GetKeyDown(KeyCode.Z))
        {

        }


    }


    void PlayerMove()
    {

        if (touchSystem.GetTouchFlag() && MoveAbleCheck())
        {
            //プレイヤーよりタッチ位置が右なら右へ移動
            if (position.x < touchSystem.GetWorldTouchPos().x)
            {

                if(CheckMoveAble(position.x + moveSpeed_X)) position.x += moveSpeed_X;

            }
            //タッチ位置が左なら左へ移動
            else if (position.x > touchSystem.GetWorldTouchPos().x)
            {

                if (CheckMoveAble(position.x - moveSpeed_X)) position.x -= moveSpeed_X;

            }
        }


        position.y += moveSpeed_Y;



        //実位置の更新
        transform.position = position;


    }

    void PlayerRotate()
    {

        transform.Rotate(new Vector3(rotateSpeed, 0, 0));

    }


    public void AddPlayerScale(float s_Scale)
    {

        scale.x += s_Scale;
        scale.y += s_Scale;
        scale.z += s_Scale;

        //上限を超えていたら補正する
        if (scale.x > scaleMax.x)
        {
            scale = scaleMax;

        }

        //過去の障害物サイズを記憶
        //stageObjParentPastScale = stageObjParentScale;

        //障害物のサイズに反映
        //float sc = (scale.x / startScale.x) - 1;
        //sc = stageObjParentStartScale.x - sc;
        //stageObjParentTargetScale = new Vector3(sc,sc,sc);

        //int cnt = 0;
        //
        //foreach (GameObject obs in obstructList)
        //{
        //
        //    //障害物のサイズに反映
        //    float sc = (scale.x / startScale.x);
        //    if (sc <= 0) sc = 0.1f;
        //    sc = obstructListStartScale[cnt].x / sc * reductionRate;
        //    obstructListTargetScale[cnt] = new Vector3(sc, sc, sc);
        //
        //    cnt++;
        //}

        //Debug.Log("scale = " + scale.x);
        //Debug.Log("MiddleScaleLine = " + MiddleScaleLine);

        //プレイヤー大きさ状態セット
        //自分の大きさに応じて加算割合を変える
        if (scale.x < MiddleScaleLine - 0.1f)
        {
            currentSizeState = PLAYERSIZE_STATE.SMALL;        
        }
        else if (scale.x < LargeScaleLine - 0.1f)
        {
            currentSizeState = PLAYERSIZE_STATE.MIDDLE;

        }
        else if(scale.x < MaxScaleLine - 0.1f)
        {
            currentSizeState = PLAYERSIZE_STATE.LARGE;
        }
        else
        {
            currentSizeState = PLAYERSIZE_STATE.MAX;
        }

        //プレイヤーの大きさ状態が前回から変わっているかチェック
        if (currentSizeState != pastSizeState)
        {

            //変わっていたらその段階に応じて拡縮する
            AddObstructScale();


        }

        pastSizeState = currentSizeState;

        transform.localScale = scale;
    }

    void AddObstructScale()
    {
        //凍結
        return;

        int cnt = 0;

        foreach (GameObject obs in obstructList)
        {

            //障害物のサイズに反映
            float sc = (scale.x / startScale.x);
            if (sc <= 0) sc = 0.1f;
            sc = obstructListStartScale[cnt].x / sc * reductionRate;
            obstructListTargetScale[cnt] = new Vector3(sc, sc, sc);

            cnt++;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //障害物
        if (collision.transform.tag == "obstruct")
        {

            //障害物の大きさによってプレイヤーの大きさを変える
            float playerHitSize = GetComponent<CircleCollider2D>().bounds.size.x;
            float obsHitSize = collision.GetComponent<CircleCollider2D>().bounds.size.x;

            //振動
            //Vibration vibration = GetComponent<Vibration>();

            //Debug.Log("hit");

            //破壊済みのオブジェクトの場合処理をしない
            if (collision.GetComponent<obstruct>().GetBreakFlag()) return;


            //プレイヤーの方が大きい もしくは吸収アイテム（雪玉）
            if (CheckBlowObstruct((int)collision.GetComponent<obstruct>().GetObstructSize()) || collision.GetComponent<obstruct>().GetObstructType() == obstruct.OBSTRUCT_TYPE.ABSORPTION)
            {


                float addScale = collision.GetComponent<obstruct>().GetAddScale();
                //scale.x += addScale;
                //scale.y += addScale;
                //scale.z += addScale;

                //障害物の場合
                if (collision.GetComponent<obstruct>().GetObstructType() == obstruct.OBSTRUCT_TYPE.BREAK)
                {

                    int rand = Random.Range(0, 3);

                    //雪男でないかチェック
                    if (!collision.GetComponent<obstruct>().CheckIsYukiotoko())
                    {

                        //破壊エフェクト
                        GameObject obj = Instantiate(leavesEffect[rand],
                            collision.transform.position,
                            Quaternion.identity);


                        //障害物の大きさに合わせてスケールの変更
                        obstruct.OBSTRUCT_SIZE obsSize = collision.GetComponent<obstruct>().GetObstructSize();

                        obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, -5);


                        if (obsSize == obstruct.OBSTRUCT_SIZE.SMALL)
                        {
                            obj.transform.localScale *= 1.0f;
                        }
                        else if (obsSize == obstruct.OBSTRUCT_SIZE.MIDDLE)
                        {
                            obj.transform.localScale *= 2.0f;
                        }
                        else
                        {
                            obj.transform.localScale *= 3.0f;
                        }

                        //吹き飛ばし
                        BlowObstruct(collision.transform.position, collision.transform.localScale.x);


                    }
                    else
                    {
                        collision.GetComponent<obstruct>().YukiotokoBreak();

                        //吹き飛ばし
                        BlowYukiotoko(collision.transform.position, collision.transform.localScale.x);

                    }

                    //木破壊音
                    AS.PlayOneShot(playerSE[(int)PLAYER_SE.TREE_BREAK]);

                    //振動
                    //Vibration.Vibrate(100);
                }
                else //吸収アイテム(雪玉)の場合
                {

                    //自分の大きさに応じて加算割合を変える
                    if (scale.x < MiddleScaleLine - 0.1f)
                    {

                        //１個当たりの加算scaleを出す
                        addScale = (scaleMax.x - startScale.x) / 3 / StageSystem.GetPlayerMiddleScaleNum();

                    }
                    else if (scale.x < LargeScaleLine - 0.1f)
                    {
                        //１個当たりの加算scaleを出す
                        addScale = (scaleMax.x - startScale.x) / 3 / StageSystem.GetPlayerLargeScaleNum();

                    }
                    else
                    {
                        //１個当たりの加算scaleを出す
                        addScale = (scaleMax.x - startScale.x) / 3 / StageSystem.GetPlayerMaxScaleNum();

                    }

                    //Debug.Log("scale = " + addScale);

                    AddPlayerScale(addScale);

                    //雪玉を取った時の音
                    AS.PlayOneShot(playerSE[(int)PLAYER_SE.GET_SNOW]);


                    //雪玉を消す
                    collision.GetComponent<obstruct>().SnowBreak();

                    //振動
                    //Vibration.Vibrate(100);
                }




            }
            else//そうでもない
            {

                //ダメージ（拡縮）処理
                if (DamageType == DAMAGE_TYPE.CONSTANT)
                {
                    ConstantDamage();
                }
                else if (DamageType == DAMAGE_TYPE.DISTANCE)
                {
                    DistanceDamage(collision.transform.position, collision.GetComponent<CircleCollider2D>().bounds.size.x);
                }
                else if (DamageType == DAMAGE_TYPE.RATIO)
                {
                    RationDamage();
                }


                //雪男でないかチェック
                if (!collision.GetComponent<obstruct>().CheckIsYukiotoko())
                {

                    int rand = Random.Range(0, 3);

                    //破壊エフェクト
                    GameObject obj = Instantiate(leavesEffect[rand],
                        collision.transform.position,
                        Quaternion.identity);


                    //障害物の大きさに合わせてスケールの変更
                    obstruct.OBSTRUCT_SIZE obsSize = collision.GetComponent<obstruct>().GetObstructSize();

                    obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, -5);

                    if (obsSize == obstruct.OBSTRUCT_SIZE.SMALL)
                    {
                        obj.transform.localScale *= 1.0f;
                    }
                    else if (obsSize == obstruct.OBSTRUCT_SIZE.MIDDLE)
                    {
                        obj.transform.localScale *= 2.0f;
                    }
                    else
                    {
                        obj.transform.localScale *= 3.0f;
                    }

                }

                //仮数値
                //scale.x -= 0.5f * 5;
                //scale.y -= 0.5f * 5;
                //scale.z -= 0.5f * 5;

                //雪玉ダメージ時の音
                AS.PlayOneShot(playerSE[(int)PLAYER_SE.SNOWBALL_BREAK]);

                //振動
                //Vibration.Vibrate(50);
            }


            //実スケールの更新
            transform.localScale = scale;

            ////障害物破壊処理
            //Destroy(collision.gameObject);
            collision.GetComponent<obstruct>().ObstructBreak();



            //リストから消す
            //obstructListStartScale.RemoveAt(obstructList.IndexOf(collision.gameObject));
            //obstructListTargetScale.RemoveAt(obstructList.IndexOf(collision.gameObject));
            //obstructList.Remove(collision.gameObject);
            
            //



        }

        //コイン
        if (collision.transform.tag == "coin")
        {
            Coin.Create(collision.gameObject);

            Destroy(collision.gameObject);

            //コイン取得音
            AS.PlayOneShot(playerSE[(int)PLAYER_SE.GET_COIN]);

            //振動
            //Vibration.Vibrate(100);
        }

        //ゴール関連
        if(collision.transform.tag == "goalBonusObject")
        {
           

            //クリアフラグを立てる
            StageSystem.StageClear();

            //破壊の処理
            bool breakSuccess;

            breakSuccess = collision.GetComponent<goalBonusObject>().ObjectBreak(transform.localScale.x);

            //破壊できなかったら止まる
            if (!breakSuccess)
            {
                moveSpeed_Y = 0;
                rotateSpeed = 0;
                //StageSystem.StageEnd();
                StartCoroutine(DeathEffectandLoadScene("result"));

                //トータルコインをセーブ
                //Coin.Save();

                //壁で雪が壊れる音
                AS.PlayOneShot(playerSE[(int)PLAYER_SE.SNOWBALL_BREAK_FORWALL]);


                //雪玉を非表示にする
                GetComponent<MeshRenderer>().enabled = false;

                //転がる音を止める
                //PlayRollingSound(false);
                Invoke("StopRollSE", 1.0f);

                //壁破壊音
                //AS.PlayOneShot(iceBreakSE[collision.GetComponent<goalBonusObject>().GetGoalNum() - 1]);


            }
            else //できたらボーナス
            {
                Coin.Add(collision.GetComponent<goalBonusObject>().GetBonusCoin());

                //コインをばらまく演出
                GameObject obj = Instantiate(scatterCoinEffect,
                    collision.transform.position,
                    Quaternion.Euler(-90f, 0f, 0f));

                obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, -5);

                obj.GetComponent<ScatterCoin>().BurstNum(collision.GetComponent<goalBonusObject>().GetBonusCoin());

                //氷の破片が飛び散る演出
                GameObject iceObj = Instantiate(iceShardEffect,
                    collision.transform.position,
                    Quaternion.Euler(-90f, 0f, 0f));

                iceObj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, -7);

                iceObj.transform.localScale *= 5;


                //壁破壊音
                AS.PlayOneShot(iceBreakSE[collision.GetComponent<goalBonusObject>().GetGoalNum() - 1]);
                AS.PlayOneShot(iceBreakAddSE[collision.GetComponent<goalBonusObject>().GetGoalNum() - 1]);

                //振動
                //Vibration.Vibrate(500);

                //割ったやつが１０枚目でも止まる
                if (collision.GetComponent<goalBonusObject>().GetGoalNum() == 10)
                {

                    //10枚目の音
                    AS.PlayOneShot(playerSE[(int)PLAYER_SE.LASTWALL_BREAK]);


                    moveSpeed_Y = 0;
                    rotateSpeed = 0;
                    //StageSystem.StageEnd();

                    //トータルコインをセーブ
                    //Coin.Save();

                    //壁で雪が壊れる音
                    AS.PlayOneShot(playerSE[(int)PLAYER_SE.SNOWBALL_BREAK_FORWALL]);

                    //振動
                    //Vibration.Vibrate(1000);

                    //転がる音を止める
                    //PlayRollingSound(false);
                    Invoke("StopRollSE", 1.0f);

                    GetComponent<MeshRenderer>().enabled = false;

                    StartCoroutine(DeathEffectandLoadScene("result"));


                }

            }


            //ゴールオブジェクト破壊
            Destroy(collision.gameObject);

        }


    }

    void ConstantDamage()
    {

        //障害物からパラメータ取得したほうがいい気がする
        //今はしていない
        float damage = damageCorrect * 1;


        //仮数値
        //scale.x -= damage;
        //scale.y -= damage;
        //scale.z -= damage;

        AddPlayerScale(-damage);
    }

    void DistanceDamage(Vector2 targetPos,float targetColRange)
    {

        //Debug.Log("tarCR =" + targetColRange);

        //衝突位置に依存したダメージ
        //0.1～1.0に分布

        //最大は0
        float damageMag;
        float dis;
        float disMax;

        //Debug.Log("bouns =" + GetComponent<CircleCollider2D>().bounds.size.x);

        disMax = targetColRange / 2 + GetComponent<CircleCollider2D>().bounds.size.x / 2;
        dis = transform.position.x - targetPos.x;

        dis = Mathf.Abs(dis);

        damageMag = 1 - (dis / disMax);
        

        float damage = damageMag * damageCorrect;

        AddPlayerScale(-damage);

    }

    //跡をつける
    void MakeTrace()
    {
        GameObject obj = Instantiate(SnowTrace, transform.position, Quaternion.identity);
        obj.transform.Translate(new Vector3(0, traceOffset_Y,0));

        obj.GetComponent<traceObject>().SetPlayer(this);

        //跡の大きさをプレイヤーに合わせて変える
        obj.transform.localScale *= (scale.x / startScale.x) * traceSizeCorrect;

    }


    void LifeCheck()
    {

        //大きさが基準値を下回ったらゲームオーバー
        if(transform.localScale.x < deathScale)
        {

            transform.localScale = Vector3.zero;

            moveSpeed_Y = 0;
            rotateSpeed = 0;

            //移動音ストップ
            PlayRollingSound(false);

            //StageSystem.StageGameover();
            StartCoroutine(DeathEffectandLoadScene("gameover"));

        }



    }


    void SetObjParentShrink()
    {

        int cnt = 0;

        foreach (GameObject obs in obstructList)
        {

            if (obs.transform.position.x != obstructListTargetScale[cnt].x) 
            {

                obs.transform.localScale = Vector3.Lerp(obstructList[cnt].transform.localScale, obstructListTargetScale[cnt], 1 / stageObjScale_SetTime);

                //障害物の大きさの下限を下回っていたら、下限まで戻す
                if(obs.transform.localScale.x < obs.GetComponent<obstruct>().GetLowerLimit())
                {
                    float lim = obs.GetComponent<obstruct>().GetLowerLimit();
                    obs.transform.localScale = new Vector3(lim,lim,lim);
                }

                //stageObjParent.transform.localScale = stageObjParentScale;
            }
            cnt++;
        }


        //変更のゴールを設定し、そこへ徐々に近づくようにする
        //if (stageObjParentScale.x != stageObjParentTargetScale.x) {
        //
        //    stageObjParentScale = Vector3.Lerp(stageObjParentScale, stageObjParentTargetScale, 1/ stageObjScale_SetTime);
        //
        //    stageObjParent.transform.localScale = stageObjParentScale;
        //}



        //自分の大きさによって変える
        //float sc = (scale.x / startScale.x) - 1;
        //sc = stageObjParentScale.x - sc;
        //
        //stageObjParent.transform.localScale = new Vector3(sc, sc, sc);
    }


    void ScaleGaugeSet()
    {

        //凍結
        return;

        Gauge.CalcGage(scale.x, scaleMax.x);

    }


    public float GetBounds()
    {
        return GetComponent<CircleCollider2D>().bounds.size.x;
    }

    //大きさによるスピードの変動
    void SpeedChangeforScale()
    {

        //クリア時は処理しない
        if (StageSystem.GetStageClearFlag()) return;

        float sc = (scale.x / startScale.x) - 1;

        moveSpeed_Y = startMoveSpeed_Y + sc * startMoveSpeed_Y * speedY_AddCorrect;

        //初速よりは遅くならないようにする
        if (moveSpeed_Y < startMoveSpeed_Y) moveSpeed_Y = startMoveSpeed_Y;

        //moveSpeed_X = moveSpeed_Y;

    }

    //スピードによる回転速度の変更
    void RotateSpeedChangeforSpeed()
    {

        //クリア時は処理しない
        if (StageSystem.GetStageClearFlag()) return;

        float sc = (moveSpeed_Y / startMoveSpeed_Y) - 1;

        rotateSpeed = startRotateSpeed + sc * startRotateSpeed * rotateY_AddCorrect;



    }

    void GetObstructArray()
    {

        obstructArray = GameObject.FindGameObjectsWithTag("obstruct");
        Debug.Log(obstructArray.Length);

        for (int i = 0;i < obstructArray.Length;i++)
        {
            obstructList.Add(obstructArray[i]);
            obstructListStartScale.Add(obstructArray[i].transform.localScale);
            obstructListTargetScale.Add(obstructArray[i].transform.localScale);

        }
    }

    public void PlaySE(PLAYER_SE s_pse)
    {
        AS.PlayOneShot(playerSE[(int)s_pse]);
    }

    public void PlayRollingSound(bool sw)
    {
        if (sw) AS.Play();
        else AS.Stop();
    }

    bool MoveAbleCheck()
    {
        //閾値を下回っていたらfalse
        if (Mathf.Abs(touchSystem.GetWorldTouchPos().x - transform.position.x) < PlayerMoveThreshold)
        {
            return false;
        }

        return true;
    }

    public float GetPlayerScale()
    {
        return scale.x;
    }

    public float GetPlayerScaleMax()
    {
        return scaleMax.x;
    }


    void BlowObstruct(Vector3 s_targetPos, float s_scale)
    {

        //飛ばすベクトルの計算
        Vector2 vec;
        vec = (s_targetPos - transform.position).normalized;
        vec *= blowSpeed;
        //飛ぶ木の生成
        GameObject obj = Instantiate(blowedTree,
            s_targetPos,
            Quaternion.identity);

        obj.GetComponent<blowedObstruct>().SetBlowRollSpeed(blowrollSpeed);
        obj.GetComponent<blowedObstruct>().SetBlowSpeed(blowSpeed);
        obj.GetComponent<blowedObstruct>().SetBlowVector(vec);

        obj.transform.localScale *= s_scale; 


    }

    void BlowYukiotoko(Vector3 s_targetPos, float s_scale)
    {

        //飛ばすベクトルの計算
        Vector2 vec;
        vec = (s_targetPos - transform.position).normalized;
        vec *= blowSpeed;
        //飛ぶ木の生成
        GameObject obj = Instantiate(blowedYukiotoko,
            s_targetPos,
            Quaternion.identity);

        obj.GetComponent<blowedObstruct>().SetBlowRollSpeed(blowrollSpeed);
        obj.GetComponent<blowedObstruct>().SetBlowSpeed(blowSpeed);
        obj.GetComponent<blowedObstruct>().SetBlowVector(vec);

        obj.transform.localScale *= s_scale;


    }

    //障害物を吹き飛ばせるかの判定 吹き飛ばせたらtrueを返す
    bool CheckBlowObstruct(int obsSize)
    {

        int checkN = (int)currentSizeState - obsSize;

        //プレイヤーのほうが大きければtrueを返す
        if (checkN > 0)
        {
            return true;
        }


        return false;
    }

    public PLAYERSIZE_STATE GetPlayerSize()
    {
        return currentSizeState;
    }


    IEnumerator DeathEffectandLoadScene(string s_sceneName)
    {

        //爆散エフェクト
        GameObject obj = Instantiate(snowExplosion,
            transform.position,
            Quaternion.identity);

        if (s_sceneName == "gameover")
        {
            StageSystem.StageGameoverFlagOn();

        }

        yield return new WaitForSeconds(deathAnimeSecond);

        if(s_sceneName == "gameover")
        {
           StageSystem.StageGameover();

        }
        else if (s_sceneName == "result")
        {
            StageSystem.StageEnd();

        }


    }

    public void SetDefaultRollSpeed()
    {
        rotateSpeed = startRotateSpeed;
        
    }

    void RationDamage()
    {

        float damage = 0;

        if (currentSizeState == PLAYERSIZE_STATE.SMALL)
        {
            damage = (scaleMax.x - startScale.x) / 3 / StageSystem.GetPlayerMiddleScaleNum();
        }
        else if (currentSizeState == PLAYERSIZE_STATE.MIDDLE)
        {
            damage = (scaleMax.x - startScale.x) / 3 / StageSystem.GetPlayerLargeScaleNum();

        }
        else if (currentSizeState == PLAYERSIZE_STATE.LARGE)
        {
            damage = (scaleMax.x - startScale.x) / 3 / StageSystem.GetPlayerMaxScaleNum();
        }
        else
        {
            damage = (scaleMax.x - startScale.x) / 3 / StageSystem.GetPlayerMaxScaleNum();
        }

        damage *= ratioDamageCorrect;

        AddPlayerScale(-damage);

    }

    public void StopRollSE()
    {

        AS.Stop();
    }

    //移動可能ならtrueを返す
    public bool CheckMoveAble(float posX)
    {
        if (posX > limitRight) return false;
        else if (posX < limitLeft) return false;

        return true;
    }



}




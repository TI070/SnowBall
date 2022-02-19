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

    float toMiddlePlusScale;//�~�h����ԂɂȂ�܂ł�addScale
    float toLargePlusScale;//Large��ԂɂȂ�܂ł�addScale
    float toMaxPlusScale;//Max��ԂɂȂ�܂ł�addScale

    float NeedtoMaxScaleNum;//�����l������E�l�ɂȂ�܂łɕK�v��scale�̒l

    float MiddleScaleLine;
    float LargeScaleLine;
    float MaxScaleLine;

    [SerializeField] float speedY_AddCorrect;
    [SerializeField] float rotateY_AddCorrect;


    GameObject[] leavesEffect = new GameObject[3];//�t���U��G�t�F�N�g
    GameObject scatterCoinEffect;//�R�C�����΂�܂��G�t�F�N�g
    GameObject iceShardEffect;//�X�̔j�ЃG�t�F�N�g
    GameObject snowExplosion;//���S���ᔚ�U�G�t�F�N�g

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

    [SerializeField] float blowSpeed;//�ӂ��Ƃ΂����x
    [SerializeField] float blowrollSpeed;//������΂������̉�]���x

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
        //������
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

        //���݂̃T�C�Y����ő�T�C�Y��ݒ肷��
        scaleMax = startScale * 40 / 9;


        NeedtoMaxScaleNum = scaleMax.x - startScale.x;
        //stagesystem�����Ԃ��Ƃ�addScale���v�Z����
        toMiddlePlusScale = NeedtoMaxScaleNum * StageSystem.GetPlayerMiddlePlusScale();
        toLargePlusScale = NeedtoMaxScaleNum * StageSystem.GetPlayerLargePlusScale();
        toMaxPlusScale = NeedtoMaxScaleNum * StageSystem.GetPlayerMaxPlusScale();


        //�傫����臒l�����߂�
        MiddleScaleLine = startScale.x + (scaleMax.x - startScale.x) / 3;
        LargeScaleLine = startScale.x + (scaleMax.x - startScale.x) * 2 / 3;
        MaxScaleLine = startScale.x + (scaleMax.x - startScale.x);

        startMoveSpeed_Y = moveSpeed_Y;
        startRotateSpeed = rotateSpeed;

        //�G�t�F�N�g�̓ǂݍ���
        leavesEffect[0] = (GameObject)Resources.Load("Effect/Tree0");
        leavesEffect[1] = (GameObject)Resources.Load("Effect/Tree1");
        leavesEffect[2] = (GameObject)Resources.Load("Effect/Tree2");

        scatterCoinEffect = (GameObject)Resources.Load("Effect/ScatterCoin");
        iceShardEffect = (GameObject)Resources.Load("Effect/Ice shards");
        snowExplosion = (GameObject)Resources.Load("Effect/Explosion2");

        //��Q�����X�g�̎擾
        GetObstructArray();

        //��
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

        //�Q�[���I�[�o�[�Ȃ瓮�삵�Ȃ�
        if (StageSystem.GetStageGameoverFlag()) return;

        //�X�e�[�W���쒆�łȂ���Γ��삵�Ȃ�
        if (!StageSystem.GetStagePlayingFlag()) return;

        //�ړ�
        PlayerMove();

        //��](�����ڂ���)
        PlayerRotate();

        ////���o
        //�Ղ�����
        if (traceCount >= traceCountMax)
        {
            MakeTrace();
            traceCount = 0;
        }
        else traceCount++;

        //��������
        LifeCheck();

        //�I�u�W�F�N�g�̑傫����ς���
        //SetObjParentShrink();

        //�傫���Q�[�W�ύX
        ScaleGaugeSet();

        //�傫���ɂ��X�s�[�h�̕ϓ�
        SpeedChangeforScale();

        //�傫���ɂ���]���x
        //RotateSpeedChangeforSpeed();

        //�f�o�b�O
        if (Input.GetKeyDown(KeyCode.Z))
        {

        }


    }


    void PlayerMove()
    {

        if (touchSystem.GetTouchFlag() && MoveAbleCheck())
        {
            //�v���C���[���^�b�`�ʒu���E�Ȃ�E�ֈړ�
            if (position.x < touchSystem.GetWorldTouchPos().x)
            {

                if(CheckMoveAble(position.x + moveSpeed_X)) position.x += moveSpeed_X;

            }
            //�^�b�`�ʒu�����Ȃ獶�ֈړ�
            else if (position.x > touchSystem.GetWorldTouchPos().x)
            {

                if (CheckMoveAble(position.x - moveSpeed_X)) position.x -= moveSpeed_X;

            }
        }


        position.y += moveSpeed_Y;



        //���ʒu�̍X�V
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

        //����𒴂��Ă�����␳����
        if (scale.x > scaleMax.x)
        {
            scale = scaleMax;

        }

        //�ߋ��̏�Q���T�C�Y���L��
        //stageObjParentPastScale = stageObjParentScale;

        //��Q���̃T�C�Y�ɔ��f
        //float sc = (scale.x / startScale.x) - 1;
        //sc = stageObjParentStartScale.x - sc;
        //stageObjParentTargetScale = new Vector3(sc,sc,sc);

        //int cnt = 0;
        //
        //foreach (GameObject obs in obstructList)
        //{
        //
        //    //��Q���̃T�C�Y�ɔ��f
        //    float sc = (scale.x / startScale.x);
        //    if (sc <= 0) sc = 0.1f;
        //    sc = obstructListStartScale[cnt].x / sc * reductionRate;
        //    obstructListTargetScale[cnt] = new Vector3(sc, sc, sc);
        //
        //    cnt++;
        //}

        //Debug.Log("scale = " + scale.x);
        //Debug.Log("MiddleScaleLine = " + MiddleScaleLine);

        //�v���C���[�傫����ԃZ�b�g
        //�����̑傫���ɉ����ĉ��Z������ς���
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

        //�v���C���[�̑傫����Ԃ��O�񂩂�ς���Ă��邩�`�F�b�N
        if (currentSizeState != pastSizeState)
        {

            //�ς���Ă����炻�̒i�K�ɉ����Ċg�k����
            AddObstructScale();


        }

        pastSizeState = currentSizeState;

        transform.localScale = scale;
    }

    void AddObstructScale()
    {
        //����
        return;

        int cnt = 0;

        foreach (GameObject obs in obstructList)
        {

            //��Q���̃T�C�Y�ɔ��f
            float sc = (scale.x / startScale.x);
            if (sc <= 0) sc = 0.1f;
            sc = obstructListStartScale[cnt].x / sc * reductionRate;
            obstructListTargetScale[cnt] = new Vector3(sc, sc, sc);

            cnt++;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //��Q��
        if (collision.transform.tag == "obstruct")
        {

            //��Q���̑傫���ɂ���ăv���C���[�̑傫����ς���
            float playerHitSize = GetComponent<CircleCollider2D>().bounds.size.x;
            float obsHitSize = collision.GetComponent<CircleCollider2D>().bounds.size.x;

            //�U��
            //Vibration vibration = GetComponent<Vibration>();

            //Debug.Log("hit");

            //�j��ς݂̃I�u�W�F�N�g�̏ꍇ���������Ȃ�
            if (collision.GetComponent<obstruct>().GetBreakFlag()) return;


            //�v���C���[�̕����傫�� �������͋z���A�C�e���i��ʁj
            if (CheckBlowObstruct((int)collision.GetComponent<obstruct>().GetObstructSize()) || collision.GetComponent<obstruct>().GetObstructType() == obstruct.OBSTRUCT_TYPE.ABSORPTION)
            {


                float addScale = collision.GetComponent<obstruct>().GetAddScale();
                //scale.x += addScale;
                //scale.y += addScale;
                //scale.z += addScale;

                //��Q���̏ꍇ
                if (collision.GetComponent<obstruct>().GetObstructType() == obstruct.OBSTRUCT_TYPE.BREAK)
                {

                    int rand = Random.Range(0, 3);

                    //��j�łȂ����`�F�b�N
                    if (!collision.GetComponent<obstruct>().CheckIsYukiotoko())
                    {

                        //�j��G�t�F�N�g
                        GameObject obj = Instantiate(leavesEffect[rand],
                            collision.transform.position,
                            Quaternion.identity);


                        //��Q���̑傫���ɍ��킹�ăX�P�[���̕ύX
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

                        //������΂�
                        BlowObstruct(collision.transform.position, collision.transform.localScale.x);


                    }
                    else
                    {
                        collision.GetComponent<obstruct>().YukiotokoBreak();

                        //������΂�
                        BlowYukiotoko(collision.transform.position, collision.transform.localScale.x);

                    }

                    //�ؔj��
                    AS.PlayOneShot(playerSE[(int)PLAYER_SE.TREE_BREAK]);

                    //�U��
                    //Vibration.Vibrate(100);
                }
                else //�z���A�C�e��(���)�̏ꍇ
                {

                    //�����̑傫���ɉ����ĉ��Z������ς���
                    if (scale.x < MiddleScaleLine - 0.1f)
                    {

                        //�P������̉��Zscale���o��
                        addScale = (scaleMax.x - startScale.x) / 3 / StageSystem.GetPlayerMiddleScaleNum();

                    }
                    else if (scale.x < LargeScaleLine - 0.1f)
                    {
                        //�P������̉��Zscale���o��
                        addScale = (scaleMax.x - startScale.x) / 3 / StageSystem.GetPlayerLargeScaleNum();

                    }
                    else
                    {
                        //�P������̉��Zscale���o��
                        addScale = (scaleMax.x - startScale.x) / 3 / StageSystem.GetPlayerMaxScaleNum();

                    }

                    //Debug.Log("scale = " + addScale);

                    AddPlayerScale(addScale);

                    //��ʂ���������̉�
                    AS.PlayOneShot(playerSE[(int)PLAYER_SE.GET_SNOW]);


                    //��ʂ�����
                    collision.GetComponent<obstruct>().SnowBreak();

                    //�U��
                    //Vibration.Vibrate(100);
                }




            }
            else//�����ł��Ȃ�
            {

                //�_���[�W�i�g�k�j����
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


                //��j�łȂ����`�F�b�N
                if (!collision.GetComponent<obstruct>().CheckIsYukiotoko())
                {

                    int rand = Random.Range(0, 3);

                    //�j��G�t�F�N�g
                    GameObject obj = Instantiate(leavesEffect[rand],
                        collision.transform.position,
                        Quaternion.identity);


                    //��Q���̑傫���ɍ��킹�ăX�P�[���̕ύX
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

                //�����l
                //scale.x -= 0.5f * 5;
                //scale.y -= 0.5f * 5;
                //scale.z -= 0.5f * 5;

                //��ʃ_���[�W���̉�
                AS.PlayOneShot(playerSE[(int)PLAYER_SE.SNOWBALL_BREAK]);

                //�U��
                //Vibration.Vibrate(50);
            }


            //���X�P�[���̍X�V
            transform.localScale = scale;

            ////��Q���j�󏈗�
            //Destroy(collision.gameObject);
            collision.GetComponent<obstruct>().ObstructBreak();



            //���X�g�������
            //obstructListStartScale.RemoveAt(obstructList.IndexOf(collision.gameObject));
            //obstructListTargetScale.RemoveAt(obstructList.IndexOf(collision.gameObject));
            //obstructList.Remove(collision.gameObject);
            
            //



        }

        //�R�C��
        if (collision.transform.tag == "coin")
        {
            Coin.Create(collision.gameObject);

            Destroy(collision.gameObject);

            //�R�C���擾��
            AS.PlayOneShot(playerSE[(int)PLAYER_SE.GET_COIN]);

            //�U��
            //Vibration.Vibrate(100);
        }

        //�S�[���֘A
        if(collision.transform.tag == "goalBonusObject")
        {
           

            //�N���A�t���O�𗧂Ă�
            StageSystem.StageClear();

            //�j��̏���
            bool breakSuccess;

            breakSuccess = collision.GetComponent<goalBonusObject>().ObjectBreak(transform.localScale.x);

            //�j��ł��Ȃ�������~�܂�
            if (!breakSuccess)
            {
                moveSpeed_Y = 0;
                rotateSpeed = 0;
                //StageSystem.StageEnd();
                StartCoroutine(DeathEffectandLoadScene("result"));

                //�g�[�^���R�C�����Z�[�u
                //Coin.Save();

                //�ǂŐႪ���鉹
                AS.PlayOneShot(playerSE[(int)PLAYER_SE.SNOWBALL_BREAK_FORWALL]);


                //��ʂ��\���ɂ���
                GetComponent<MeshRenderer>().enabled = false;

                //�]���鉹���~�߂�
                //PlayRollingSound(false);
                Invoke("StopRollSE", 1.0f);

                //�ǔj��
                //AS.PlayOneShot(iceBreakSE[collision.GetComponent<goalBonusObject>().GetGoalNum() - 1]);


            }
            else //�ł�����{�[�i�X
            {
                Coin.Add(collision.GetComponent<goalBonusObject>().GetBonusCoin());

                //�R�C�����΂�܂����o
                GameObject obj = Instantiate(scatterCoinEffect,
                    collision.transform.position,
                    Quaternion.Euler(-90f, 0f, 0f));

                obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, -5);

                obj.GetComponent<ScatterCoin>().BurstNum(collision.GetComponent<goalBonusObject>().GetBonusCoin());

                //�X�̔j�Ђ���юU�鉉�o
                GameObject iceObj = Instantiate(iceShardEffect,
                    collision.transform.position,
                    Quaternion.Euler(-90f, 0f, 0f));

                iceObj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, -7);

                iceObj.transform.localScale *= 5;


                //�ǔj��
                AS.PlayOneShot(iceBreakSE[collision.GetComponent<goalBonusObject>().GetGoalNum() - 1]);
                AS.PlayOneShot(iceBreakAddSE[collision.GetComponent<goalBonusObject>().GetGoalNum() - 1]);

                //�U��
                //Vibration.Vibrate(500);

                //����������P�O���ڂł��~�܂�
                if (collision.GetComponent<goalBonusObject>().GetGoalNum() == 10)
                {

                    //10���ڂ̉�
                    AS.PlayOneShot(playerSE[(int)PLAYER_SE.LASTWALL_BREAK]);


                    moveSpeed_Y = 0;
                    rotateSpeed = 0;
                    //StageSystem.StageEnd();

                    //�g�[�^���R�C�����Z�[�u
                    //Coin.Save();

                    //�ǂŐႪ���鉹
                    AS.PlayOneShot(playerSE[(int)PLAYER_SE.SNOWBALL_BREAK_FORWALL]);

                    //�U��
                    //Vibration.Vibrate(1000);

                    //�]���鉹���~�߂�
                    //PlayRollingSound(false);
                    Invoke("StopRollSE", 1.0f);

                    GetComponent<MeshRenderer>().enabled = false;

                    StartCoroutine(DeathEffectandLoadScene("result"));


                }

            }


            //�S�[���I�u�W�F�N�g�j��
            Destroy(collision.gameObject);

        }


    }

    void ConstantDamage()
    {

        //��Q������p�����[�^�擾�����ق��������C������
        //���͂��Ă��Ȃ�
        float damage = damageCorrect * 1;


        //�����l
        //scale.x -= damage;
        //scale.y -= damage;
        //scale.z -= damage;

        AddPlayerScale(-damage);
    }

    void DistanceDamage(Vector2 targetPos,float targetColRange)
    {

        //Debug.Log("tarCR =" + targetColRange);

        //�Փˈʒu�Ɉˑ������_���[�W
        //0.1�`1.0�ɕ��z

        //�ő��0
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

    //�Ղ�����
    void MakeTrace()
    {
        GameObject obj = Instantiate(SnowTrace, transform.position, Quaternion.identity);
        obj.transform.Translate(new Vector3(0, traceOffset_Y,0));

        obj.GetComponent<traceObject>().SetPlayer(this);

        //�Ղ̑傫�����v���C���[�ɍ��킹�ĕς���
        obj.transform.localScale *= (scale.x / startScale.x) * traceSizeCorrect;

    }


    void LifeCheck()
    {

        //�傫������l�����������Q�[���I�[�o�[
        if(transform.localScale.x < deathScale)
        {

            transform.localScale = Vector3.zero;

            moveSpeed_Y = 0;
            rotateSpeed = 0;

            //�ړ����X�g�b�v
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

                //��Q���̑傫���̉�����������Ă�����A�����܂Ŗ߂�
                if(obs.transform.localScale.x < obs.GetComponent<obstruct>().GetLowerLimit())
                {
                    float lim = obs.GetComponent<obstruct>().GetLowerLimit();
                    obs.transform.localScale = new Vector3(lim,lim,lim);
                }

                //stageObjParent.transform.localScale = stageObjParentScale;
            }
            cnt++;
        }


        //�ύX�̃S�[����ݒ肵�A�����֏��X�ɋ߂Â��悤�ɂ���
        //if (stageObjParentScale.x != stageObjParentTargetScale.x) {
        //
        //    stageObjParentScale = Vector3.Lerp(stageObjParentScale, stageObjParentTargetScale, 1/ stageObjScale_SetTime);
        //
        //    stageObjParent.transform.localScale = stageObjParentScale;
        //}



        //�����̑傫���ɂ���ĕς���
        //float sc = (scale.x / startScale.x) - 1;
        //sc = stageObjParentScale.x - sc;
        //
        //stageObjParent.transform.localScale = new Vector3(sc, sc, sc);
    }


    void ScaleGaugeSet()
    {

        //����
        return;

        Gauge.CalcGage(scale.x, scaleMax.x);

    }


    public float GetBounds()
    {
        return GetComponent<CircleCollider2D>().bounds.size.x;
    }

    //�傫���ɂ��X�s�[�h�̕ϓ�
    void SpeedChangeforScale()
    {

        //�N���A���͏������Ȃ�
        if (StageSystem.GetStageClearFlag()) return;

        float sc = (scale.x / startScale.x) - 1;

        moveSpeed_Y = startMoveSpeed_Y + sc * startMoveSpeed_Y * speedY_AddCorrect;

        //�������͒x���Ȃ�Ȃ��悤�ɂ���
        if (moveSpeed_Y < startMoveSpeed_Y) moveSpeed_Y = startMoveSpeed_Y;

        //moveSpeed_X = moveSpeed_Y;

    }

    //�X�s�[�h�ɂ���]���x�̕ύX
    void RotateSpeedChangeforSpeed()
    {

        //�N���A���͏������Ȃ�
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
        //臒l��������Ă�����false
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

        //��΂��x�N�g���̌v�Z
        Vector2 vec;
        vec = (s_targetPos - transform.position).normalized;
        vec *= blowSpeed;
        //��Ԗ؂̐���
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

        //��΂��x�N�g���̌v�Z
        Vector2 vec;
        vec = (s_targetPos - transform.position).normalized;
        vec *= blowSpeed;
        //��Ԗ؂̐���
        GameObject obj = Instantiate(blowedYukiotoko,
            s_targetPos,
            Quaternion.identity);

        obj.GetComponent<blowedObstruct>().SetBlowRollSpeed(blowrollSpeed);
        obj.GetComponent<blowedObstruct>().SetBlowSpeed(blowSpeed);
        obj.GetComponent<blowedObstruct>().SetBlowVector(vec);

        obj.transform.localScale *= s_scale;


    }

    //��Q���𐁂���΂��邩�̔��� ������΂�����true��Ԃ�
    bool CheckBlowObstruct(int obsSize)
    {

        int checkN = (int)currentSizeState - obsSize;

        //�v���C���[�̂ق����傫�����true��Ԃ�
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

        //���U�G�t�F�N�g
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

    //�ړ��\�Ȃ�true��Ԃ�
    public bool CheckMoveAble(float posX)
    {
        if (posX > limitRight) return false;
        else if (posX < limitLeft) return false;

        return true;
    }



}




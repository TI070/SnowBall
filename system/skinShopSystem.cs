using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class skinShopSystem : MonoBehaviour
{

    enum PRICE_ICON
    {
        ICON_500,
        ICON_1000
    }

    enum SKINSHOP_SE
    {
        SKIN_SELECT,
        SKIN_UNLOCK
    }

    skinSystem SkinSystem;
    [SerializeField] Button skinBuyButton;
    [SerializeField] GameObject skinBuyCanvas;

    [SerializeField] Image[] PriceIconImage = new Image[4];
    [SerializeField] Sprite[] PriceIconSprite = new Sprite[2];

    [SerializeField] Image BigPriceImage;
    [SerializeField] Sprite[] BigPriceSprite = new Sprite[2];

    coin Coin;

    int currentPage;
    int pageMax;

    [SerializeField] Sprite[] skinBuyButtonAddVisual = new Sprite[(int)skinSystem.SKINGET_STATUS.STATUS_MAX];

    [SerializeField] Image[] skinBuyButtonAddVisualimage = new Image[4];

    bool SkinBuyAble;

    [SerializeField] Sprite[] skinButtonSprites = new Sprite[(int)skinSystem.SKIN_KIND.SKINKIND_MAX];

    [SerializeField] GameObject[] skinButtons = new GameObject[4];

    [SerializeField] GameObject playerView;
        
    Vector3 playerViewRollVec;
    [SerializeField] float pvRollSpeed;

    bool isTouch;

    Vector3 swipeStartPos;

    AudioSource AS;

    [SerializeField] AudioClip[] skinshopSE = new AudioClip[2];

    // Start is called before the first frame update
    void Start()
    {
        SkinSystem = GameObject.Find("GameSystem").GetComponent<skinSystem>();
        Coin = GameObject.Find("CoinEvent").GetComponent<coin>();

        //��
        AS = GetComponent<AudioSource>();



        currentPage = 1;

        //�w����Ԃ̕\��
        SkinBuyButtonVisualSet();

        SkinBuyAble = true;

        pageMax = (int)skinSystem.SKIN_KIND.SKINKIND_MAX / 4;

        PlayerView((skinSystem.SKIN_KIND)SkinSystem.GetPlayerSkin());

        PlayerViewRoll_RandomSet();

        ButtonPageChange(0);

    }

    // Update is called once per frame
    void Update()
    {

        PlayerViewRoll();

        //�X���C�v��t
        PlayerViewRoll_SetSwipe();

    }

    public void SkinBuyButtonSet(int s_sKind)
    {

        //����炷
        AS.PlayOneShot(skinshopSE[(int)SKINSHOP_SE.SKIN_SELECT]);

        //���ɊJ���ς݂��ǂ����m�F
        if (SkinSystem.GetSkinGetStatus(s_sKind) == (int)skinSystem.SKINGET_STATUS.LOCK)
        {
            //���J���Ȃ�w������

            //�w����ʂ��A�N�e�B�u�ɂ���
            skinBuyCanvas.SetActive(true);

            skinBuyButton.onClick.RemoveAllListeners();

            skinBuyButton.onClick.AddListener(() => SkinBuyButton(s_sKind));

            SkinBuyAble = true;

            //�w����ʂ̒l�i�\����l�i�ɉ����ĕς���
            if (SkinSystem.GetSkinPrice((skinSystem.SKIN_KIND)s_sKind) == 500)
            {
                BigPriceImage.sprite = BigPriceSprite[(int)PRICE_ICON.ICON_500];
            }
            else if (SkinSystem.GetSkinPrice((skinSystem.SKIN_KIND)s_sKind) == 1000)
            {
                BigPriceImage.sprite = BigPriceSprite[(int)PRICE_ICON.ICON_1000];
            }


            //�v���r���[�X�V
            PlayerView((skinSystem.SKIN_KIND)s_sKind);


        }
        else if (SkinSystem.GetSkinGetStatus(s_sKind) == (int)skinSystem.SKINGET_STATUS.UNLOCK)
        {

            //�J���ς݂����I�����Ă��Ȃ���΁A�I����Ԃɂ���

            //�w����Ԃ̍X�V
            SkinSystem.SetSkinGetStatus((int)s_sKind, skinSystem.SKINGET_STATUS.SELECT);

            //�\�����X�V
            SkinBuyButtonVisualSet();

            //�X�L�����Z�b�g����
            SkinSystem.SetPlayerSkin((skinSystem.SKIN_KIND)s_sKind);

            //�v���r���[�X�V
            PlayerView((skinSystem.SKIN_KIND)SkinSystem.GetPlayerSkin());

        }



    }

    //�X�L���w������{�^���Ɋ��蓖�Ă�
    public void SkinBuyButton(int s_sKind)
    {

        Debug.Log("button_num = " + s_sKind);
        Debug.Log("currentPage = " + currentPage);

        if (!SkinBuyAble) return;

        //���z���擾
        int skinPrice = SkinSystem.GetSkinPrice((skinSystem.SKIN_KIND)s_sKind);

        //�w���ł��邩�`�F�b�N        
        if (skinPrice > Coin.GetTotal()) return;

        //�R�C�������炷
        Coin.TotalCoinAdd(-skinPrice);
        Coin.Save();

        //�X�L�����Z�b�g����
        SkinSystem.SetPlayerSkin((skinSystem.SKIN_KIND)s_sKind);

        //�w����Ԃ̍X�V
        SkinSystem.SetSkinGetStatus((int)s_sKind,skinSystem.SKINGET_STATUS.SELECT);

        //�X�L���Z�[�u
        SkinSystem.SkinDataSave();

        //Debug.Log("skinP = " + skinPrice);

        //�\�����X�V
        SkinBuyButtonVisualSet();

        //��\��
        skinBuyCanvas.SetActive(false);

        SkinBuyAble = false;

        //�v���r���[�̍X�V
        PlayerView((skinSystem.SKIN_KIND)SkinSystem.GetPlayerSkin());

        //��
        AS.PlayOneShot(skinshopSE[(int)SKINSHOP_SE.SKIN_UNLOCK]);

    }

    public void CloseSkinBuyCanvas()
    {

        //�v���r���[�߂�
        PlayerView((skinSystem.SKIN_KIND)SkinSystem.GetPlayerSkin());

        skinBuyCanvas.SetActive(false);
    }
    
    public void ButtonPageChange(int num)
    {

        //�y�[�W�̈ړ�
        currentPage += num;
        if (currentPage < 1) currentPage = pageMax;
        else if (currentPage > pageMax) currentPage = 1;

        Debug.Log("pageChange");

        int i_start = (currentPage - 1) * 4;

        for (int i = i_start; i < i_start + 4; i++)
        {
            //�{�^���̃e�N�X�`���X�V
            skinButtons[i % 4].GetComponent<Image>().sprite = skinButtonSprites[i];


            //�{�^���̊֐��X�V
            skinButtons[i % 4].GetComponent<Button>().onClick.RemoveAllListeners();
            int dummy = i;
            skinButtons[i % 4].GetComponent<Button>().onClick.AddListener(() => SkinBuyButtonSet(dummy));


        }

        //�\���X�V
        SkinBuyButtonVisualSet();

    }

    public void DestroyThisScene()
    {

        SceneManager.UnloadSceneAsync("SkinShop");
    }


    //�I�𒆁A�J���ςݓ�������悤�ɂ���
    void SkinBuyButtonVisualSet()
    {

        int i_start = (currentPage - 1) * 4;

        for (int i = i_start ;i < i_start + 4;i++)
        {
            //Debug.Log(SkinSystem.GetSkinGetStatus(i));
            switch (SkinSystem.GetSkinGetStatus(i))
            {

                case (int)skinSystem.SKINGET_STATUS.LOCK:

                    skinBuyButtonAddVisualimage[i % 4].sprite = skinBuyButtonAddVisual[(int)skinSystem.SKINGET_STATUS.LOCK];

                    //�A�N�e�B�u
                    skinBuyButtonAddVisualimage[i % 4].gameObject.SetActive(true);
                    PriceIconImage[i % 4].gameObject.SetActive(true);

                    //�K�v�ȋ��z��\������
                    if (SkinSystem.GetSkinPrice((skinSystem.SKIN_KIND)i) == 500)
                    {
                        PriceIconImage[i % 4].sprite = PriceIconSprite[(int)PRICE_ICON.ICON_500];
                    }
                    else if (SkinSystem.GetSkinPrice((skinSystem.SKIN_KIND)i) == 1000)
                    {
                        PriceIconImage[i % 4].sprite = PriceIconSprite[(int)PRICE_ICON.ICON_1000];
                    }



                    break;

                case (int)skinSystem.SKINGET_STATUS.UNLOCK:


                    //��A�N�e�B�u
                    skinBuyButtonAddVisualimage[i % 4].gameObject.SetActive(false);
                    PriceIconImage[i % 4].gameObject.SetActive(false);

                    break;

                case (int)skinSystem.SKINGET_STATUS.SELECT:

                    skinBuyButtonAddVisualimage[i % 4].sprite = skinBuyButtonAddVisual[(int)skinSystem.SKINGET_STATUS.SELECT];

                    //�A�N�e�B�u
                    skinBuyButtonAddVisualimage[i % 4].gameObject.SetActive(true);

                    //��A�N�e�B�u
                    PriceIconImage[i % 4].gameObject.SetActive(false);

                    break;



            }


        }


    }


    void PlayerView(skinSystem.SKIN_KIND s_sKind)
    {

        //Debug.Log(SkinSystem.GetSkinMaterial(s_sKind));

        playerView.GetComponent<Renderer>().sharedMaterial = SkinSystem.GetSkinMaterial(s_sKind);

    }

    void PlayerViewRoll()
    {

        //Debug.Log(playerViewRollVec);

        playerView.transform.Rotate(playerViewRollVec, Space.World);

    }


    void PlayerViewRoll_RandomSet()
    {
        playerViewRollVec = new Vector3(Random.value, Random.value, Random.value) * pvRollSpeed;
    }

    void PlayerViewRoll_SetSwipe()
    {

        //Debug.Log(touchSystem.GetEventThrough_TouchPhase());
        //Debug.Log(touchSystem.GetEventThrough_TouchFlag());

        //�^�b�`����Ă��Ȃ���Ԃ̏���
        if (isTouch == false)
        {
            //�^�b�`����Ă����珈���J�n
            if (!touchSystem.GetEventThrough_TouchFlag()) return;

            isTouch = true;

            swipeStartPos = touchSystem.GetEventThrough_TouchPos();

        }
        //�^�b�`���Ă����Ԃ̏���
        else if (isTouch == true)
        {
            //�^�b�`�������ꂽ�珈���J�n
            if (touchSystem.GetEventThrough_TouchPhase() != TouchPhase.Ended) return;
            //Debug.Break();
            isTouch = false;

            //Vector3 rollvec = (swipeStartPos - touchSystem.GetEventThrough_TouchPos()).normalized;

            //playerViewRollVec = rollvec * pvRollSpeed;

            float wid = Screen.width;
            float hei = Screen.height;

            float tx = (swipeStartPos.x - touchSystem.GetEventThrough_TouchPos().x) / wid; //���ړ���(-1<tx<1)
            float ty = (swipeStartPos.y - touchSystem.GetEventThrough_TouchPos().y) / hei; //�c�ړ���(-1<ty<1)
            //obj.transform.rotation = sRot;
            //obj.transform.Rotate(new Vector3(90 * ty, -90 * tx, 0), Space.World);

            playerViewRollVec = new Vector3(0, 90 * tx, 90 * ty).normalized * pvRollSpeed;

        }

        
    }

}

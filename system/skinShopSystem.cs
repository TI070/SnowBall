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

        //音
        AS = GetComponent<AudioSource>();



        currentPage = 1;

        //購入状態の表示
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

        //スワイプ受付
        PlayerViewRoll_SetSwipe();

    }

    public void SkinBuyButtonSet(int s_sKind)
    {

        //音を鳴らす
        AS.PlayOneShot(skinshopSE[(int)SKINSHOP_SE.SKIN_SELECT]);

        //既に開放済みかどうか確認
        if (SkinSystem.GetSkinGetStatus(s_sKind) == (int)skinSystem.SKINGET_STATUS.LOCK)
        {
            //未開放なら購入処理

            //購入画面をアクティブにする
            skinBuyCanvas.SetActive(true);

            skinBuyButton.onClick.RemoveAllListeners();

            skinBuyButton.onClick.AddListener(() => SkinBuyButton(s_sKind));

            SkinBuyAble = true;

            //購入画面の値段表示を値段に応じて変える
            if (SkinSystem.GetSkinPrice((skinSystem.SKIN_KIND)s_sKind) == 500)
            {
                BigPriceImage.sprite = BigPriceSprite[(int)PRICE_ICON.ICON_500];
            }
            else if (SkinSystem.GetSkinPrice((skinSystem.SKIN_KIND)s_sKind) == 1000)
            {
                BigPriceImage.sprite = BigPriceSprite[(int)PRICE_ICON.ICON_1000];
            }


            //プレビュー更新
            PlayerView((skinSystem.SKIN_KIND)s_sKind);


        }
        else if (SkinSystem.GetSkinGetStatus(s_sKind) == (int)skinSystem.SKINGET_STATUS.UNLOCK)
        {

            //開放済みだが選択していなければ、選択状態にする

            //購入状態の更新
            SkinSystem.SetSkinGetStatus((int)s_sKind, skinSystem.SKINGET_STATUS.SELECT);

            //表示を更新
            SkinBuyButtonVisualSet();

            //スキンをセットする
            SkinSystem.SetPlayerSkin((skinSystem.SKIN_KIND)s_sKind);

            //プレビュー更新
            PlayerView((skinSystem.SKIN_KIND)SkinSystem.GetPlayerSkin());

        }



    }

    //スキン購入するボタンに割り当てる
    public void SkinBuyButton(int s_sKind)
    {

        Debug.Log("button_num = " + s_sKind);
        Debug.Log("currentPage = " + currentPage);

        if (!SkinBuyAble) return;

        //金額を取得
        int skinPrice = SkinSystem.GetSkinPrice((skinSystem.SKIN_KIND)s_sKind);

        //購入できるかチェック        
        if (skinPrice > Coin.GetTotal()) return;

        //コインを減らす
        Coin.TotalCoinAdd(-skinPrice);
        Coin.Save();

        //スキンをセットする
        SkinSystem.SetPlayerSkin((skinSystem.SKIN_KIND)s_sKind);

        //購入状態の更新
        SkinSystem.SetSkinGetStatus((int)s_sKind,skinSystem.SKINGET_STATUS.SELECT);

        //スキンセーブ
        SkinSystem.SkinDataSave();

        //Debug.Log("skinP = " + skinPrice);

        //表示を更新
        SkinBuyButtonVisualSet();

        //非表示
        skinBuyCanvas.SetActive(false);

        SkinBuyAble = false;

        //プレビューの更新
        PlayerView((skinSystem.SKIN_KIND)SkinSystem.GetPlayerSkin());

        //音
        AS.PlayOneShot(skinshopSE[(int)SKINSHOP_SE.SKIN_UNLOCK]);

    }

    public void CloseSkinBuyCanvas()
    {

        //プレビュー戻す
        PlayerView((skinSystem.SKIN_KIND)SkinSystem.GetPlayerSkin());

        skinBuyCanvas.SetActive(false);
    }
    
    public void ButtonPageChange(int num)
    {

        //ページの移動
        currentPage += num;
        if (currentPage < 1) currentPage = pageMax;
        else if (currentPage > pageMax) currentPage = 1;

        Debug.Log("pageChange");

        int i_start = (currentPage - 1) * 4;

        for (int i = i_start; i < i_start + 4; i++)
        {
            //ボタンのテクスチャ更新
            skinButtons[i % 4].GetComponent<Image>().sprite = skinButtonSprites[i];


            //ボタンの関数更新
            skinButtons[i % 4].GetComponent<Button>().onClick.RemoveAllListeners();
            int dummy = i;
            skinButtons[i % 4].GetComponent<Button>().onClick.AddListener(() => SkinBuyButtonSet(dummy));


        }

        //表示更新
        SkinBuyButtonVisualSet();

    }

    public void DestroyThisScene()
    {

        SceneManager.UnloadSceneAsync("SkinShop");
    }


    //選択中、開放済み等分かるようにする
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

                    //アクティブ
                    skinBuyButtonAddVisualimage[i % 4].gameObject.SetActive(true);
                    PriceIconImage[i % 4].gameObject.SetActive(true);

                    //必要な金額を表示する
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


                    //非アクティブ
                    skinBuyButtonAddVisualimage[i % 4].gameObject.SetActive(false);
                    PriceIconImage[i % 4].gameObject.SetActive(false);

                    break;

                case (int)skinSystem.SKINGET_STATUS.SELECT:

                    skinBuyButtonAddVisualimage[i % 4].sprite = skinBuyButtonAddVisual[(int)skinSystem.SKINGET_STATUS.SELECT];

                    //アクティブ
                    skinBuyButtonAddVisualimage[i % 4].gameObject.SetActive(true);

                    //非アクティブ
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

        //タッチされていない状態の処理
        if (isTouch == false)
        {
            //タッチされていたら処理開始
            if (!touchSystem.GetEventThrough_TouchFlag()) return;

            isTouch = true;

            swipeStartPos = touchSystem.GetEventThrough_TouchPos();

        }
        //タッチしている状態の処理
        else if (isTouch == true)
        {
            //タッチが離されたら処理開始
            if (touchSystem.GetEventThrough_TouchPhase() != TouchPhase.Ended) return;
            //Debug.Break();
            isTouch = false;

            //Vector3 rollvec = (swipeStartPos - touchSystem.GetEventThrough_TouchPos()).normalized;

            //playerViewRollVec = rollvec * pvRollSpeed;

            float wid = Screen.width;
            float hei = Screen.height;

            float tx = (swipeStartPos.x - touchSystem.GetEventThrough_TouchPos().x) / wid; //横移動量(-1<tx<1)
            float ty = (swipeStartPos.y - touchSystem.GetEventThrough_TouchPos().y) / hei; //縦移動量(-1<ty<1)
            //obj.transform.rotation = sRot;
            //obj.transform.Rotate(new Vector3(90 * ty, -90 * tx, 0), Space.World);

            playerViewRollVec = new Vector3(0, 90 * tx, 90 * ty).normalized * pvRollSpeed;

        }

        
    }

}

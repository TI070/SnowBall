using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSystem : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        //if(touchSystem.GetTouchFlag())
        //{
        //    Invoke("DestroyScene", 0.05f);
        //}

    }

    void DestroyScene()
    {

        BGM bgm = GameObject.Find("BGM").GetComponent<BGM>();

        bgm.SetGame();

        SceneManager.UnloadSceneAsync("Title");

    }

    public void ToSkinShop()
    {
        UIControll gameSystem = GameObject.Find("GameSystem").GetComponent<UIControll>();
        gameSystem.SetSkinShopUI();
        // ここでアクティブの関数を呼ぶ
        SceneManager.LoadScene("SkinShop", LoadSceneMode.Additive);

    }

    public void EventTrigger_SetGameStart()
    {

        Debug.Log("trigger");

        stageSystem sS = GameObject.Find("StageSystem").GetComponent<stageSystem>();
        sS.EventTrigger_GameStart();

        Invoke("DestroyScene", 0.05f);

    }

}

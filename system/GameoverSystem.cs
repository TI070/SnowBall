using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameoverSystem : MonoBehaviour
{

    [SerializeField] float continueScale;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void PlayerContinue()
    {
        

        player Player = GameObject.Find("player").GetComponent<player>();
        Player.AddPlayerScale(continueScale);
        Player.SetDefaultRollSpeed();

        Debug.Log(continueScale);

        SceneManager.UnloadSceneAsync("continueAd");


        SceneManager.UnloadSceneAsync("GameOver");


        GameObject.Find("StageSystem").GetComponent<stageSystem>().StageContinue();

        Player.PlayRollingSound(true);

        BGM bgm = GameObject.Find("BGM").GetComponent<BGM>();
        bgm.SetGame();
        bgm.Mute(false);

    }

    public void LoadContinueAdScene()
    {


        SceneManager.LoadScene("continueAd", LoadSceneMode.Additive);



    }



}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneControll : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StageRetry()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        BGM bgm = GameObject.Find("BGM").GetComponent<BGM>();
        bgm.SetGame();
        bgm.Mute(false);

    }

    public void ToNextStage()
    {
        GameSystem.ToNextStage();
        SceneManager.LoadScene("stage" + GameSystem.currentStageNum);

        BGM bgm = GameObject.Find("BGM").GetComponent<BGM>();
        bgm.SetGame();
        bgm.Mute(false);

    }

    public void AdDelete()
    {
        //çLçêÉVÅ[ÉìçÌèú
        SceneManager.UnloadSceneAsync("Ad");
    }

}

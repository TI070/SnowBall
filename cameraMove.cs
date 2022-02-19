using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMove : MonoBehaviour
{

    [SerializeField] GameObject Target;
    [SerializeField] float zoomoutScaleCorrect;


    float targetDefaultScale;
    float targetCurrentScale;

    [SerializeField] float cameraCorrect;

    // Start is called before the first frame update
    void Start()
    {
        targetDefaultScale = Target.transform.localScale.x;
        //targetCurrentScale = targetDefaultScale;
        targetCurrentScale = 0;
        //ZoomOut();
    }

    // Update is called once per frame
    void Update()
    {

        transform.position = new Vector3(transform.position.x, Target.transform.position.y + GetComponent<Camera>().orthographicSize * cameraCorrect, -10);

        //ZoomOut();

    }


    void ZoomOut()
    {

        //プレイヤーの大きさに変更があればカメラサイズも変更
        if (Target.transform.localScale.x == targetCurrentScale) return;

        //差を算出
        float diff = Target.transform.localScale.x - targetCurrentScale;

        GetComponent<Camera>().orthographicSize += diff * zoomoutScaleCorrect;

        targetCurrentScale = Target.transform.localScale.x;


    }


}

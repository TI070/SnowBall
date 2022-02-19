using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blowedObstruct : MonoBehaviour
{

    Vector2 blowedVector;
    float blowSpeed;
    float blowRollSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        BlowedMove();
    }

    void BlowedMove()
    {

        //ふっとばしベクトルと速度に応じて移動
        transform.position = new Vector3(transform.position.x + blowedVector.x, transform.position.y + blowedVector.y,0);

        //回転
        transform.Rotate(new Vector3(0, 0, blowRollSpeed));

    }

    public void SetBlowSpeed(float s_speed)
    {
        blowSpeed = s_speed;

    }

    public void SetBlowRollSpeed(float s_speed)
    {
        blowRollSpeed = s_speed;

    }

    public void SetBlowVector(Vector2 s_vec)
    {

        blowedVector = s_vec;
        
    }


}

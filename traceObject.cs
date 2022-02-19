using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class traceObject : MonoBehaviour
{

    player Player;
    [SerializeField] float deathDistance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DestroyCheck();
    }

    //�v���C���[�����苗�����ꂽ��Ղ�����(���׌y��)
    void DestroyCheck()
    {

        if(deathDistance < Player.transform.position.y - transform.position.y)
        {

            Destroy(this.gameObject);

        }

    }

    public void SetPlayer(player s_p)
    {
        Player = s_p;
    }

}

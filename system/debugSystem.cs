using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debugSystem : MonoBehaviour
{

    [SerializeField] bool isUse;
    [SerializeField] int setStageNum;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        

    }

    public bool IsUse()
    {
        return isUse;
    }

    public int GetStageNum()
    {
        return setStageNum;
    }



}

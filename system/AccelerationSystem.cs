using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelerationSystem : MonoBehaviour
{

    static Vector3 m_accel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        m_accel = Input.acceleration;
    }

    static Vector3 GetAccel()
    {
        return m_accel;
    }





}

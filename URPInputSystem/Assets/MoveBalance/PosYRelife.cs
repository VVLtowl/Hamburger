using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosYRelife : MonoBehaviour
{
    public Action OnRelife; 

    // Start is called before the first frame update
    void Start()
    {
        OnRelife = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -5.0f)
        {
            OnRelife();
            transform.position = new Vector3(0, 3, 0);
        }
    }
    //---------------------------------
    public void AddRelifeAction(Action func)
    {
        OnRelife += func;
    }
}

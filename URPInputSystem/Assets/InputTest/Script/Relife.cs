using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Relife : MonoBehaviour
{
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y<-10.0f)
        {
            transform.position = new Vector3(0, 3, 0);
        }
    }
}

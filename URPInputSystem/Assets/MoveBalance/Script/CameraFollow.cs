using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform cmrTrs;

    [Header("LOOK ONLY")]
    [SerializeField]
    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        //cmrTrs = Camera.main.transform;
        offset = cmrTrs.position - transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 nowPos = cmrTrs.position;
        Vector3 targetPos = transform.position + offset;
        Vector3 pos = Vector3.Lerp(nowPos, targetPos, 0.01f);
        cmrTrs.position = pos;
    }
}

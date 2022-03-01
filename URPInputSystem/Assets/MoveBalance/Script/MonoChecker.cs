using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoChecker : MonoBehaviour
{
    public Transform checkerTrs;

    [Header("ONLY LOOK")]
    [SerializeField]
    private Mono colMono;

    // Start is called before the first frame update
    void Start()
    {
        colMono = null;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (!colMono)
            if (other.tag == "Mono")
            {
                colMono = other.gameObject.GetComponent<Mono>();
                if (!colMono.CanCatch())
                {
                    colMono = null;
                }
            }
    }

    //-----------------------------------
    public Mono GetColMono()
    {
        Mono mono = colMono;
        colMono = null;
        return mono;
    }
    public void SetCheckerHigh(float highRatio)
    {
        checkerTrs.localScale = new Vector3(1, highRatio, 1);
    }

    public void SetCheckerPosY(float posY)
    {
        Vector3 pos = checkerTrs.position;
        pos.y = posY;
        checkerTrs.position = pos;
    }
}

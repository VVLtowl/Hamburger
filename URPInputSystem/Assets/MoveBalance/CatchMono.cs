using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchMono : MonoBehaviour
{

    [Header("LOOK ONLY")]
    public float shakeDuration;
    [SerializeField]
    private List<Mono> monos = new List<Mono>();
    private MonoChecker monoChecker;
    private Player2Ctrl moveCtrl;
    private PosYRelife relifeCtrl;

    [SerializeField]
    private float shakeMonoTimeStamp;


    // Start is called before the first frame update
    void Start()
    {
        monos.Clear();
        monoChecker = GetComponentInChildren<MonoChecker>();
        moveCtrl = GetComponent<Player2Ctrl>();
        relifeCtrl = GetComponent<PosYRelife>();

        if(relifeCtrl)
        {
            relifeCtrl.AddRelifeAction(RemoveAllMono);
        }
        shakeMonoTimeStamp = 0;
    }

    // Update is called once per frame
    void Update()
    {
        CheckGetMono();
        CheckShakeMono();
    }

    void FixedUpdate()
    {
        UpdateCheckerPos();
        UpdateMoveCtrl();
    }
    //---------------------------------

    private void CheckGetMono()
    {
        Mono colMono = monoChecker.GetColMono();
        if (colMono)
        {
            Mono topMono = null;
            if (monos.Count > 0)
            {
                topMono = monos[monos.Count - 1];
                topMono.SetTopMono(colMono);
            }

            Mono bottomMono = topMono ? topMono : null;
            float followRatio = Mathf.Max(0.05f, 0.9f * Mathf.Pow(0.7f, GetMonoCount()));
            colMono.SetMonoFollow(bottomMono, this, followRatio);

            //float highRatio = GetMonoHigh() / 1.0f;
            //monoChecker.SetCheckerHigh(highRatio);
            monos.Add(colMono);
        }
        else
        {

        }
    }

    private void CheckShakeMono()
    {
        if(shakeMonoTimeStamp==0)
        {
            if (moveCtrl.GetMoveSpeedRatio() >= 0.2f)
            {
                if (monos.Count > 0)
                {
                    shakeMonoTimeStamp = Time.time;

                    Mono bottomMono = monos[0];
                    float xz=1.0f;
                    bottomMono.SetMonoShake(new Vector3(Random.Range(-xz, xz),1.0f, Random.Range(-xz, xz)) * 2.0f);
                }
            }
        }
        else
        {
            shakeDuration = 1 - moveCtrl.GetMoveSpeedRatio() * 0.5f;

            if(Time.time-shakeMonoTimeStamp >= shakeDuration)
            {
                shakeMonoTimeStamp = 0;
            }
        }
    }

    private void UpdateCheckerPos()
    {
        if(monos.Count>0)
        {
            Mono mono = monos[monos.Count - 1];
            float posY = mono.transform.position.y + mono.GetHeight() / 2;
            monoChecker.SetCheckerPosY(posY);
        }
        else
        {
            monoChecker.SetCheckerPosY(transform.position.y + 1.0f);
        }
    }

    private void UpdateMoveCtrl()
    {
        float acc = 1.5f - Mathf.Min(1, GetWeight() / 5.0f);
        moveCtrl.SetSpeedAcc(acc);
    }
    //---------------------------------
    public float GetMonoHigh()
    {
        float high = 0;
        high = 1.0f + GetMonoCount() * 0.5f;
        return high;
    }

    public float GetWeight()
    {
        float weight = 1.0f;
        for(int i=0;i<monos.Count;i++)
        {
            weight += monos[i].GetWeight();
        }
        return weight;
    }

    public int GetMonoCount()
    {
        return monos.Count;
    }

    public void LostMono(Mono mono)
    {
        monos.Remove(mono);
        ChangeCheckerToTop();
    }

    public void RemoveAllMono()
    {
        if (monos.Count > 0)
        {
            Mono mono = monos[0];
            mono.SetMonoFall();
        }
    }
    //give up
    public void ChangeCheckerToTop()//when startfollow over
    {
        if(monos.Count>0)
        {
            Mono topMono = monos[monos.Count - 1];
            monoChecker.SetCheckerPosY(topMono.transform.position.y);
        }
        else
        {
            monoChecker.SetCheckerPosY(transform.position.y + 1.0f);
        }
    }

    public float GetMoveSpeedRatio()
    {
        return 1.0f - moveCtrl.GetMoveSpeedRatio();
    }
}

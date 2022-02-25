using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavAI : MonoBehaviour
{

    public Transform targetTrs;
    private NavMeshAgent agent;
    private Rigidbody rb;

    //bound
    [SerializeField]
    private bool nowBound;
    [SerializeField]
    private float boundStartTimeStamp;
    private float boundKeepTime;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = 1.0f;
        boundStartTimeStamp = 0;
        boundKeepTime = 2.7f;
        nowBound = false;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBoundCheck();

        if(!nowBound)
        {
            if (agent
           && targetTrs)
            {
                agent.SetDestination(targetTrs.position);
            }
        }
    }

    public void StartBound()
    {
        nowBound = true;
        boundStartTimeStamp = Time.time;
        agent.enabled = false;
        //Debug.Log("start bound");
    }

    private void UpdateBoundCheck()
    {
        if (nowBound)
        {
            if (Time.time - boundStartTimeStamp >= boundKeepTime)
              // || rb.velocity.sqrMagnitude < 0.01f)
            {
                agent.enabled = true;
                nowBound = false;
                boundStartTimeStamp = 0;
            }
        }
    }
}

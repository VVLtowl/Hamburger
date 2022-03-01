using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestPlayerInputSystem : MonoBehaviour
{

    private Rigidbody sphereRigidBody;
    private PlayerInput playerInput;
    //private InputTest0 inputAction0;

    private Vector3 moveDir;

    //crash
    public float crashCoolDown;
    private float crashStartTimeStamp;
    private bool canCrash;

    //accelerate
    [SerializeField]
    private bool nowAccelerate;

    //move
    private Transform cmrTrs;
    [SerializeField]
    private float moveSpeed;

    //bound
    [SerializeField]
    private bool nowBound;
    [SerializeField]
    private float boundStartTimeStamp;
    private float boundKeepTime;

    // Start is called before the first frame update
    void Start()
    {
        cmrTrs = Camera.main.transform;
        sphereRigidBody = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        //playerInput.onActionTriggered += PlayerInput_onActionTriggered;

        //inputAction init
        //inputAction0 = new InputTest0();
        //inputAction0.Man.Enable();

        //inputAction0.Man.Crash.performed += ipt_Crash;
        //inputAction0.Man.Crash.performed += OnCrash;
        //inputAction0.Man.Accelerate.started += OnAccelerateStart;
        //inputAction0.Man.Accelerate.canceled += OnAccelerateStop;

        //other init
        moveDir = Vector3.zero;
        crashStartTimeStamp = 0;
        canCrash = true;
        nowAccelerate = false;

        boundStartTimeStamp = 0;
        boundKeepTime = 2.7f;
        nowBound = false;
    }

    // Update is called once per frame
    void Update()
    {
        //var inputDir = playerInput. Man.Move.ReadValue<Vector2>();
       
        Move();
        UpdateMoveSpeed();
        UpdateCrashCheck();
        UpdateBoundCheck();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="Floor")
        {
            
        }
    }
    //------------------------------------------------------------
    public void ipt_OnCrash(InputAction.CallbackContext context)
    {
        //if(context.started)//or performed
        if (nowBound) return;

        if (context.performed)
        {
            if (canCrash)
            {
                canCrash = false;
                sphereRigidBody.AddForce(moveDir * 5.0f, ForceMode.Impulse);
                crashStartTimeStamp = Time.time;
            }
        }
        Debug.Log("crash! " + context.phase);
    }

    public void ipt_OnMove(InputAction.CallbackContext context)
    {
        var inputDir = context.ReadValue<Vector2>();
        moveDir = Quaternion.Euler(0, cmrTrs.eulerAngles.y, 0) * new Vector3(inputDir.x, 0, inputDir.y);
    }

    public void ipt_OnAccelerate(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            nowAccelerate = true;
        }
        else if(context.canceled)
        {
            nowAccelerate = false;
        }
    }
    //----------------------------------------
    private void UpdateCrashCheck()
    {
        if (!canCrash)
        {
            float nowTime = Time.time;
            if (nowTime - crashStartTimeStamp >= crashCoolDown)
            {
                canCrash = true;
                crashStartTimeStamp = 0;
            }
        }
    }
    private void UpdateMoveSpeed()
    {
        //if (nowBound) return;

        if (nowAccelerate)
        {
            moveSpeed += 0.02f;
        }
        else
        {
            moveSpeed -= 0.2f;
        }
        moveSpeed = Mathf.Clamp(moveSpeed, 0, 4.0f);
    }
    private void Move()
    {
        if (nowBound) return;

        if (moveDir.sqrMagnitude < 0.01f)
        {
            //Debug.Log("cant move! " + inputDir.x + " : " + inputDir.y);
            //return;
        }
        else
        {
            moveSpeed = Mathf.Max(moveSpeed, 2.0f);
            transform.forward = moveDir;
        }
        //Debug.Log("move! ");

        var scaledMoveSpeed = moveSpeed * Time.deltaTime;

        transform.position += moveDir * scaledMoveSpeed;
    }

    public void StartBound()
    {
        nowBound = true;
        boundStartTimeStamp = Time.time;
        moveSpeed = 0;
        Debug.Log("start bound");
    }
    public bool IsBound()
    {
        return nowBound;
    }

    private void UpdateBoundCheck()
    {
        if(nowBound)
        {
            if(Time.time-boundStartTimeStamp>=boundKeepTime
               ||sphereRigidBody.velocity.sqrMagnitude<0.01f)
            {
                nowBound = false;
                boundStartTimeStamp = 0;
            }
        }
    }
}


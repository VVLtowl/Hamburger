using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestInputSystem : MonoBehaviour
{

    private Rigidbody sphereRigidBody;
    private PlayerInput playerInput;
    private InputTest0 inputAction0;

    private Vector3 moveDir;
    
    //crash
    public float crashCoolDown;
    private float crashStartTimeStamp;
    private bool canCrash;

    //accelerate
    private bool nowAccelerate;

    //move
    private Transform cmrTrs;
    public float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        cmrTrs = Camera.main.gameObject.transform;
        sphereRigidBody = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        //playerInput.onActionTriggered += PlayerInput_onActionTriggered;

        //inputAction init
        inputAction0 = new InputTest0();
        inputAction0.Man.Enable();

        //inputAction0.Man.Crash.performed += ipt_Crash;
        inputAction0.Man.Crash.performed += OnCrash;
        inputAction0.Man.Accelerate.started += OnAccelerateStart;
        inputAction0.Man.Accelerate.canceled += OnAccelerateStop;

        //other init
        moveDir = Vector3.zero;
        crashStartTimeStamp = 0;
        canCrash = true;
        nowAccelerate = false;
    }

    // Update is called once per frame
    void Update()
    {
        var inputDir = inputAction0.Man.Move.ReadValue<Vector2>();
        moveDir = Quaternion.Euler(0, cmrTrs.eulerAngles.y, 0) * new Vector3(inputDir.x, 0, inputDir.y);

        Move(inputDir);
        Accelerate();
        UpdateCrashCheck();
    }

    //------------------------------------------------------------
    private void PlayerInput_onActionTriggered(InputAction.CallbackContext context)
    {
        Debug.Log(context);
    }

    public void ipt_Crash(InputAction.CallbackContext context)
    {
        //if(context.started)//or performed
        if (context.performed)
        {
            sphereRigidBody.AddForce(Vector3.up * 5.0f, ForceMode.Impulse);
        }
        Debug.Log("crash! " + context.phase);
    }


    private void OnCrash(InputAction.CallbackContext context)
    {
        if(canCrash)
        {
            canCrash = false;
            sphereRigidBody.AddForce(moveDir * 5.0f, ForceMode.Impulse);
            crashStartTimeStamp = Time.time;
        }
    }

    private void OnAccelerateStart(InputAction.CallbackContext context)
    {
        nowAccelerate = true;
    }

    private void OnAccelerateStop(InputAction.CallbackContext context)
    {
        nowAccelerate = false;
    }

    private void UpdateCrashCheck()
    {
        if(!canCrash)
        {
            float nowTime = Time.time;
            if (nowTime - crashStartTimeStamp >= crashCoolDown)
            {
                canCrash = true;
                crashStartTimeStamp = 0;
            }
        }
    }
    private void Accelerate()
    {
        if(nowAccelerate)
        {
            moveSpeed += 0.02f;
        }
        else
        {
            moveSpeed -= 0.2f;
        }
        moveSpeed = Mathf.Clamp(moveSpeed, 0, 4.0f);
    }
    private void Move(Vector2 inputDir)
    {
        if (inputDir.sqrMagnitude < 0.01)
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
        //var move = Quaternion.Euler(0, cmrTrs.eulerAngles.y, 0) * new Vector3(inputDir.x, 0, inputDir.y);
        //Debug.Log("moveDir " + moveDir.x + " : " + moveDir.z);
        //transform.position += move * scaledMoveSpeed;

        transform.position += moveDir * scaledMoveSpeed;
    }
}

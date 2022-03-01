using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player2Ctrl : MonoBehaviour
{
    public enum CtrlType
    {
        normal,
        stickAngleSpeed,
    }

    private Rigidbody rb;
    private PlayerInput playerInput;
    [Header("INPUT")]
    public CtrlType ctrlType;


    [Header("ONLY LOOK")]
    //move
    private Transform cmrTrs;
    private Vector3 moveDir;
    private Vector3 lastMoveDir;

    [SerializeField]
    private bool nowMove;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float minSpeed;
    [SerializeField]
    private float speedAcc;
    [SerializeField]
    private float speedDownAcc;

    //crash
    public float crashCoolDown;
    private float crashStartTimeStamp;
    private bool canCrash;
    [SerializeField]
    private float crashPower;

    //accelerate
    [SerializeField]
    private bool nowAccelerate;
    [SerializeField]
    private bool nowMinusAccelerate;
    [SerializeField]
    private float acceleratePower;
    [SerializeField]
    private float minusAcceleratePower;

    //jump
    [SerializeField]
    private bool canJump;
    [SerializeField]
    private float jumpPower;

    //mono


    // Start is called before the first frame update
    void Start()
    {
        cmrTrs = Camera.main.transform;
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();

        //other init
        maxSpeed = 8.0f;
        minSpeed = 3.5f;
        speedAcc = 0.0f;
        speedDownAcc = 4.0f;

        jumpPower = 7.0f;
        crashPower = 5.0f;

        crashStartTimeStamp = 0;
        canCrash = true;

        moveDir = Vector3.zero;
        nowMove = false;
        nowAccelerate = false;
        nowMinusAccelerate = false;
        acceleratePower = 3.0f;
        minusAcceleratePower = 6.0f;

        moveSpeed = 0;

        canJump = true;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        UpdateMoveSpeed();
        UpdateAccelerate();
        UpdateMinusAccelerate();
        UpdateCrashCheck();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            canJump = true;
        }
    }
    //------------------------------------------------------------
    public void ipt_OnCrash(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (canCrash)
            {
                canCrash = false;
                rb.AddForce(moveDir * crashPower, ForceMode.Impulse);
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
        else if (context.canceled)
        {
            nowAccelerate = false;
        }
    }

    public void ipt_OnMinusAccelerate(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            nowMinusAccelerate = true;
        }
        else if (context.canceled)
        {
            nowMinusAccelerate = false;
        }
    }

    public void ipt_OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (canJump)
            {
                canJump = false;
                rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            }
        }
        Debug.Log("jump! " + context.phase);
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
        if (nowMove)
        {
            moveSpeed += speedAcc * Time.deltaTime;
        }
        else
        {
            moveSpeed -= speedDownAcc * Time.deltaTime;
        }
        moveSpeed = Mathf.Clamp(moveSpeed, 0, maxSpeed);
    }
    private void UpdateAccelerate()
    {
        if (nowAccelerate)
        {
            moveSpeed += acceleratePower * Time.deltaTime;
        }
        moveSpeed = Mathf.Clamp(moveSpeed, 0, maxSpeed);
    }
    private void UpdateMinusAccelerate()
    {
        if (nowMinusAccelerate)
        {
            moveSpeed -= minusAcceleratePower * Time.deltaTime;
            if(ctrlType==CtrlType.stickAngleSpeed)
            {
                moveSpeed = 3.0f;
            }
        }
        moveSpeed = Mathf.Clamp(moveSpeed, 0, maxSpeed);
    }

    private void Move()
    {
        if (moveDir.sqrMagnitude < 0.01f)
        {
            //Debug.Log("cant move! " + inputDir.x + " : " + inputDir.y);
            //return;
            nowMove = false;
            var scaledMoveSpeed = moveSpeed * Time.deltaTime;
            transform.position += lastMoveDir * scaledMoveSpeed;
        }
        else
        {
            nowMove = true;
            //moveSpeed = Mathf.Max(moveSpeed, minSpeed);
            transform.forward = moveDir;
            var scaledMoveSpeed = moveSpeed * Time.deltaTime;
            transform.position += moveDir * scaledMoveSpeed;
            lastMoveDir = moveDir;
        }
        //Debug.Log("move! ");

    }

    //--------------------------------------------------------
    public float GetMoveSpeed()
    {
        return moveSpeed;
    }
    public float GetMoveSpeedRatio()
    {
        return moveSpeed / maxSpeed;
    }
    public void SetSpeedAcc(float acc)
    {
        speedAcc = acc;
    }
}

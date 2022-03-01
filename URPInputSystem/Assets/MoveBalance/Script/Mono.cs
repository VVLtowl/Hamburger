using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mono : MonoBehaviour
{
    public enum MonoState
    {
        prepare,
        freeFall,
        free,
        startFollow,//catching perform
        follow,
        fall,
    }

    [Header("ONLY LOOK")]
    [SerializeField]
    private MonoState state;


    private Rigidbody rb;
    [SerializeField]
    private float maxLengthSqr;
    [SerializeField]
    private float followLengthSqr;
    [SerializeField]
    private MonoChecker monoChecker;
    [SerializeField]
    private Renderer rd;

    [SerializeField]
    private float followRatio;
    [SerializeField]
    private Transform bottomMonoTrs;
    [SerializeField]
    private float deltaPosY;
    [SerializeField]
    private Vector2 offsetXZ;

    [SerializeField]
    private bool nowShakeXZ;
    [SerializeField]
    private Vector2 shakeForceXZ;
    [SerializeField]
    private float shakeForceY;

    [SerializeField]
    private float shakeAccY;
    [SerializeField]
    private float shakePosY;

    [SerializeField]
    private Mono bottomMono;
    [SerializeField]
    private Mono topMono;
    [SerializeField]
    private CatchMono catchMono;
   
    [SerializeField]
    private float height;
    [SerializeField]
    private float weight;

    // Start is called before the first frame update
    void Start()
    {
        monoChecker = GetComponentInChildren<MonoChecker>();
        //state = MonoState.follow;
        //followRatio = 0.2f;
        rb = GetComponent<Rigidbody>();
        rd = GetComponent<MeshRenderer>();
        //rb.isKinematic = true;
        followLengthSqr = 0.1f;
        maxLengthSqr = 6.0f;
        deltaPosY = 0.5f;

        float sclXZ = Random.Range(2.5f, 3.2f);
        float sclY = Random.Range(0.2f, 1.0f);
        transform.localScale = new Vector3(sclXZ, sclY, sclXZ);
        height = sclY * 2;
        weight = sclY;
        
        shakeForceXZ = Vector2.zero;
        shakeForceY = 0.0f;
        shakePosY = 0.0f;
        shakeAccY = 0.0f;



        offsetXZ = Vector2.zero;

        ChangeState(MonoState.free);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        CheckDie();
    }

    void FixedUpdate()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            if (state == MonoState.startFollow
                || state == MonoState.follow)
            {
                SetMonoFall();
            }
            else
            {
                ChangeState(MonoState.free);

                MonoManager.Instance.RemoveMono(this);
                Destroy(gameObject);
            }
        }
    }
    //----------------------------------------
    private void Move()
    {
        switch (state)
        {
            case MonoState.prepare:
                {

                }
                break;
            case MonoState.free:
                {

                }
                break;
            case MonoState.freeFall:
                {

                }
                break;
            case MonoState.fall:
                {
                    Vector3 velo = rb.velocity;
                    // rb.velocity = new Vector3(velo.x, Mathf.Min(3, velo.y), velo.z);
                }
                break;
            case MonoState.startFollow:
                {
                    if (bottomMonoTrs)
                    {
                        //pos
                        {
                            //Vector3 nowPos = transform.position;
                            //Vector3 targetPos = bottomMonoTrs.position + new Vector3(offsetXZ.x, deltaPosY, offsetXZ.y);

                            //float lengthSqr = Vector3.SqrMagnitude(targetPos - nowPos);

                            //Vector3 pos = Vector3.Lerp(nowPos, targetPos, 0.4f);
                            //transform.position = pos;
                            //if (lengthSqr <= 0.1f)
                            //{
                            //    ChangeState(MonoState.follow);
                            //}
                        }

                        //high
                        {
                            Vector3 nowPos = transform.position;
                            Vector3 targetPos = new Vector3(transform.position.x,bottomMonoTrs.position.y + deltaPosY, transform.position.z);

                            float lengthSqr = Vector3.SqrMagnitude(targetPos - nowPos);

                            Vector3 pos = Vector3.Lerp(nowPos, targetPos, 0.4f);
                            transform.position = pos;
                            if (lengthSqr <= 0.1f)
                            {
                                ChangeState(MonoState.follow);
                            }
                        }
                    }
                }
                break;
            case MonoState.follow:
                {
                    if (bottomMonoTrs)
                    {
                        ShakeY();

                        Vector2 nowPosXZ = new Vector2(transform.position.x, transform.position.z);
                        Vector2 targetPosXZ = new Vector2(bottomMonoTrs.position.x + offsetXZ.x, bottomMonoTrs.position.z + offsetXZ.y);
                        Vector3 nowPos = transform.position;
                        Vector3 targetPos = bottomMonoTrs.position + new Vector3(offsetXZ.x, deltaPosY + shakePosY, offsetXZ.y);
                        Vector3 reversePos = new Vector3((nowPos - (targetPos - nowPos)).x, targetPos.y, (nowPos - (targetPos - nowPos)).z);

                        float lengthSqr = Vector2.SqrMagnitude(targetPosXZ - nowPosXZ);

                        if(nowShakeXZ)
                        {
                            ShakeXZ();
                            if (shakeForceXZ == Vector2.zero)
                            {
                                nowShakeXZ = false;
                            }
                        }

                        if (lengthSqr > maxLengthSqr)
                        {
                            Debug.Log("fall lengthSqr: " + lengthSqr);
                            SetMonoFall();
                        }
                        else if (lengthSqr > followLengthSqr
                            && lengthSqr <= maxLengthSqr)
                        {
                            rd.material.color = Color.red;
                            float ratio = lengthSqr / maxLengthSqr;
                            Vector3 pos = Vector3.Lerp(nowPos, reversePos, 0.02f * ratio);
                            Vector3 pos1 = Vector3.Lerp(nowPos, targetPos, 0.2f * catchMono.GetMoveSpeedRatio() + followRatio);
                            pos.y = pos1.y;
                            transform.position = pos;
                        }
                        else
                        {
                            rd.material.color = Color.green;
                            Vector3 pos = Vector3.Lerp(nowPos, targetPos, 0.2f * catchMono.GetMoveSpeedRatio() + followRatio);
                            transform.position = pos;
                            //ShakeXZ();
                        }
                    }
                }
                break;
        }
    }

    private void CheckDie()
    {
        if (transform.position.y < -30.0f)
        {
            MonoManager.Instance.RemoveMono(this);
            Destroy(gameObject);
        }
    }
    private void ChangeState(MonoState nextState)
    {
        if (nextState == state) return;
        switch (state)
        {
            case MonoState.startFollow:
                {
                    //catchMono.ChangeCheckerToTop();
                }
                break;
        }

        state = nextState;

        switch (state)
        {
            case MonoState.free:
            case MonoState.freeFall:
                {
                    //rb.isKinematic = false;
                    rb.useGravity = true;
                    rd.material.color = Color.gray;
                }
                break;
            case MonoState.startFollow:
                {
                    //rb.isKinematic = true;
                    rb.velocity = Vector3.zero;
                    rb.useGravity = false;
                }
                break;
            case MonoState.fall:
                {
                    bottomMonoTrs = null;
                    deltaPosY = 0;
                    offsetXZ = Vector2.zero;
                    followRatio = 0;
                    rb.useGravity = true;
                }
                break;
        }
    }
    private void ShakeXZ()
    {
        //XZ
        {
            if(Vector2.SqrMagnitude(shakeForceXZ)>0.001f)
            {
                transform.position += new Vector3(shakeForceXZ.x, 0, shakeForceXZ.y);
                shakeForceXZ = Vector2.Lerp(shakeForceXZ, Vector2.zero, 0.5f * Time.deltaTime);
            }
            else
            {
                shakeForceXZ = Vector2.zero;
            }
        }       
    }
    private void ShakeY()
    {
        //Y
        {
            if (shakePosY > 0)
            {
                shakePosY += shakeForceY * Time.deltaTime;
                shakeForceY -= shakeAccY * Time.deltaTime;
            }
            else
            {
                shakePosY = 0;
                shakeForceY = 0;
            }
        }
    }

    //----------------------------------------------
    public void SetMonoShake(Vector3 force)
    {
        shakePosY = 0.0001f;
        shakeForceXZ = new Vector2(force.x, force.z);
        shakeForceY = force.y;
        shakeAccY = shakeForceY / 0.25f;

        if(shakeForceXZ!=Vector2.zero)
        {
            nowShakeXZ = true;
        }
        if (topMono)
        {
            topMono.SetMonoShake(force * 0.9f);
        }
    }
    public void SetMonoFall()
    {
        Vector3 nowPos = transform.position;
        Vector3 targetPos = bottomMonoTrs.position + new Vector3(offsetXZ.x, deltaPosY + shakePosY, offsetXZ.y);
        float deltaPosX = Mathf.Max(0.2f, (nowPos - targetPos).x);
        float deltaPosZ = Mathf.Max(0.2f, (nowPos - targetPos).z);
        Vector3 fallDir = new Vector3(deltaPosX, 0, deltaPosZ);
        rb.velocity = fallDir.normalized * 3.0f + new Vector3(0, 5.0f, 0);

        catchMono.LostMono(this);
        catchMono = null;

        if (bottomMono)
        {
            bottomMono.SetTopMono(null);
            bottomMono = null;
        }

        ChangeState(MonoState.freeFall);

        if (topMono)
        {
            topMono.SetMonoFall();
        }
    }
    public void SetTopMono(Mono _topMono)
    {
        topMono = _topMono;
    }
    public void SetBottomMono(Mono _bottomMono)
    {
        bottomMono = _bottomMono;
    }

    public void SetMonoFollow(Mono _bottomMono, CatchMono _catchMono, float _followRatio)
    {
        bottomMono = _bottomMono;
        catchMono = _catchMono;
        bottomMonoTrs = bottomMono ? bottomMono.transform : catchMono.transform;
        deltaPosY = bottomMono ? bottomMono.GetHeight() / 2 + height / 2 + 0.1f : 1.5f + height / 2;
        offsetXZ = new Vector2(Random.value * 0.2f - 0.1f, Random.value * 0.2f - 0.1f);
        followRatio = _followRatio;
        ChangeState(MonoState.startFollow);
    }
    public bool HasBottomMono()
    {
        return bottomMono ? true : false;
    }
    public Mono GetBottomMono()
    {
        return bottomMono;
    }

    public bool CanCatch()
    {
        return state == MonoState.free || state == MonoState.freeFall ? true : false;
    }
    public float GetHeight()
    {
        return height;
    }
    public float GetWeight()
    {
        return weight;
    }
}

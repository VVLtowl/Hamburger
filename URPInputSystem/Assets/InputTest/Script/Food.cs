using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public enum FoodState
    {
        prepare,
        free,
        startFollow,
        follow,
        fall,
    }

    [SerializeField]
    private FoodState state;
   

    private Rigidbody rb;
    private float radius;


    [SerializeField]
    private float followRatio;
    [SerializeField]
    private Transform bottomMonoTrs;
    [SerializeField]
    private float deltaPosY;

    [SerializeField]
    private Food bottomFood;
    [SerializeField]
    private Food topFood;
    [SerializeField]
    private Chef chef;

    private float weight;

    // Start is called before the first frame update
    void Start()
    {
        //state = FoodState.follow;
        //followRatio = 0.2f;
        rb = GetComponent<Rigidbody>();
        //rb.isKinematic = true;
        radius = 0.5f;
        //deltaPosY = 0.5f;
        weight = 1.0f;

        ChangeState(FoodState.fall);
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
            if (state == FoodState.startFollow
                ||state==FoodState.follow)
            {
                SetFoodFall();
            }
            else
            {
                ChangeState(FoodState.free);
            }
        }
    }
    //----------------------------------------
    private void Move()
    { 
        switch(state)
        {
            case FoodState.prepare:
                {

                }
                break;
            case FoodState.free:
                {

                }
                break;
            case FoodState.fall:
                {

                }
                break;
            case FoodState.startFollow:
                {
                    if (bottomMonoTrs)
                    {
                        Vector3 nowPos = transform.position;
                        Vector3 targetPos = bottomMonoTrs.position + new Vector3(0, deltaPosY, 0);
                        float lengthSqr = Vector3.SqrMagnitude(targetPos - nowPos);

                        Vector3 pos = Vector3.Lerp(nowPos, targetPos, 0.4f);
                        transform.position = pos;

                        if (lengthSqr <= radius * radius)
                        {
                            ChangeState(FoodState.follow);
                        }
                    }
                }
                break;
            case FoodState.follow:
                {
                    if(bottomMonoTrs)
                    {
                        Vector3 nowPos = transform.position;
                        Vector3 targetPos = bottomMonoTrs.position + new Vector3(0, deltaPosY, 0);
                        float lengthSqr = Vector3.SqrMagnitude(targetPos - nowPos);

                        if (lengthSqr > radius*radius)
                        {
                            Debug.Log("fall lengthSqr: " + lengthSqr);
                            SetFoodFall();
                        }
                        else
                        {
                            Vector3 pos = Vector3.Lerp(nowPos, targetPos, followRatio);
                            transform.position = pos;
                        }
                    }
                }
                break;
        }
    }
    private void CheckDie()
    {
        if(transform.position.y<-30.0f)
        {
            Destroy(gameObject);
        }
    }
    private void ChangeState(FoodState nextState)
    {
        if (nextState == state) return;
        switch(state)
        {

        }

        state = nextState;

        switch(state)
        {
            case FoodState.free:
                {
                    //rb.isKinematic = false;
                    rb.useGravity = true;
                }
                break;
            case FoodState.startFollow:
                {
                    //rb.isKinematic = true;
                    rb.useGravity = false;
                }
                break;
            case FoodState.fall:
                {
                    bottomMonoTrs = null;
                    deltaPosY = 0;
                    followRatio = 0;
                    rb.useGravity = true;
                }
                break;
        }
    }

    public void SetFoodFall()
    {
        chef.LostFood(this);
        chef = null;

        FoodManager.Instance.RemoveFood(this);

        if (bottomFood)
        {
            bottomFood.SetTopFood(null);
            bottomFood = null;
        }

        ChangeState(FoodState.fall);

        if(topFood)
        {
            topFood.SetFoodFall();
        }
    }
    public void SetTopFood(Food _topFood)
    {
        topFood = _topFood;
    }
    public void SetBottomFood(Food _bottomFood)
    {
        bottomFood = _bottomFood;
    }
    public void SetFoodFollow(Food _bottomFood, Chef _chef, float _followRatio)
    {
        bottomFood = _bottomFood;
        chef = _chef;
        bottomMonoTrs = bottomFood ? bottomFood.transform : chef.transform;
        deltaPosY = bottomFood ? 0.4f : 0.7f;
        followRatio = _followRatio;
        ChangeState(FoodState.startFollow);
    }
    public bool HasBottomFood()
    {
        return bottomFood ? true : false;
    }
    public Food GetBottomFood()
    {
        return bottomFood;
    }

    public bool CanCatch()
    {
        return state == FoodState.free ? true : false;
    }

    public float GetWeight()
    {
        return weight;
    }
}

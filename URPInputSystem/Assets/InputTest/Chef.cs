using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chef : MonoBehaviour
{
    public bool isAI;

    [SerializeField]
    private List<Food> foods = new List<Food>();
    private FoodChecker foodChecker;
    private ChefChecker chefChecker;
    private Rigidbody rb;

    private TestPlayerInputSystem playerInput;
    private NavAI aiCtrl;

    private Vector3 boundDir;
    private float boundSpeed;
    private bool canBound;
    // Start is called before the first frame update
    void Start()
    {
        foods.Clear();
        foodChecker = GetComponentInChildren<FoodChecker>();
        chefChecker = GetComponentInChildren<ChefChecker>();
        rb = GetComponent<Rigidbody>();

        if(!isAI)
        {
            playerInput = GetComponent<TestPlayerInputSystem>();
        }
        else
        {
            aiCtrl = GetComponent<NavAI>();
        }

        boundDir = Vector3.zero;
        boundSpeed = 0;
        canBound = true;
    }

    // Update is called once per frame
    void Update()
    {
        CheckGetFood();
        CheckRestart();
    }

    void FixedUpdate()
    {
        if (!canBound) canBound = true;
    }
    //--------------------------------------------------------------
    public void PrepareBound(Chef colChef)
    {
        Debug.Log("has colChef");
        float selfWeight = GetWeight();
        float colWeight = colChef.GetWeight();
        float power = Mathf.Clamp(selfWeight + colWeight, 5.0f, 12.0f);

        boundSpeed = power * colWeight / (colWeight + selfWeight);
        boundDir = (transform.position - colChef.transform.position).normalized;
    }

    public void StartBound()
    {
        canBound = false;
        if (!isAI) playerInput.StartBound();
        else aiCtrl.StartBound();
        rb.AddForce(boundDir * boundSpeed, ForceMode.Impulse);
    }

    private void CheckRestart()
    {
        if (transform.position.y < -5.0f)
        {
            ClearAllFood();
            transform.position = new Vector3(0, 3, 0);
        }
    }

    private void CheckGetFood()
    {
        Food colFood = foodChecker.GetColFood();
        if (colFood)
        {
            Food topFood = null;
            if (foods.Count > 0)
            {
                topFood = foods[foods.Count - 1];
                topFood.SetTopFood(colFood);
            }

            Food bottomFood = topFood ? topFood : null;
            float followRatio = 0.9f * Mathf.Pow(0.7f, GetFoodCount());
            colFood.SetFoodFollow( bottomFood,this, followRatio);

            //float highRatio = GetFoodHigh() / 1.0f;
            //foodChecker.SetColHigh(highRatio);

            foods.Add(colFood);
        }
        else
        {

        }
    }

    //--------------------------------------
    public float GetFoodHigh()
    {
        float high = 0;
        high = 1.0f + GetFoodCount() * 5f;
        return high;
    }

    public int GetFoodCount()
    {
        return foods.Count;
    }

    public void LostFood(Food food)
    {
        foods.Remove(food);
    }

    public void ClearAllFood()
    {
        if (foods.Count <= 0) return;
        Food food = foods[0];
        if(food) food.SetFoodFall();
    }

    public float GetWeight()
    {
        float weight = 1.0f;

        foreach(var food in foods)
        {
            weight += food.GetWeight();
        }

        return weight;
    }

    public bool IsCanBound()
    {
        return canBound;
    }
}

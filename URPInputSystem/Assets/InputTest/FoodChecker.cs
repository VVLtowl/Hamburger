using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodChecker : MonoBehaviour
{
    [SerializeField]
    private Food colFood;
    public Transform colTrs;

    // Start is called before the first frame update
    void Start()
    {
        colFood = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!colFood)
            if (other.tag == "Food")
            {
                colFood = other.gameObject.GetComponent<Food>();
                if(!colFood.CanCatch())
                {
                    colFood = null;
                }
            }
    }

    //-----------------------------------
    public Food GetColFood()
    {
        Food food = colFood;
        colFood = null;
        return food;
    }
    public void SetColHigh(float highRatio)
    {
        colTrs.localScale = new Vector3(1, highRatio, 1);
    }
}

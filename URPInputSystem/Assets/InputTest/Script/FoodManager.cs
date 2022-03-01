using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    private static FoodManager instance;
    public static FoodManager Instance
    {
        get
        {
            return instance;
        }
    }
    //---------------------------------------------------

    public GameObject foodPrefab;
    public Vector3 pos;
    public float range;
    public List<Food> foods;

    private float createFoodStartTimeStamp;
    public float createFoodDuration;

    private void Awake()
    {
        instance = this;

        //foods = new List<Food>();
        createFoodStartTimeStamp = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        TimeCreateFood();
    }

    //-------------------------------------------------------
    void TimeCreateFood()
    {
        if (foods.Count > 10) return;
        if(Time.time-createFoodStartTimeStamp>createFoodDuration)
        {
            createFoodStartTimeStamp = Time.time;

            GameObject go = Instantiate(foodPrefab);
            go.name += " (" + (foods.Count + 1) + ")";
            float rot = Random.Range(-Mathf.PI, Mathf.PI);
            float radius = Random.Range(1.0f, 5.0f);
            float posX = radius * Mathf.Cos(rot);
            float posZ = radius * Mathf.Sin(rot);
            go.transform.position = new Vector3(posX, 3.0f, posZ);
            Food food = go.GetComponent<Food>();

            foods.Add(food);
        }
    }

    public void RemoveFood(Food food)
    {
        foods.Remove(food);
    }
}

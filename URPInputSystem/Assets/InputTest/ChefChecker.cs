using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChefChecker : MonoBehaviour
{
    [SerializeField]
    public Chef colChef;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Chef")
        {
            Debug.Log("chef crash");
            Chef chef = other.GetComponentInParent<Chef>();
            colChef = chef;
        }
    }

    //----------------------------------------------------------

    public Chef GetColChef()
    {
        Chef chef = colChef;
        colChef = null;
        return chef;
    }
}

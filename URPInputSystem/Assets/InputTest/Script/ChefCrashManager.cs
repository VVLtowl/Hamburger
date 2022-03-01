using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChefCrashManager : MonoBehaviour
{
    public List<Chef> chefs;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i=0;i<chefs.Count;i++)
        {
            Chef chef1 = chefs[i];
            if (!chef1.IsCanBound()) continue;

            for (int j=0;j<chefs.Count;j++)
            {
                if (i == j) continue;

                Chef chef2 = chefs[j];
                if (!chef2.IsCanBound()) continue;

                Vector3 pos1 = chef1.transform.position;
                Vector3 pos2 = chef2.transform.position;
                float minLength = 1.1f;

                if((pos2-pos1).sqrMagnitude< minLength * minLength)
                {
                    chef1.PrepareBound(chef2);
                    chef2.PrepareBound(chef1);

                    chef1.StartBound();
                    chef2.StartBound();
                }
            }
        }
    }
}

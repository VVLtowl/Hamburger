using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoManager : MonoBehaviour
{
    private static MonoManager instance;
    public static MonoManager Instance
    {
        get
        {
            return instance;
        }
    }
    //---------------------------------------------------

    public GameObject monoPrefab;



    public float createMonoDuration;
    public int monoMax;

    [Header("ONLY LOOK")]
    //public Vector3 pos;
    public float range;
    public List<Mono> monos;

    private float createMonoStartTimeStamp;

    private void Awake()
    {
        instance = this;
        //pos = new Vector3(0, 0, 0);
        range = 3.0f;
        //monos = new List<Mono>();
        createMonoStartTimeStamp = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        TimeCreateMono();
    }

    //-------------------------------------------------------
    void TimeCreateMono()
    {
        if (monos.Count > monoMax) return;
        if (Time.time - createMonoStartTimeStamp > createMonoDuration)
        {
            createMonoStartTimeStamp = Time.time;

            GameObject go = Instantiate(monoPrefab);
            go.name += " (" + (monos.Count + 1) + ")";
            float rot = Random.Range(-Mathf.PI, Mathf.PI);
            float radius = Random.Range(1.0f, 5.0f);
            float posX = transform.position.x + radius * Mathf.Cos(rot);
            float posY = 20.0f;
            float posZ = transform.position.z + radius * Mathf.Sin(rot);
            go.transform.position = new Vector3(posX, posY, posZ);
            Mono mono = go.GetComponent<Mono>();

            monos.Add(mono);
        }
    }

    public void RemoveMono(Mono mono)
    {
        monos.Remove(mono);
    }
}

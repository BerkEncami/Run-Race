using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingSystem : MonoBehaviour
{
    public int currectCheckPoint = 1, lapCount;

    public float distance;
    private Vector3 checkPoint;

    public float counter;
    public int rank;

    void Start()
    {
        currectCheckPoint = 1;
        checkPoint = GameObject.Find("CheckPoint" + currectCheckPoint).transform.position;
    }

    void Update()
    {
        CalculateDistance();
    }

    void CalculateDistance()
    {
        distance = Vector3.Distance(transform.position, checkPoint);
        counter = lapCount * 1000 + currectCheckPoint * 100 + distance;
    }

    void OnTriggerEnter(Collider target)
    {
        if (target.tag == "CheckPoint")
        {
            currectCheckPoint = target.GetComponent<CurrentCheckPoint>().currentCheckNumber;
            checkPoint = GameObject.Find("CheckPoint" + currectCheckPoint).transform.position;
        }

        if (target.tag == "Finish")
        {
            lapCount += 1;
            GameManager.Instance.pass += 1;
        }
    }
}

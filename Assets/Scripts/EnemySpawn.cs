using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawn : MonoBehaviour
{
    List<Transform> wayPoints = new List<Transform>();
    List<Transform> wayPoints2 = new List<Transform>();

    public GameObject enemy;
    private Animation Animation;

    public bool isRunning = false;
    public bool gateOpened = false;

    public float spawnTime = 8f;
    private float counter = 0f;

    private float moveTime = 2f;
    private float counter2 = -5f;
    private bool move = false;

    void Start()
    {
        Transform w = GameObject.FindGameObjectWithTag("wayPoints").transform;
        foreach (Transform t in w.transform)
            wayPoints.Add(t);

        Transform s = GameObject.Find("WayPoints2").transform;
        foreach (Transform t in s.transform)
            wayPoints2.Add(t);
    }

    void Update()
    {
        counter += Time.deltaTime;

        // Spawn enemy
        if(counter >= spawnTime)
        {
            counter = 0f;

            Transform w;

            if (gateOpened)
            {
                int temp = Random.Range(0, 100);
                if (temp > 50)
                    w = wayPoints[Random.Range(0, wayPoints.Count)].transform;
                else
                    w = wayPoints2[Random.Range(0, wayPoints2.Count)].transform;
            }
            else
                w = wayPoints[Random.Range(0, wayPoints.Count)].transform;

            GameObject e = Instantiate(enemy, w.position, w.rotation);
            Animation = e.GetComponent<Animation>();
            Animation.Play("Idle");
            move = true;
        }

        if(move)
        {
            counter2 += Time.deltaTime;
            if (counter2 >= moveTime)
            {
                counter2 = 0f;
                isRunning = true;
            }
        }
    }
}

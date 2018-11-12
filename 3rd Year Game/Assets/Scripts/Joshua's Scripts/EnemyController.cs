using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public Transform[] Waypoints;
    private int nextPoint;
    public float Speed;
    public float setTimer;
    private float waitTimer;
    private bool touched;

    public bool MoveToPlayer;

    public GameObject Player;

	// Use this for initialization
	void Start () {
        nextPoint = 1;
        transform.LookAt(Waypoints[nextPoint].position);
        waitTimer = 0;
    }

    // Update is called once per frame
    void Update () {
        if(waitTimer >= 0)
        {
            waitTimer -= Time.deltaTime;
        }
		if(transform.position == Waypoints[nextPoint].position)
        {
            if(touched == false)
            {
                waitTimer = setTimer;
                touched = true;
            }
            if (waitTimer <= 0)
            {
                touched = false;

                if (nextPoint == Waypoints.Length - 1)
                {
                    nextPoint = 0;
                    transform.LookAt(Waypoints[nextPoint].position);
                }
                else
                {
                    nextPoint += 1;
                    transform.LookAt(Waypoints[nextPoint].position);
                }
            }
        }
        if (MoveToPlayer == true)
        {
            waitTimer = 0;
            transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, Speed * Time.deltaTime);
            transform.LookAt(Player.transform.position);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, Waypoints[nextPoint].position, Speed * Time.deltaTime);
            transform.LookAt(Waypoints[nextPoint].position);
        }
	}
}

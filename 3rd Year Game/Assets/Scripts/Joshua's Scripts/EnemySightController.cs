using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySightController : MonoBehaviour{

    //how the enemy finds and checks and if it can go to the player(Variables).
    public float Radius;
    [Range(0, 360)] public float Angle;
    public LayerMask PlayerMask;
    public LayerMask ObstacleMask;
    public List<Transform> VisibleTargets = new List<Transform>();

    public GameObject Player;
	private bool playerInLight = false;
	public float minDistanceDetection = 3.5f;

    public EnemyController Movement;

    // Use this for initialization
    void Start()
    {

        StartCoroutine("FindTargetDelay", .2f);

    }

    IEnumerator FindTargetDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindTarget();
        }
    }

    void FindTarget()
    {
        VisibleTargets.Clear();
        Collider[] targetsInRadius = Physics.OverlapSphere(transform.position, Radius, PlayerMask);

        for (int i = 0; i < targetsInRadius.Length; i++)
        {
            Transform target = targetsInRadius[i].transform;
            Vector3 direction = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, direction) < Angle / 2)
            {
                float distance = Vector3.Distance(transform.position, target.position);
                if (!Physics.Raycast(transform.position, direction, distance, ObstacleMask))
                {
                    VisibleTargets.Add(target);
                }
            }

        }
    }

    public Vector3 DirFromAngle(float angle, bool globalAngle)
    {
        if (!globalAngle)
        {
            angle += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }

    // Update is called once per frame
    void Update()
    {
		playerInLight = Player.GetComponent<DetectionController> ().isPlayerInLight ();
		float distance = Mathf.Abs(Vector3.Distance(transform.position, Player.transform.position));

		if (VisibleTargets.Contains(Player.transform) && (playerInLight == true ||distance <= minDistanceDetection))
        {
            Movement.MoveToPlayer = true;
			Player.GetComponent<DetectionController> ().playerDetected ();
        }
       /* else
        {
            Movement.MoveToPlayer = false;
			Player.GetComponent<DetectionController> ().playerUndetected ();
        }*/
    }
}

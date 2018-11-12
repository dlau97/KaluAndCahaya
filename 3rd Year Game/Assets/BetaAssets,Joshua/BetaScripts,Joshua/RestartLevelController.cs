using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartLevelController : MonoBehaviour {

    public GameObject Player;
    public Vector3 CheckPointLocation;

	// Use this for initialization
	void Start () {
		CheckPointLocation = Player.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "Player") {
            Player.transform.position = CheckPointLocation;
		}
	}
}

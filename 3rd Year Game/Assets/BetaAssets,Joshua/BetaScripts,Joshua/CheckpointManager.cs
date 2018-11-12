using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour {

    public RestartLevelController LevelController;

	// Use this for initialization
	void Start () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == LevelController.Player)
        LevelController.CheckPointLocation = this.transform.position;
    }

    // Update is called once per frame
    void Update () {
		
	}
}

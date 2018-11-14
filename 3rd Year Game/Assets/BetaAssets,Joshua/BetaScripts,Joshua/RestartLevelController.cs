using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartLevelController : MonoBehaviour {

    public GameObject Player;
    public Vector3 CheckPointLocation;
    public FadeEffectController ActivateFade;
    public bool Fallen = false;

	// Use this for initialization
	void Start () {
		CheckPointLocation = Player.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (Fallen == true)
        {
            if (ActivateFade.fadeInTimer > 0f)
            {
                Player.transform.position = CheckPointLocation;
                Fallen = false;
            }
        }
    }

	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "Player") {
            ActivateFade.Activate = true;
            Fallen = true;
		}
	}
}

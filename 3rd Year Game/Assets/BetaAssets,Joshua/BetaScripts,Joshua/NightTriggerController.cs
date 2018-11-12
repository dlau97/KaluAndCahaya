using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightTriggerController : MonoBehaviour {

    public Light LightSource;
    public GameObject SupportCharacter;
    public GameObject Tree;
    public float SetTimer;
    public float getTimer = 0;
    public GameObject Enemy;
    public MainCharacterController PlayerMovement;
    public float getMoveSpeed;

	// Use this for initialization
	void Start () {
        SupportCharacter.SetActive(false);
        Tree.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (Enemy.activeSelf == true)
        {
            getTimer = SetTimer;
            getMoveSpeed = PlayerMovement.shadowWalkSpeedFactor;
            PlayerMovement.shadowWalkSpeedFactor = 0;
			PlayerMovement.EnableNightMode ();
        }
        SupportCharacter.SetActive(true);
        Tree.SetActive(true);
        Enemy.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
		if(getTimer > 0)
        {
            LightSource.intensity -= 0.2f * Time.deltaTime *3;
            getTimer -= 0.1f * Time.deltaTime;
            if (getTimer <= 0.7)
            {
                PlayerMovement.shadowWalkSpeedFactor = getMoveSpeed;
            }
        }
	}
}

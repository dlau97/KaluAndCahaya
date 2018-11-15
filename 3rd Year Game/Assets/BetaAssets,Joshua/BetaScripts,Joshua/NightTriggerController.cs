﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightTriggerController : MonoBehaviour {

    public Light LightSource;
    public Light IndirectLight;
    public GameObject SupportCharacter;
    public GameObject Tree;
    public float SetTimer;
    public float getTimer = 0;
    public GameObject Enemy;
    public MainCharacterController PlayerMovement;
    public float getMoveSpeed;
    public MoveEnemyController MoveEnemy;
    public Vector3 ThicketEndPos = new Vector3(208.5f, 0.8f, 3.4f);

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
        MoveEnemy.NightTriggered = true;
    }

    // Update is called once per frame
    void Update () {
		if(getTimer > 0)
        {
            LightSource.intensity -= 0.2f * Time.deltaTime * 3;
            IndirectLight.intensity -= 0.2f * Time.deltaTime * 3;
            getTimer -= 0.1f * Time.deltaTime;
            Tree.transform.position = Vector3.MoveTowards(Tree.transform.position, ThicketEndPos, 2 * Time.deltaTime);
            if (getTimer <= 0.7)
            {
                PlayerMovement.shadowWalkSpeedFactor = getMoveSpeed;
                Enemy.SetActive(false);
            }
        }
	}
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BullyController : MonoBehaviour {

	public bool canMoveTowardPlayer = false;
	public float detectionRadius = 6f;

	private float playerDistance;
	private GameObject player;
	private Transform playerT;

	private Ray visualDetectionRay;
	private GameObject centrePRCTarget;

	public float alertTime = 1.2f;
	private float currentAlertTime;

	public GameObject EnemyDetectionUI;
	private StealthManager playerSM; 

	private Vector3 initialRotDirection;
	public float rotationSpeed = 3f;
	public float moveTowardPlayerSpeed = 5f;

	[SerializeField]
	private bool alerted = false;
	[SerializeField]
	private bool gameOver = false;
	private float startGameOverTime;
    public FadeEffectController ActivateFade;

	private Vector3 initialPos;
	public Animator charAnim;

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player");
		playerT = player.transform;
		playerSM = player.GetComponent<StealthManager> ();
		centrePRCTarget = GameObject.Find ("PlayerRayCastTarget9");
		currentAlertTime = alertTime;
		EnemyDetectionUI.SetActive (false);
		initialRotDirection = transform.rotation.eulerAngles;
		initialPos = this.transform.position;
		charAnim.SetInteger ("State", 0);
	}
	
	// Update is called once per frame
	void Update () {
		if (gameOver == true && (Time.time >= startGameOverTime + 2f)) {
			Debug.Log ("GameoVer");
			Vector3 checkpointPos = GameObject.Find ("KillCollider").GetComponent<RestartLevelController>().CheckPointLocation;
			player.transform.position = checkpointPos;
			player.GetComponent<MainCharacterController> ().EnableControls ();
			GameObject.Find ("Support Character").GetComponent<SuppCharController> ().EnableControls ();
			gameOver = false;
			currentAlertTime = alertTime;
			this.transform.position = initialPos;
			charAnim.SetInteger ("State", 0);
		}
		if (alerted == true && gameOver == false) {
			charAnim.SetInteger ("State", 1);
			currentAlertTime -= Time.deltaTime;
			EnemyDetectionUI.gameObject.SetActive (true);
			rotateTowardPlayer ();
			if (currentAlertTime <= 0f) {
				gameOver = true;
				startGameOverTime = Time.time;
				EnemyDetectionUI.gameObject.GetComponent<SpriteRenderer> ().color = Color.red;

				if(canMoveTowardPlayer == true){
					moveTowardPlayer();
					charAnim.SetInteger ("State", 2);
				}
				player.GetComponent<MainCharacterController> ().DisableControls ();
				GameObject.Find ("Support Character").GetComponent<SuppCharController> ().DisableControls ();


                ActivateFade.Activate = true;
			} else {
				EnemyDetectionUI.gameObject.GetComponent<SpriteRenderer> ().color = Color.yellow;
			}

		} 
		else if(alerted == false && gameOver == false){
			//continue surveying
			EnemyDetectionUI.SetActive (false);
			currentAlertTime = alertTime;
			charAnim.SetInteger ("State", 0);
			//rotateToNeutral ();
		}



	}

	void FixedUpdate(){
		checkPlayerDistance ();
	}

	void rotateTowardPlayer(){
		Vector3 playerDir = playerT.position - this.transform.position;
		playerDir.y = 0f;
		transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (playerDir), Time.deltaTime * rotationSpeed);
	}
	void rotateToNeutral(){
		Vector3 dir = initialRotDirection - this.transform.position;
		dir.y = 0f;
		transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (dir), Time.deltaTime * rotationSpeed);
	}
	void moveTowardPlayer(){
		if (playerDistance >= 2f) {
			transform.position = Vector3.MoveTowards (transform.position, playerT.position, moveTowardPlayerSpeed * Time.deltaTime);
		}
	}

	void checkPlayerDistance(){
		playerDistance = Vector3.Magnitude (playerT.position - transform.position);
		//Debug.Log ("Player Distance: " + playerDistance);
		if (playerDistance <= detectionRadius) {


			//CheckIfPlayerInLight
			RaycastHit hit;

			visualDetectionRay = new Ray (transform.position, centrePRCTarget.transform.position - transform.position);
			Debug.DrawLine (transform.position, centrePRCTarget.transform.position, Color.red);

			if (Physics.Raycast (visualDetectionRay, out hit, detectionRadius) && ((hit.transform.gameObject.tag == "PRCTarget") || hit.transform.gameObject.tag == "Player")) {

				bool playerInLight = player.gameObject.GetComponent<StealthManager> ().isPlayerInLight ();
				if (playerInLight == true) {
					alerted = true;
					Debug.DrawLine (hit.point, hit.point + Vector3.up * 2f, Color.green);
				} else {
					alerted = false;
				}

			} else {
				alerted = false;
			}

			//If Player Is In Light - Get Alerted - After alertTime seconds chase player and game over
			//else continue surveying area.
		} else {
			alerted = false;
		}
	}

	void OnDrawGizmosSelected(){
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (transform.position, detectionRadius);
	}
}

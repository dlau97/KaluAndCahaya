﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

[RequireComponent (typeof(Rigidbody))]

public class MainCharacterController : MonoBehaviour
{


	private Rigidbody playerRB;

	private InputDevice controller;

	private float xInput, yInput, zInput;

	public float moveSpeed = 6f;
	public float crawlSpeed = 3f;
	public float shadowWalkSpeedFactor = 0.6f;
	private float actualSpeed;

	public float jumpForce = 3.5f;
	public float fallMultiplier = 2.5f;
	private bool canJump = false;

	private bool isGrounded = true;

	private bool crawling = false;
	private float startCrawlingSquishTime;
	private float squishDuration = 0.25f;

	public float rotationSpeed = 10f;

	private bool jumpRequest = false;
	private bool crawlRequest = false;
	private bool tuckNRollRequest = false;
	private bool disableCrawl = false;
	private bool disableTNR = false;
	private bool disableJump = false;

	private bool rolling = false;
	private float startTNRTime;
	public float TNRSpeedFactor = 12f;
	public float TNRTime = 0.2f;
	public float delayTNRTime = 1.5f;

	private bool controlsDisabled = false;

	public Animator charAnim;
	private bool nightMode = false;

	private GameObject standingCollider;

	[SerializeField]
	private bool underTrunk = false;



	// Use this for initialization
	void Start ()
	{
		controller = InputManager.ActiveDevice;
		actualSpeed = moveSpeed;
		playerRB = this.GetComponent<Rigidbody> ();
		crawling = false;
		standingCollider = GameObject.Find ("StandingCollider");
		standingCollider.SetActive (true);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (controlsDisabled == false) {
			checkOnGround ();
			checkJumpRequest ();
			checkCrawlRequest ();
			checkTuckNRollRequest ();
			checkCrawling ();
			checkRotation ();
		} else {
			charAnim.SetInteger("State", 5); 
			crawling = false;
			standingCollider.SetActive (true);
		}
	}

	void FixedUpdate ()
	{
		if(controlsDisabled == false){
			checkMovement ();
			//checkJump ();
			checkFalling ();
			//checkTuckNRoll ();
		}

	}

	////////////////////////////////////////////////////////////////Primary Functions//////////////////////////////////////////////////////////////

	void checkMovement () //In Fixed Update
	{
		xInput = controller.LeftStick.X;  
		zInput = controller.LeftStick.Y;

		//float walkingSpeedFactor = 1f;
		bool playerInLight = this.gameObject.GetComponent<StealthManager> ().isPlayerInLight ();



		Vector3 movement = Vector3.zero;
		if (xInput == 0f && zInput == 0f) {
			if (crawling == false) {
				charAnim.SetInteger ("State", 0);
			} else {
				charAnim.SetInteger ("State", 8);
			}
		} else {
			if (crawling == false) {
				if (playerInLight == false && nightMode == true) {
					movement = new Vector3 (xInput, 0f, zInput) * Time.deltaTime * actualSpeed * shadowWalkSpeedFactor;
					//charAnim.SetInteger ("State", 4);
					//Debug.Log ("player in shadows");
				} else {
					movement = new Vector3 (xInput, 0f, zInput) * Time.deltaTime * actualSpeed;
					charAnim.SetInteger ("State", 1);
				}
			} else if (crawling == true) {
				movement = new Vector3 (xInput, 0f, zInput) * Time.deltaTime * crawlSpeed;
				charAnim.SetInteger ("State", 7);
			}
			playerRB.MovePosition (transform.position + movement);
		}
	}

	void checkJumpRequest ()
	{
		if (controller.Action1.WasPressed && canJump == true && disableJump == false && jumpRequest == false) {
			jumpRequest = true;
			canJump = false;
		}
	}

	void checkJump ()
	{ //In Fixed Update
		//Check Jump Action
		if (jumpRequest == true) {
			playerRB.AddForce (Vector3.up * jumpForce, ForceMode.Impulse);
			jumpRequest = false;
		} 
	}

	void checkFalling ()
	{
		//check Falling
		if (playerRB.velocity.y < 0f) {
			playerRB.useGravity = false;
			Vector3 gravityForce = Vector3.up * -9.81f * (fallMultiplier);
			playerRB.AddForce (gravityForce, ForceMode.Acceleration);
			playerRB.AddForce (Vector3.up * (-1f) * fallMultiplier/5f, ForceMode.Impulse);

		} else if (playerRB.velocity.y > 0f) {
			playerRB.useGravity = true;

		} else if (playerRB.velocity.y == 0f) {
			playerRB.AddForce (Vector3.up * (-1f) * jumpForce, ForceMode.Impulse);
			playerRB.useGravity = true;
		}
	}

	void checkRotation ()
	{
		xInput = controller.LeftStick.X;  
		zInput = controller.LeftStick.Y;

		if (Mathf.Abs (xInput) > 0f || Mathf.Abs (zInput) > 0f) {

			Vector3 dir = new Vector3 ((-1f) * xInput, 0f, (-1f) * zInput);

			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (dir), Time.deltaTime * rotationSpeed);
		}
	}

	void checkCrawlRequest ()
	{
		if (controller.LeftStickButton.WasPressed == true /*&& disableCrawl == false && crawlRequest == false*/) {
			crawlRequest = true;
		}
	}

	void checkCrawling ()
	{ //In Update
		if (crawlRequest == true) {
			if (crawling == false) {
				charAnim.SetInteger ("State", 6);
				startCrawlingSquishTime = Time.time;
				crawling = true;
				disableJump = true;
				disableTNR = true;
				standingCollider.SetActive (false);
			} else if(crawling == true && underTrunk == false){
				charAnim.SetInteger ("State", 9); //Stand
				crawling = false;
				startCrawlingSquishTime = Time.time;
				disableJump = false;
				disableTNR = false;
				standingCollider.SetActive (true);
			}
			crawlRequest = false;
		} 

		if (crawling == true) {
			//Disable this when applying charcater model
			//ScaleSize (new Vector3 (2.1f, 0.75f, 2.1f));
		} else {
			//ScaleSize (new Vector3 (1.5f, 1.5f, 1.5f));
		}
	}

	void checkTuckNRollRequest ()
	{
		if ((Mathf.Abs (xInput) > 0f || Mathf.Abs (zInput) > 0f) && controller.RightTrigger.WasPressed == true && rolling == false && delayTNRTime <= 0f && disableTNR == false && tuckNRollRequest == false) {
			tuckNRollRequest = true;
		}
	}

	void checkTuckNRoll ()
	{
		if (tuckNRollRequest == true) {
			playerRB.AddForce (Vector3.Normalize(new Vector3 (xInput, 0f, zInput)) * TNRSpeedFactor, ForceMode.Impulse);
			rolling = true;
			startTNRTime = Time.time;

			delayTNRTime = 1f;
			Debug.Log ("TNR");
			disableCrawl = true;
			disableJump = true;
			tuckNRollRequest = false;
		}

		if (Time.time >= startTNRTime + TNRTime && rolling == true) {
			rolling = false;
			playerRB.velocity = Vector3.zero;
			disableCrawl = false;
			disableTNR = false;
		}
		delayTNRTime -= Time.deltaTime;
	}

	////////////////////////////////////////////////////////////////Support Functions//////////////////////////////////////////////////////////////

	void checkOnGround ()
	{

		if (isGrounded == true) {
			canJump = true;
			disableCrawl = false;
			disableTNR = false;
		} else {
			canJump = false;
			disableCrawl = true;
			disableTNR = true;
		}
	}

	public void SetGrounded (bool b)
	{
		isGrounded = b;
	}

	void ScaleSize (Vector3 newScale)
	{

		float t = (Time.time - startCrawlingSquishTime) / squishDuration;

		this.transform.localScale = Vector3.Lerp (this.transform.localScale, newScale, t);
	}

	public float toDegrees (float radians)
	{
		return radians * (180 / Mathf.PI);
	}

	public void DisableControls(){
		controlsDisabled = true;
	}
	public void EnableControls(){
		controlsDisabled = false;
	}
	public void EnableNightMode(){
		nightMode = true;
	}
	public void UnderTrunk(bool b){
		underTrunk = b;
	}

		


}

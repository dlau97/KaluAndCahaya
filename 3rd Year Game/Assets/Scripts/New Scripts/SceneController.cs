﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using InControl;

public class SceneController : MonoBehaviour {

	private InputDevice controller;

	void Start(){
		controller = InputManager.ActiveDevice;
	}

	public void onClickMainMenu(){
		SceneManager.LoadScene ("Main Menu Scene");
	}

	public void onClickDemoScene(){
		SceneManager.LoadScene ("Final Demo Scene");
	}

	public void onClickControlsScene(){
		SceneManager.LoadScene ("Controls Scene");
	}

	public void onClickQuitGame(){
		Application.Quit ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape) || controller.Action4.WasPressed) {
			if (SceneManager.GetActiveScene ().name == "Final Demo Scene" || SceneManager.GetActiveScene ().name == "Controls Scene") {
				SceneManager.LoadScene ("Main Menu Scene");
			} else {
				Application.Quit ();
			}
		}
	}



	public void RestartLevel(){
		Scene thisScene = SceneManager.GetActiveScene ();
		SceneManager.LoadScene (thisScene.name);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class GameManager : MonoBehaviour {


	#region

	public static GameManager instance;

	void Awake(){
		instance = this;
	}

	#endregion

	public GameObject player;
}

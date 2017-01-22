using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour {

	public string SceneToStart;
	// Use this for initialization
	void OnTriggerEnter(Collider other){
		SceneManager.LoadScene (SceneToStart);
	}
}

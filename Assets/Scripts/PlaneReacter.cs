using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneReacter : MonoBehaviour {
	void OnTriggerEnter(Collider other){
		Debug.Log ("It's a hit");
	}
	void OnTriggerStay(Collider other){
		Debug.Log ("Still here");
	}
	void OnTriggerExit(Collider other){
		Debug.Log ("And it went");
	}
}

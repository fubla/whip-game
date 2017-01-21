using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneReacter : MonoBehaviour {
	void OnCollisionEnter(Collision collision){
		Debug.Log ("It's a hit");
	}
}

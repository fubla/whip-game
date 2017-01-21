using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhipController : MonoBehaviour {

	public Transform whipShaft;
	public Vector2 scaleFactor;
	public float riseSpeed;
	public float lowerSpeed; 

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 mousePosition = Input.mousePosition;
		Vector2 mouseRelative = new Vector2(0.0f, 0.0f);
		mouseRelative.x = (mousePosition.x - Screen.width / 2.0f) / Screen.width;
		mouseRelative.y = (mousePosition.y - Screen.height / 2.0f) / Screen.height;
		whipShaft.position = Vector2.Scale(scaleFactor, mouseRelative);


	}
}

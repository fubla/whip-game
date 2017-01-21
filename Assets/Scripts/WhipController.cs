using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhipController : MonoBehaviour {

	public Transform whipShaft;
	public Vector2 scaleFactor;
	public float riseSpeed;
	public float lowerSpeed; 

	private float maxAngle;
	// Use this for initialization
	void Start () {
		maxAngle = 90;
	}
	
	// Update is called once per frame
	void Update () {
		
		Vector3 mousePosition = Input.mousePosition;
		Vector2 mouseRelative = new Vector2(0.0f, 0.0f);
		mouseRelative.x = (mousePosition.x - Screen.width / 2.0f) / Screen.width;
		mouseRelative.y = (mousePosition.y - Screen.height / 2.0f) / Screen.height;

		whipShaft.localPosition = Vector2.Scale(scaleFactor, mouseRelative);

		float angle = whipShaft.transform.rotation.eulerAngles.x;
		Debug.Log (angle);

		if (Input.GetMouseButton (0)) {
			if (angle > 0) {
		        Vector3 pos = whipShaft.transform.position;
		        Vector3 axis = Vector3.right;
		        float amount = -riseSpeed * Time.deltaTime;
		        whipShaft.transform.RotateAround (pos, axis, amount);
			}
			if (angle < maxAngle)
				maxAngle = angle;
		} else {
			if (angle < 90 || angle < 180 - maxAngle) {
				Vector3 pos = whipShaft.transform.position;
				Vector3 axis = Vector3.right;
				float amount = lowerSpeed * Time.deltaTime;
				whipShaft.transform.RotateAround (pos, axis, amount);
			} else {
                maxAngle = 90;
            }
		}

	}
}

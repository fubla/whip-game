﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhipController : MonoBehaviour {
	
	public Camera playerCamera;
	public Transform WhipRoot;
	public Animator animator;
	private int attackHash;

	public Vector3 scaleOffset; 

	public Vector2 panLimits;
	public Vector2 panSpeed;
	public Vector2 panBoundaries;
	public AudioSource audioSource;

	void Start () {
		attackHash = Animator.StringToHash ("Attack");
	}
	
	// Update is called once per frame
	void Update () {
		// Whip position
		Vector3 mouse = Input.mousePosition;
		mouse.x = (mouse.x / Screen.width - 0.5f) * 2.0f;
		mouse.y = (mouse.y / Screen.height - 0.5f) * 2.0f;
		mouse.z = 0.0f;
		Vector3 translation = new Vector3 (mouse.x, mouse.y / Mathf.Sqrt (2.0f), mouse.y / Mathf.Sqrt (2.0f)); 
		WhipRoot.transform.localPosition = Vector3.Scale (translation, scaleOffset);

		//Camera Pan
		if(mouse.x > panLimits.x)
			transform.position += Vector3.right * (panSpeed.x * (mouse.x - panLimits.x) / (2.0f - panLimits.x));
		if(mouse.x < -panLimits.x)
			transform.position += Vector3.right * (panSpeed.x * (mouse.x + panLimits.x) / (2.0f - panLimits.x));
		if(mouse.y > panLimits.y)
			transform.position += Vector3.forward * (panSpeed.y * (mouse.y - panLimits.y) / (2.0f - panLimits.y));
		if(mouse.y < -panLimits.y)
			transform.position += Vector3.forward * (panSpeed.y * (mouse.y + panLimits.y) / (2.0f - panLimits.y));

		Vector3 position = transform.position;
		if (position.x > panBoundaries.x)
			position.x = panBoundaries.x;
		if (position.x < -panBoundaries.x)
			position.x = -panBoundaries.x;
		if (position.z > panBoundaries.y)
			position.z = panBoundaries.y;
		if (position.z < -panBoundaries.y)
			position.z = -panBoundaries.y;
		transform.position = position;

		RaycastHit hit;
		Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast (ray, out hit)) {
			WhipRoot.transform.LookAt (hit.point);
		}

		if (Input.GetMouseButtonDown (0)) {
			audioSource.Play();
			animator.SetTrigger (attackHash);
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pleb : MonoBehaviour {

	public enum STATE
	{
		IDLE,
		PANICKED,
		BUILDING,
		DESTROYING,
		MOVING
	}
		

	public float timeStep = .01f;
	public STATE state;

	private Vector3 velocity;

	// Use this for initialization
	void Start () {
		state = STATE.MOVING;
		velocity = new Vector3 (0,0,0);
	}
	
	// Update is called once per frame
	void Update () {
		Random.InitState (Time.frameCount);
		if (state == STATE.IDLE)
			velocity = new Vector3 ();
		else if (state == STATE.PANICKED)
			velocity = new Vector3 (Random.Range (-10.0f, 10.0f), 0, Random.Range (-10.0f, 10.0f));
		else if (state == STATE.MOVING) {
			transform.position += timeStep * velocity;
		}
	}

	public Vector3 GetVelocity(){
		return velocity;
	}

	public void SetVelocity(Vector3 velocity){
		this.velocity = velocity;
	}
}

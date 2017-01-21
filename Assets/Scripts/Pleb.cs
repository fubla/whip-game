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
	public float dampening = 0.99f;
	public bool dampen;
	public STATE state;

	public Vector3 velocity;

	// Use this for initialization
	void Start () {
		dampen = false;
		state = STATE.MOVING;
		velocity = new Vector3 (Random.Range (-1.0f, 1.0f), 0, Random.Range (-1.0f, 1.0f));
	}
	
	// Update is called once per frame
	void Update () {
		Random.InitState (Time.frameCount);
		if (state == STATE.PANICKED){
			transform.position += timeStep * velocity;
			velocity = new Vector3 (Random.Range (-1.0f, 1.0f), 0, Random.Range (-1.0f, 1.0f));
		}
		else if (state == STATE.MOVING) {
			transform.position += timeStep * velocity;
			velocity *= dampen ? dampening : 1;
		}
	}

	public Vector3 GetVelocity(){
		return velocity;
	}

	public void SetVelocity(Vector3 velocity){
		this.velocity = velocity;
	}
}

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
		
	public float dampening = 0.90f;
	public bool dampen;
	public STATE state;
	private SpriteRenderer sr;
	private int panickCounter;


	public Vector3 velocity;
	Animator animator;
	// Use this for initialization
	void Start () {
		sr = GetComponent<SpriteRenderer> ();
		animator = GetComponent<Animator> ();
		animator.SetFloat ("Speedvar", Random.Range (0.5f, 1.0f));
		dampen = false;
		state = STATE.MOVING;
		velocity = new Vector3 (Random.Range (-1.0f, 1.0f), 0, Random.Range (-1.0f, 1.0f));
	}
	
	// Update is called once per frame
	void Update () {
		Random.InitState (Time.frameCount);
		if (state == STATE.PANICKED) {
			animator.SetBool ("Moving", true);
			velocity = new Vector3 (Random.Range (-1.0f, 1.0f), 0, Random.Range (-1.0f, 1.0f));
			transform.position += velocity;
			if (panickCounter-- <= 0) {
				panickCounter = 0;
				state = STATE.IDLE;
			}

		} else if (state == STATE.MOVING) {
			bool flipX = velocity.x < 0;
			animator.SetBool ("Moving", true);

			if (Mathf.Abs (velocity.x) < 0.2f && Mathf.Abs (velocity.z) < 0.2f) {
				velocity = new Vector3 (0, 0, 0);
				animator.SetBool ("Moving", false);
				state = STATE.IDLE;
			}
			transform.position += velocity;
			sr.flipX = flipX;
			velocity *= dampen ? dampening : 1;
		} else if (state == STATE.IDLE) {
			if (Mathf.Abs (velocity.x) >= 0.2f || Mathf.Abs (velocity.z) >= 0.2f) {
				state = STATE.MOVING;
			}
		} else if (state == STATE.BUILDING) {
			animator.SetBool ("Building", true);

		}else if (state == STATE.DESTROYING) {
			animator.SetBool ("Destroying", true);

		}
	}

	public Vector3 GetVelocity(){
		return velocity;
	}

	public void SetVelocity(Vector3 velocity){
		this.velocity = velocity;
	}
}

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
			animator.SetBool ("Idle", false);
			animator.SetBool ("Moving", true);
			transform.position += velocity;
			velocity = new Vector3 (Random.Range (-1.0f, 1.0f), 0, Random.Range (-1.0f, 1.0f));
		} else if (state == STATE.MOVING) {
			bool flipX = velocity.x < 0;
			animator.SetBool ("Idle", false);
			animator.SetBool ("Moving", true);
			transform.position += velocity;

			if (Mathf.Abs(velocity.x) < 0.2f && Mathf.Abs(velocity.z) < 0.2f) {
				velocity = new Vector3 ();
				state = STATE.IDLE;
			}
			sr.flipX = flipX;
			velocity *= dampen ? dampening : 1;
		} else if (state == STATE.IDLE) {
			animator.SetBool ("Idle", true);
			animator.SetBool ("Moving", false);
			if (Mathf.Abs (velocity.x) >= 0.3f || Mathf.Abs (velocity.z) >= 0.3f) {
				state = STATE.MOVING;
			}
		}
	}

	public Vector3 GetVelocity(){
		return velocity;
	}

	public void SetVelocity(Vector3 velocity){
		this.velocity = velocity;
	}


}

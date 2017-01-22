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

	public float dampening = 0.9f;
	public bool dampen;
	public STATE state;
	private SpriteRenderer sr;
	public int morale;
	public bool scatter;


	public Vector3 velocity;
	Animator animator;
	// Use this for initialization
	void Start () {
		morale = 100;
		scatter = false;
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
		if (morale++ > 100)
			morale = 100;
		else if (morale <= 0) {
			state = STATE.PANICKED;
			animator.SetBool ("Moving", true);
		}

		if(morale < -100)
			morale = -100;
		
		if (state == STATE.PANICKED) {
			scatter = true;
			velocity.y = 0;
			transform.position += velocity;

			if (morale > 0) {
				state = STATE.IDLE;
				scatter = false;
				velocity = new Vector3 (0, 0, 0);
				animator.SetBool ("Moving", false);
			}

		} else if (state == STATE.MOVING) {
			bool flipX = velocity.x < 0;
			if (velocity.magnitude < 0.05f) {
				velocity = new Vector3 (0, 0, 0);
				state = STATE.IDLE;
				animator.SetBool ("Moving", false);
			} else {
				velocity *= dampen ? dampening : 1;
				velocity.y = 0;
				transform.position += velocity;
				sr.flipX = flipX;
			}
		} else if (state == STATE.IDLE) {
			if (velocity.magnitude >= 0.05f) {
				state = STATE.MOVING;
				animator.SetBool ("Moving", true);
			}
			else {
				velocity = new Vector3 (0, 0, 0);
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

	public int GetMorale()
	{
		return morale;
	}

	public void SetMorale(int morale){
		this.morale = morale;
	}

	public bool GetScatter(){
		return scatter;
	}

	public STATE GetState(){
		return state;
	}
}

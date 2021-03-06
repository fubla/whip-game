﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlebFlocker : MonoBehaviour {

	public GameObject[] plebs;
	public GameObject plebPrefab;
	public List<Vector3> horrors;
	public List<float> horrorDurations;
	public List<Transform> targets;
	public Transform fleeFrom;
	public int numPlebs = 20;
	private int remaining; 		// remaining plebs (for gameover condition)
	public float speedLim = .5f; 
	public float factor1 = 1000f; // plebs seek center of mass of their population
	public float factor2 = 2f; // collision distance
	public float factor3 = 10f;	// alignment with average velocity vector
	public float factor4 = 800f;	// seek target amount
	public float factor5 = 1f;		// avoid scary stuff amount
	public float repellant = 2f;	// amount of repellant "force" from scary stuff

	public float horrorFactor = 1000f;
	public float horrorDistance = 10f;
	public float horrorDecay = .5f;

	public float buildDistance = 5.0f;

	public int xMax, zMax, xMin, zMin;


	public float cohesionFac = .1f;  //how much plebs repel each other


	// Use this for initialization
	void Start () {
		plebs = new GameObject[numPlebs];
		SpawnPlebs (numPlebs);
		remaining = numPlebs;
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < numPlebs; i++) {
			if (plebs [i].transform.position.x >= xMax ||
			    plebs [i].transform.position.x <= xMin ||
			    plebs [i].transform.position.z >= zMax ||
			    plebs [i].transform.position.z <= zMin) {
				remaining--;
				plebs [i].SetActive (false);
			} else if (plebs[i].activeSelf){
				Pleb pleb = plebs [i].GetComponent<Pleb> ();
				Vector3 v1 = (pleb.GetScatter() ? -1 : 1) * Rule1 (plebs [i]);
				Vector3 v2 = Rule2 (plebs [i]);
				Vector3 v3 = Rule3 (plebs [i]);
				Vector3 v4 = (pleb.GetScatter() ? new Vector3(0,0,0) : Rule4 (plebs [i]));
				Vector3 v5 = Rule5 (plebs [i]);
				Vector3 v = pleb.GetVelocity ();
				int moraleImpact = MoraleImpact (plebs [i]);
				CheckAndBuild (plebs [i]);
				CheckHorrors ();
				pleb.SetMorale (pleb.GetMorale () + moraleImpact);
				pleb.SetVelocity (LimitSpeed (v + v1 + v2 + v3 + v4 + v5));
			}
		}
	}

	void SpawnPlebs(int numPlebs){
		Random.InitState (Time.frameCount);
		for (int i = 0; i < numPlebs; i++) {
			
				plebs [i] = Instantiate (plebPrefab, new Vector3 (Random.Range (-50.0f, 50.0f), 2f, Random.Range (-50.0f, 50.0f)), Quaternion.identity);
				plebs [i].GetComponent<Pleb> ().SetVelocity (new Vector3 (Random.Range (-1.0f, 1.0f), 0, Random.Range (-1.0f, 1.0f)));

		}
	}

	Vector3 Rule1(GameObject pleb){
		Vector3 pcj = new Vector3 ();
		for (int i = 0; i < numPlebs; i++) {
			if (!plebs [i].Equals (pleb) && plebs [i].activeSelf) {
				pcj += plebs [i].transform.position;
			}
		}

		pcj /= (numPlebs - 1);
		return (pcj - pleb.transform.position) / factor1;
	}

	Vector3 Rule2(GameObject pleb){
		Vector3 c = new Vector3 ();
		for (int i = 0; i < numPlebs; i++) {
			if (!plebs [i].Equals (pleb) && (pleb.transform.position - plebs[i].transform.position).magnitude < factor2 && plebs [i].activeSelf) {
				c -= (plebs [i].transform.position - pleb.transform.position);
			}
		}
		return cohesionFac * c;
	}

	Vector3 Rule3(GameObject pleb){
		Vector3 pvj = new Vector3 ();
		for (int i = 0; i < numPlebs; i++) {
			if (!plebs [i].Equals (pleb) && plebs [i].activeSelf) {
				pvj += plebs [i].GetComponent<Pleb>().GetVelocity();
			}
		}

		pvj /= (numPlebs - 1);
		return (pvj - pleb.GetComponent<Pleb>().GetVelocity()) / factor3;
	}

	Vector3 Rule4(GameObject pleb){		//seek target
		Vector3 whereTo = new Vector3();
		float distance = float.MaxValue;
		foreach (Transform target in targets){
			float tmpDist = (target.position - pleb.transform.position).magnitude;
			if (tmpDist < distance) {
				distance = tmpDist;
				whereTo = target.position - pleb.transform.position;
				if (whereTo.magnitude < 10) {
					pleb.GetComponent<Pleb> ().dampen = true;
				} else if (pleb.GetComponent<Pleb> ().dampen == true) {
					pleb.GetComponent<Pleb> ().dampen = false;
				}
			}
		} 
		return whereTo / factor4;
	}

	Vector3 Rule5(GameObject pleb){	//flee from target
		if(fleeFrom){
				Vector3 targetPos = fleeFrom.position;
				Vector3 whereTo = targetPos - pleb.transform.position;
				return -repellant * whereTo / (factor5 * Mathf.Pow (whereTo.magnitude, 2));
			}
		else
			return new Vector3 (0, 0, 0);
	}

	Vector3 LimitSpeed(Vector3 v){
		if (v.magnitude > speedLim)
			return speedLim * v.normalized;
		else
			return v;
	}

	public int GetRemaining(){
		return remaining;
	}

	int MoraleImpact(GameObject pleb){
		int moraleImpact = 0;

		for (int i = 0; i < numPlebs; i++) {
			float distance = (plebs[i].transform.position - pleb.transform.position).magnitude;
			if (!plebs [i].Equals (pleb) 
				&& plebs [i].activeSelf 
				&& distance <= 5) 
			{
				int morale = plebs [i].GetComponent<Pleb> ().GetMorale ();
				moraleImpact += (int)( (morale < 0 ? morale : 0) / (distance*distance));
			}

		}
		foreach (Vector3 horror in horrors) {
			if ((horror - pleb.transform.position).magnitude <= horrorDistance) {
				moraleImpact -= (int)((horrorFactor / (horror - pleb.transform.position).magnitude));
			}
		}
		return moraleImpact;
	}

	public int CountBuilders(Vector3 location, float radius){
		int count = 0;
		for (int i = 0; i < numPlebs; i++) {
			float distance = (plebs[i].transform.position - location).magnitude;
			if (plebs [i].activeSelf && distance <= radius && plebs[i].GetComponent<Pleb>().GetState() == Pleb.STATE.BUILDING)
				count++;
		}
		return count;
	}

	public int CountDestroyers(Vector3 location, float radius){
		int count = 0;
		for (int i = 0; i < numPlebs; i++) {
			float distance = (plebs[i].transform.position - location).magnitude;
			if (plebs [i].activeSelf && distance <= radius && plebs[i].GetComponent<Pleb>().GetState() == Pleb.STATE.DESTROYING)
				count++;
		}
		return count;
	}
		
	public void AddTerror(Vector3 location, float duration){
		horrors.Add (location);
		horrorDurations.Add (duration);
	}



	void CheckAndBuild(GameObject pleb){
		foreach (Transform target in targets) {
			float distance = (target.position - pleb.transform.position).magnitude;
			if (distance <= buildDistance) {
				if (target.gameObject.GetComponent<Tile> ().tileStage == Tile.TILE_STAGE.BLUEPRINT) {
					pleb.GetComponent<Pleb> ().SetState (Pleb.STATE.BUILDING);
				} else if (target.gameObject.GetComponent<Tile> ().tileStage == Tile.TILE_STAGE.BUILD) {
					pleb.GetComponent<Pleb> ().SetState (Pleb.STATE.DESTROYING);
				} else {
					pleb.GetComponent<Pleb> ().SetState (Pleb.STATE.MOVING);
				}
			}
		}
	}

	void CheckHorrors(){
		for(int i = 0; i < horrors.Count; i++){
			if (horrorDurations [i] <= 0) {
				horrorDurations.RemoveAt (i);
				horrors.RemoveAt (i);
			} else
				horrorDurations [i] -= Time.deltaTime;
		}
	}
}

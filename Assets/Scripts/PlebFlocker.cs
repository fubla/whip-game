using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlebFlocker : MonoBehaviour {

	public GameObject[] plebs;
	public GameObject plebPrefab;
	public Transform target;
	public Transform fleeFrom;
	public int numPlebs = 10;
	private int remaining; 		// remaining plebs (for gameover condition)
	public float speedLim = .1f; 
	public float factor1 = 100f; // plebs seek center of mass of their population
	public float factor2 = 1.5f; // collision distance
	public float factor3 = 8f;	// alignment with average velocity vector
	public float factor4 = 100f;	// seek target amount
	public float factor5 = 10f;		// avoid scary stuff amount
	public float repellant = 100f;	// amount of repellant "force" from scary stuff

	public int xMax, zMax, xMin, zMin;


	public float cohesionFac = .05f;  //how much plebs repel each other


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
				Vector3 v1 = Rule1 (plebs [i]);
				Vector3 v2 = Rule2 (plebs [i]);
				Vector3 v3 = Rule3 (plebs [i]);
				Vector3 v4 = Rule4 (plebs [i]);
				Vector3 v5 = Rule5 (plebs [i]);
				Vector3 v = plebs [i].GetComponent<Pleb> ().GetVelocity ();
				plebs [i].GetComponent<Pleb> ().SetVelocity (LimitSpeed (v + v1 + v2 + v3 + v4 + v5));
			}
		}
	}

	void SpawnPlebs(int numPlebs){
		Random.InitState (Time.frameCount);
		for (int i = 0; i < numPlebs; i++) {
			
				plebs [i] = Instantiate (plebPrefab, new Vector3 (Random.Range (-10.0f, 10.0f), 2f, Random.Range (-10.0f, 10.0f)), Quaternion.identity);
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
		Vector3 targetPos = target.position;
		Vector3 whereTo = targetPos - pleb.transform.position;
		if (whereTo.magnitude < 10) {
			pleb.GetComponent<Pleb> ().dampen = true;
		} else if (pleb.GetComponent<Pleb> ().dampen == true){
			pleb.GetComponent<Pleb> ().dampen = false;
		}
		return whereTo / factor4;
	}

	Vector3 Rule5(GameObject pleb){		//flee from target
		Vector3 targetPos = fleeFrom.position;
		Vector3 whereTo = targetPos - pleb.transform.position;
		return -repellant * whereTo / (factor5 * Mathf.Pow(whereTo.magnitude, 2));
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
}

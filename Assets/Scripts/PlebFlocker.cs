using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlebFlocker : MonoBehaviour {

	public GameObject[] plebs;
	public GameObject plebPrefab;
	public int numPlebs = 10;
	public int factor1 = 100;
	public int factor2 = 100;
	public int factor3 = 8;

	// Use this for initialization
	void Start () {
		plebs = new GameObject[numPlebs];
		SpawnPlebs (numPlebs);
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < numPlebs; i++) {
			Vector3 v1 = Rule1 (plebs[i]);
			Vector3 v2 = Rule2 (plebs[i]);
			Vector3 v3 = Rule3 (plebs[i]);
			Vector3 v = plebs [i].GetComponent<Pleb> ().GetVelocity();
			plebs [i].GetComponent<Pleb> ().SetVelocity (v + v1 + v2 + v3);
		}
	}

	void SpawnPlebs(int numPlebs){
		Random.InitState (Time.frameCount);
		for (int i = 0; i < numPlebs; i++) {
			plebs [i] = Instantiate (plebPrefab, new Vector3 (Random.Range (-10.0f, 10.0f), 0, Random.Range (-10.0f, 10.0f)), Quaternion.identity);
			plebs [i].GetComponent<Pleb> ().SetVelocity (new Vector3 (Random.Range (-1.0f, 1.0f), 0, Random.Range (-1.0f, 1.0f)));
		}
	}

	Vector3 Rule1(GameObject pleb){
		Vector3 pcj = new Vector3 ();
		for (int i = 0; i < numPlebs; i++) {
			if (!plebs [i].Equals (pleb)) {
				pcj += plebs [i].transform.position;
			}
		}

		pcj /= (numPlebs - 1);
		return (pcj - pleb.transform.position) / factor1;
	}

	Vector3 Rule2(GameObject pleb){
		Vector3 c = new Vector3 ();
		for (int i = 0; i < numPlebs; i++) {
			if (!plebs [i].Equals (pleb) && (pleb.transform.position - plebs[i].transform.position).magnitude < factor2) {
				c -= (plebs [i].transform.position - pleb.transform.position);
			}
		}

		return c;
	}

	Vector3 Rule3(GameObject pleb){
		Vector3 pvj = new Vector3 ();
		for (int i = 0; i < numPlebs; i++) {
			if (!plebs [i].Equals (pleb)) {
				pvj += plebs [i].GetComponent<Pleb>().GetVelocity();
			}
		}

		pvj /= (numPlebs - 1);
		return (pvj - pleb.GetComponent<Pleb>().GetVelocity()) / factor3;
	}
}

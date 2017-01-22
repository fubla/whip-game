using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMapGenerator : MonoBehaviour {
	public GameObject[] tiles;
	public float[] tilesWeights;
	public int size;
	public float scaleOffset;
	public GameObject borderTile;

	// Use this for initialization
	void Start () {
		float totalWeight = 0.0f;
		foreach (float weight in tilesWeights) {
			totalWeight += weight;
		}

		for (int i = 0; i < size; ++i) {
			for (int j = 0; j < size; ++j) {
				float weightPoint = Random.Range(0.0f, totalWeight);
				int r = 0;
				while (weightPoint > 0.0f) {
					weightPoint -= tilesWeights[r];
					r++;
				}
				Vector3 pos = transform.position + scaleOffset * (Vector3.right * i + Vector3.forward * j);
				GameObject tile = Instantiate(tiles[r-1], pos, Quaternion.identity) as GameObject;
				tile.transform.parent = transform;
			}
		}

		for (int i = -1; i < size + 1; ++i){
			int j1 = -1;
			int j2 = size;
			Vector3 pos1 = transform.position + scaleOffset * (Vector3.right * i + Vector3.forward * j1);
			GameObject tile1 = Instantiate(borderTile, pos1, Quaternion.identity) as GameObject;
			tile1.transform.parent = transform;
		
			Vector3 pos2 = transform.position + scaleOffset * (Vector3.right * i + Vector3.forward * j2);
			GameObject tile2 = Instantiate(borderTile, pos2, Quaternion.identity) as GameObject;
			tile2.transform.parent = transform;
		}
		for (int j = 0; j < size; ++j){
			int i1 = -1;
			int i2 = size;
			Vector3 pos1 = transform.position + scaleOffset * (Vector3.right * i1 + Vector3.forward * j);
			GameObject tile1 = Instantiate(borderTile, pos1, Quaternion.identity) as GameObject;
			tile1.transform.parent = transform;

			Vector3 pos2 = transform.position + scaleOffset * (Vector3.right * i2 + Vector3.forward * j);
			GameObject tile2 = Instantiate(borderTile, pos2, Quaternion.identity) as GameObject;
			tile2.transform.parent = transform;
		}
	}
	void Update () {
		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMapController : MonoBehaviour {
    public GameObject[] tiles;
    public float[] tilesWeights;
    public int size;
	public float scaleOffset;
	
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
				Vector3 pos = transform.position + scaleOffset * (Vector3.right * (i+0.5f) + Vector3.forward * (j+0.5f));
				GameObject tile = Instantiate(tiles[r-1], pos, Quaternion.identity) as GameObject;
				tile.transform.parent = transform;
			}
        }
	}

	void Update () {
		
	}
}

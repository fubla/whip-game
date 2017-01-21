using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureTile : MonoBehaviour {

	public enum TEXTURE_TYPE {TEXTURE1, TEXTURE2, TEXTURE3};
	const int dim = 5;
	public TEXTURE_TYPE texture;
	public GameObject[] tiles;
	public GameObject tilePrefab;

	// Use this for initialization
	void Start () {
		Random.InitState (Time.frameCount);
		texture = (TEXTURE_TYPE)Random.Range ((int)TEXTURE_TYPE.TEXTURE1, (int)TEXTURE_TYPE.TEXTURE2 + 1);
		tiles = new GameObject[dim*dim];
		for (int x = 0; x < dim; x++) {
			for (int y = 0; y < dim; y++) {
				tiles [y * dim + x] = Instantiate (
					tilePrefab, 
					new Vector3(transform.position.x + x, transform.position.y + y, 0), 
					Quaternion.identity, 
					transform);
			}
		}
	}

	GameObject GetTile(int x, int y){
		return tiles [y * dim + x];
	}

	// Update is called once per frame
	void Update () {
		
	}
}

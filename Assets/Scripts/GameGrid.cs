using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameGrid : MonoBehaviour {

	public int gridsXY = 5;
	public int tilesPerTextureTile = 5;
	public GameObject textureTilePrefab;
	public GameObject[] textureTiles;

	// Use this for initialization
	void Start () {
		textureTiles = new GameObject[gridsXY * gridsXY];
		for (int x = 0; x < gridsXY; x++) {
			for (int y = 0; y < gridsXY; y++) {
				textureTiles [y * gridsXY + x] = Instantiate (
					textureTilePrefab, 
					new Vector3(transform.position.x + x*tilesPerTextureTile, transform.position.y + y*tilesPerTextureTile, 0), 
					Quaternion.identity,
					transform);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	GameObject GetTextureTile(int x, int y){
		return textureTiles [y * gridsXY + x];
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

	public enum TILE_TYPE {UNBUILDABLE, BADLY_BUILDABLE, BUILDABLE};
	public enum BUILDING_TYPE {NONE, FIRST, SECOND, THIRD};
	public BUILDING_TYPE buildingType;
	public TILE_TYPE tileType;
	// Use this for initialization

	void Start () {
		Random.InitState (Time.frameCount);
		tileType = (TILE_TYPE)Random.Range ((int)TILE_TYPE.UNBUILDABLE, (int)TILE_TYPE.BUILDABLE + 1);
		buildingType = BUILDING_TYPE.NONE;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

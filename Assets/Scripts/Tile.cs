using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

	public bool passable = true;
	public bool buildable = true;

	public enum TILE_STAGE {WATER, GRASS, SAND, BLUEPRINT, BUILD, DESTROYED};
	public TILE_STAGE tileStage;

	public float whippingToImprove = 2.0f;
	public float whippingDecay = 0.5f;

	public float buildingWork = 5.0f;
	public float buildingDecay = 0.5f;
	public float buildRadius = 0.5f;

	public float buildingHealt = 10.0f;
	public float healthRegen = 0.9f;

	public Material grass;
	public Material sand;
	public Material blueprint;
	public Material build;
	public Material destroyed;

	public GameObject Building;
	public GameObject Blueprint;
	public GameObject Ruin;

	private MeshRenderer localRenderer;

	private BlockMapGenerator map;
	private PlebFlocker flocker;

	private float whippingScore = 0.0f;
	private float buildScore = 0.0f;
	private float damage = 0.0f;

	void Start () {
		GameObject mapObject = GameObject.Find ("Map"); 
		map = mapObject.GetComponent<BlockMapGenerator> ();
		flocker = mapObject.GetComponent<PlebFlocker> ();

		localRenderer = GetComponent<MeshRenderer> ();

		foreach (Transform child in transform)
		{
			if (child.tag == "BuildingRuin")
				Ruin = child.gameObject;
			if (child.tag == "BuildingDone")
				Building = child.gameObject;
			if (child.tag == "BuildingBlueprint")
				Blueprint = child.gameObject;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (whippingScore > 0.0f)
			whippingScore -= (Time.deltaTime * whippingDecay);
		if (tileStage == TILE_STAGE.SAND && whippingScore < whippingToImprove) {
			tileStage = TILE_STAGE.GRASS;
			localRenderer.material = grass;
		}
		if (tileStage == TILE_STAGE.BLUEPRINT){
			if (buildScore > 0.0f)
				buildScore -= buildingDecay * Time.deltaTime;
			int buildingPlebs = flocker.CountBuilders (transform.position, buildRadius);
			buildScore += Time.deltaTime * buildingPlebs;
			if (buildScore > buildingWork) {
				tileStage = TILE_STAGE.BUILD;
				localRenderer.material = build;
				Blueprint.SetActive (false);
				Building.SetActive (true);
			}
				
		}
		if (tileStage == TILE_STAGE.BUILD) {
			if (damage > 0.0f)
				damage -= healthRegen * Time.deltaTime;
			int destroyPlebs = flocker.CountDestroyers (transform.position, buildRadius);
			damage += destroyPlebs * Time.deltaTime;
			if (damage > buildingHealt) {
				tileStage = TILE_STAGE.DESTROYED;
				Building.SetActive (false);
				Ruin.SetActive (true);
				flocker.targets.Remove (transform);
			}
		}
	}

	void OnTriggerEnter(Collider other){
		whippingScore += 1.0f;
		if(tileStage == TILE_STAGE.GRASS && whippingScore > whippingToImprove){
			tileStage = TILE_STAGE.SAND;
			localRenderer.material = sand;
		}
		if (tileStage == TILE_STAGE.SAND && whippingScore > whippingToImprove * 2) {
			tileStage = TILE_STAGE.BLUEPRINT;
			localRenderer.material = blueprint;
			flocker.targets.Add (transform);
			Blueprint.SetActive (true);
		}
		flocker.AddTerror (transform.position, 2000.0f);
	}
}

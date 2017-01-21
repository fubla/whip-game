using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WhipRopeRenderer : MonoBehaviour {
	public Transform start;
	public Rigidbody startBody;
	public Transform end;
	public LineRenderer renderer;

	public int segments;

	void Start() {
		if (renderer) {
			renderer.numPositions = segments;
			for (int i = 0; i < segments; ++i) {
				Vector3 pos = start.position + ((float)i / (float)(segments-1)) * (end.position - start.position);
				renderer.SetPosition(i, pos);
			}
		}
	}

	void Update() {
		Vector3 v = startBody.velocity;

		if (renderer) {
			renderer.numPositions = segments;
			for (int i = 0; i < segments; ++i) {
				Vector3 current = renderer.GetPosition (i);
				Vector3 target = start.position + ((float)i / (float)(segments-1)) * (end.position - start.position);

				renderer.SetPosition(i, 0.5f * (current + target));
			}
		}
	}

}
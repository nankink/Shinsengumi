using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof (FieldOfView))]
public class FieldOfViewEditor : Editor {

	void OnSceneGUI() {
		FieldOfView fow = (FieldOfView)target;
		Handles.color = Color.white;
		Handles.DrawWireArc (fow.transform.position, Vector3.forward, Vector3.up, 360, fow.n_viewRadius);
		Vector3 viewAngleA = fow.DirectionFromAngle (-fow.n_viewAngle / 2, false);
		Vector3 viewAngleB = fow.DirectionFromAngle (fow.n_viewAngle / 2, false);

		Handles.DrawLine (fow.transform.position, fow.transform.position + viewAngleA * fow.n_viewRadius);
		Handles.DrawLine (fow.transform.position, fow.transform.position + viewAngleB * fow.n_viewRadius);

		Handles.color = Color.red;
		foreach (Transform visibleTarget in fow.n_visibleTargetsPursuit) {
			Handles.DrawLine (fow.transform.position, visibleTarget.position);
		}
	}

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

public class AnchorManger : MonoBehaviour {

	public GameObject anchorPrefab;
	public GameObject unanchorprefab;

	Anchor anchor;
	Vector3 lastAnchorPosition;
	Quaternion lastanchorrotation;
	public Transform point;
	public Camera camera;
	public GameObject currentobject;
	// Update is called once per frame
	void Update () {
		//Debug.Log ("positiom="+	transform.position);
	//	Debug.Log("angle = "+transform.rotation.eulerAngles);

		if( Input.GetMouseButtonDown(0)) {
			
			Pose p = new Pose();
			RaycastHit hit;
			Ray ray = camera.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out hit)) {
				//Transform objectHit = hit.transform;
				point.transform.position = hit.point;
				Debug.Log (point.transform.position);
				// Do something with the object that was hit by the raycast.
			}
			//Transform point = transform.Find ("");
			Vector3 ppos = new Vector3(point.position.x,point.position.y,point.position.z);
			p.position = ppos;
			anchor = Session.CreateWorldAnchor (p);
			//Debug.Log (anchor.transform.position);
			if (anchor != null) {
				currentobject = Instantiate (anchorPrefab, ppos, anchor.transform.rotation, anchor.transform);
			} else {
				currentobject = Instantiate (anchorPrefab, ppos, Quaternion.identity, null);
			}
		//	Instantiate (unanchorprefab, anchor.transform.position, anchor.transform.rotation, anchor.transform);
			//lastAnchorPosition = anchor.transform.position;
			//lastanchorrotation = anchor.transform.rotation;
		}

		if( Input.GetMouseButton(0)) {


		
			RaycastHit hit;
			Ray ray = camera.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out hit)) {
				//Transform objectHit = hit.transform;
				point.transform.position = hit.point;
				Debug.Log (point.transform.localPosition);
				// Do something with the object that was hit by the raycast.
			}
			//Transform point = transform.Find ("");
			Vector3 ppos = new Vector3(point.position.x,point.position.y,point.position.z);

			if (currentobject != null) {
				currentobject.transform.position = ppos;
			}

			//lastanchorrotation = anchor.transform.rotation;
		}
		if (Input.GetMouseButtonUp (0)) {
			currentobject = null;
		}
	


	}
}

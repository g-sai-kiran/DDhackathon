using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using UnityEngine.Events;

public class AnchorManger : MonoBehaviour {

	public GameObject anchorPrefab;
	public GameObject unanchorprefab;

	Anchor anchor;
	Vector3 lastAnchorPosition;
	Quaternion lastanchorrotation;
	public Transform point;
	public Camera camera;
	public GameObject currentobject;
	public UnityAction destroyline;

	public AudioSource audio;

	float currenttime = 0;
	public static AnchorManger instance;
	public List<GameObject> drawedObjects = new List<GameObject> ();

	float accelerometerUpdateInterval = 1.0f / 60.0f;
	// The greater the value of LowPassKernelWidthInSeconds, the slower the
	// filtered value will converge towards current input sample (and vice versa).
	float lowPassKernelWidthInSeconds = 1.0f;
	// This next parameter is initialized to 2.0 per Apple's recommendation,
	// or at least according to Brady! ;)
	float shakeDetectionThreshold = 1.2f;

	float lowPassFilterFactor;
	Vector3 lowPassValue;


	void Awake()
	{
		instance = this;
		Input.multiTouchEnabled = false;

		lowPassFilterFactor = accelerometerUpdateInterval / lowPassKernelWidthInSeconds;
		shakeDetectionThreshold *= shakeDetectionThreshold;
		lowPassValue = Input.acceleration;
		audio.volume = 0;
	}

	// Update is called once per frame
	void Update () {

		if( Input.GetMouseButtonDown(0)) {
			
			Pose p = new Pose();
			RaycastHit hit;
			Touch touch;
			touch = Input.GetTouch (0);
			Ray ray = camera.ScreenPointToRay(touch.position);

			if (Physics.Raycast(ray, out hit)) {
				//Transform objectHit = hit.transform;
				point.transform.position = hit.point;
				Debug.Log (point.transform.position);
			}
			Vector3 ppos = new Vector3(point.position.x,point.position.y,point.position.z);
			p.position = ppos;
			anchor = Session.CreateWorldAnchor (p);
			//Debug.Log (anchor.transform.position);
			if (anchor != null) {
				currentobject = Instantiate (anchorPrefab, ppos, anchor.transform.rotation, anchor.transform);
				drawedObjects.Add (currentobject);
			} else {
				currentobject = Instantiate (anchorPrefab, ppos, Quaternion.identity, null);
				drawedObjects.Add (currentobject);
			}
		}

		if( Input.GetMouseButton(0)) {


		
			RaycastHit hit;
			Touch touch;
			touch = Input.GetTouch (0);
			Ray ray = camera.ScreenPointToRay(touch.position);


			if (Physics.Raycast(ray, out hit)) {
				point.transform.position = hit.point;
				Debug.Log (point.transform.localPosition);
			}
			Vector3 ppos = new Vector3(point.position.x,point.position.y,point.position.z);
		
			if (currentobject != null) {
				currentobject.transform.position = ppos;
			}
			audio.volume += Time.deltaTime;
			return;
		}
		if (Input.GetMouseButtonUp (0) && currentobject != null) {
			ParticleSystem.EmissionModule pEmission = currentobject.GetComponent<ParticleSystem> ().emission;
				pEmission.enabled = false;
				currentobject = null;
		}
		currenttime -= Time.deltaTime;

		if( Input.GetMouseButton(1)) {
			if (destroyline != null) {
				destroyline.Invoke ();
			}
		}
		audio.volume -= Time.deltaTime;
		Vector3 acceleration = Input.acceleration;
		lowPassValue = Vector3.Lerp(lowPassValue, acceleration, lowPassFilterFactor);
		Vector3 deltaAcceleration = acceleration - lowPassValue;

		if (deltaAcceleration.sqrMagnitude >= shakeDetectionThreshold)
		{
			Invoke ("Destroline", 2);
		}
	}

	void Destroline()
	{
		if (destroyline != null) {
			destroyline.Invoke ();
		}
		for (int i = 0; i < drawedObjects.Count; i++) {
			if(drawedObjects[i] != null)
			Destroy (drawedObjects [i]);
		}
		drawedObjects.Clear ();
	}


}

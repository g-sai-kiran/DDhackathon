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
	//public ConnectLine latestLine;
//	public GameObject line;
	public UnityAction destroyline;

	public AudioSource audio;

	float currenttime = 0;
	public static AnchorManger instance;

	float accelerometerUpdateInterval = 1.0f / 60.0f;
	// The greater the value of LowPassKernelWidthInSeconds, the slower the
	// filtered value will converge towards current input sample (and vice versa).
	float lowPassKernelWidthInSeconds = 1.0f;
	// This next parameter is initialized to 2.0 per Apple's recommendation,
	// or at least according to Brady! ;)
	float shakeDetectionThreshold = 0.6f;

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
				//GameObject obj = Instantiate (line.gameObject, ppos, anchor.transform.rotation, anchor.transform);
				//latestLine = obj.GetComponent<ConnectLine> ();
				//destroyline += latestLine.EnbleGravity;
			} else {
				currentobject = Instantiate (anchorPrefab, ppos, Quaternion.identity, null);
				//GameObject obj  = Instantiate (line.gameObject, ppos, Quaternion.identity, null);
			//	latestLine = obj.GetComponent<ConnectLine> ();
				//destroyline += latestLine.EnbleGravity;	
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
			/*if (latestLine != null && currenttime <= 0) {
				GameObject obj = Instantiate (line.gameObject, ppos, Quaternion.identity, null);
				//destroyline += latestLine.EnbleGravity;
				latestLine.gameObject2 = obj.transform;
				latestLine = obj.GetComponent<ConnectLine> ();
				currenttime = -0.2f;
			}*/
			if (currentobject != null) {
				currentobject.transform.position = ppos;
			}
			audio.volume += Time.deltaTime;
			return;
			//lastanchorrotation = anchor.transform.rotation;
		}
		if (Input.GetMouseButtonUp (0) && currentobject != null) {
			ParticleSystem.EmissionModule pEmission = currentobject.GetComponent<ParticleSystem> ().emission;
				pEmission.enabled = false;
		//	pMain.loop = false;
			//Destroy (currentobject);
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
			// Perform your "shaking actions" here. If necessary, add suitable
			// guards in the if check above to avoid redundant handling during
			// the same shake (e.g. a minimum refractory period).
			Debug.Log("Shake event detected at time "+Time.time);
		//	Invoke ("Destroline", 2);
		}
	}

	void Destroline()
	{
		if (destroyline != null) {
			destroyline.Invoke ();
		}
	}


}

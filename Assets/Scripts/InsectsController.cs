using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InsectsController : MonoBehaviour {

	// Use this for initialization
	public LineRenderer line;
	public static InsectsController instance;

	LineRenderer currentline;
	public float speed = 1.0F;
	private float startTime;
	private float journeyLength;

	public List<Vector3> cooridinates = new List<Vector3> ();
	public Slider slide;
	void Awake()
	{
		instance = this;
		//line.positionCount = 1000;
	}
		
	void Start () {
		CreateLine (Vector3.zero);
	}

	public Quaternion GetGyro()
	{
		return Input.gyro.attitude;
	}

	public void CreateLine(Vector3 pos)
	{
		GameObject obj = Instantiate (line.gameObject, pos, Quaternion.identity);
		currentline = obj.GetComponent<LineRenderer> ();
		startTime = Time.time;
		cooridinates.Clear ();
		//currentline.transform.position = pos;
	}

	bool updating = false;
	public IEnumerator UpdatePos(Vector3 pos)
	{
		if (currentline != null) {
			if (updating) {
				yield break;
			}
			updating = true;
			cooridinates.Add (pos);
			currentline.positionCount = cooridinates.Count;
			currentline.SetPositions (cooridinates.ToArray ());
			yield return new WaitForSeconds (0.02f);
			updating = false;
		
			//float distCovered = (Time.time - startTime) * speed;
			//float fracJourney = distCovered / journeyLength;
			//currentline.transform.position = Vector3.Lerp(currentline.transform.position, pos, fracJourney);
		}
	}


	void Update()
	{
		
	

		if (Input.GetMouseButton (0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			StartCoroutine (InsectsController.instance.UpdatePos ( ray.direction));
		}
	}

	public void RemoveCurrentLine()
	{
		currentline = null;
	}

	public void UpdateWidth()
	{
		if (currentline != null) {
			currentline.GetComponent<TrailRenderer> ().startWidth = slide.value;
			currentline.GetComponent<TrailRenderer> ().endWidth = slide.value;
		}
	}

	void OnGUI()
	{
		Vector3 p = new Vector3();
		Camera  c = Camera.main;
		Event   e = Event.current;
		Vector2 mousePos = new Vector2();

		// Get the mouse position from Event.
		// Note that the y position from Event is inverted.
		mousePos.x = e.mousePosition.x;
		mousePos.y = c.pixelHeight - e.mousePosition.y;

		p = c.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, c.nearClipPlane));

		GUILayout.BeginArea(new Rect(20, 20, 250, 120));
		GUILayout.Label("Screen pixels: " + c.pixelWidth + ":" + c.pixelHeight);
		GUILayout.Label("Mouse position: " + mousePos);
		GUILayout.Label("World position: " + p.ToString("F3"));
		GUILayout.EndArea();
	//	StartCoroutine(UpdatePos (p));
	}
		


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InsectsController : MonoBehaviour {

	// Use this for initialization
	public GameObject line;
	public static InsectsController instance;

	GameObject currentline;
	public float speed = 1.0F;
	private float startTime;
	private float journeyLength;

	public Slider slide;
	void Awake()
	{
		instance = this;
	}
		
	void Start () {
		
	}
	
	public void CreateLine(Vector3 pos)
	{
		currentline = Instantiate (line, pos, Quaternion.identity);
		startTime = Time.time;
		//currentline.transform.position = pos;
	}

	public void UpdatePos(Vector3 pos)
	{
		if (currentline != null) {
			float distCovered = (Time.time - startTime) * speed;
			float fracJourney = distCovered / journeyLength;
			currentline.transform.position = Vector3.Lerp(currentline.transform.position, pos, fracJourney);
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
		


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsectsController : MonoBehaviour {

	// Use this for initialization
	public GameObject line;
	public static InsectsController instance;

	GameObject currentline;

	void Awake()
	{
		instance = this;
	}
		
	void Start () {
		
	}
	
	public void CreateLine(Vector3 pos)
	{
		currentline = Instantiate (line, null, false);
		currentline.transform.position = pos;
	}

	public void UpdatePos(Vector3 pos)
	{
		if (currentline != null) {
			currentline.transform.position = pos;
		}
	}

	public void RemoveCurrentLine()
	{
		currentline = null;
	}
		
		


}

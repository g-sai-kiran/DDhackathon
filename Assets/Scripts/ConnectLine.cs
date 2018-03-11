using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectLine : MonoBehaviour {

	public Transform gameObject1;          // Reference to the first GameObject
	public Transform gameObject2;          // Reference to the second GameObject

	public LineRenderer line; 
	public Rigidbody rg;

	// Use this for initialization
	void Start () {
		gameObject1 = this.transform;
		gameObject2 = this.transform;
		AnchorManger.instance.destroyline += EnableGravity;
		//line.SetPosition(0, gameObject1.transform.position);
	}

	// Update is called once per frame
	void Update () {
		// Check if the GameObjects are not null
		if (gameObject1 != null && gameObject2 != null)
		{
			// Update position of the two vertex of the Line Renderer
			line.SetPosition(0, gameObject1.transform.position);
			line.SetPosition(1, gameObject2.transform.position);
		}
	}

	 void EnableGravity()
	{
		rg.isKinematic = false;
		AnchorManger.instance.destroyline -= EnableGravity;
		Destroy (gameObject, 2);
	}
}

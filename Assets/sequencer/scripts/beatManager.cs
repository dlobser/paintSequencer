using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class beatManager : MonoBehaviour {


	/* a collection of bulbs that get triggered at regular intervals
	bulbs will have lists of strokes
	beatManager will instantiate strokes and place them in theappropriate bulb
	bulb will get triggered by beat

*/

//	public GameObject testBrush;


	bool triggered = false;
	bool makeNewStroke = false;
	public List<GameObject> bulbs;
	public int amount;
	public float radius = 5;
	public float height = 1;
//	public Material trailMat;

	public GameObject activeBulb;
//	public TrailRenderer trail;

	beatGlobals bGlobals;

	int which = 0;
	public float bpm = 1;
	private float bpmCounter = 0;

	int whichClip = 0;

	bool deleteMode = false;

	// Use this for initialization
	void Start () {
		Application.targetFrameRate = 60;
		bGlobals = beatGlobals.Instance;
//		bGlobals.clips = clips;
		bulbs = new List<GameObject> ();
		activeBulb = null;
		for (int i = 0; i < amount; i++) {
			GameObject b = new GameObject ();
			Bulb c = b.AddComponent<Bulb> ();
			c.Create ();
//			c.trailMat = trailMat;
			c.clip = bGlobals.clips [0];
			c.setCenter(new Vector3(
				Mathf.Sin( ((float)i/(float)amount)*Mathf.PI*2)*radius,
				height,
				Mathf.Cos( ((float)i/(float)amount)*Mathf.PI*2)*radius));
			bulbs.Add (b);
		}
	}

	public GameObject findClosest(Vector3 vec){

		int q = 0;
		float min = 1e6f;
		for (int i = 0; i < bulbs.Count; i++) {
			float dist = Vector3.Distance (bulbs [i].GetComponent<Bulb>().center, vec);
			if (dist < min) {
				min = dist;
				q = i;
			}
		}
		return bulbs [q];
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyUp (KeyCode.W)) {
			Debug.Log (bGlobals.which);
			bGlobals.which++;
			if (bGlobals.which > bGlobals.strokes.Length-1)
				bGlobals.which = 0;
		}
		if (Input.GetKeyUp (KeyCode.D)) {
			deleteMode = !deleteMode;
		}
		if(Input.GetMouseButton(0)){
			Vector3 hit = hitPoint();
			if (activeBulb==null) {
				activeBulb = findClosest (hit);
				activeBulb.active = true;
				if (deleteMode)
					activeBulb.GetComponent<Bulb> ().clearStrokes ();
			} else if (activeBulb.active && !deleteMode) {
				activeBulb.GetComponent<Bulb>().drawStroke (hit);
			}
				
		}
		if (Input.GetMouseButtonUp (0)) {
			if(activeBulb)
				activeBulb.GetComponent<Bulb> ().finishStroke ();
			activeBulb = null;
		}

		bpmCounter += Time.deltaTime;
		if (bpmCounter > bpm) {
			bpmCounter = 0;
			which++;
			if (which > bulbs.Count - 1)
				which = 0;
			bulbs [which].GetComponent<Bulb> ().trigger ();
		}
	
	}

	public Vector3 hitPoint(){
		Vector3 vec = Vector3.zero;
		RaycastHit vHit = new RaycastHit ();
		Ray vRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		if (Physics.Raycast (vRay, out vHit, 1000)) {
			vec = vHit.point;
		}
		return vec;
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Bulb : MonoBehaviour {

	public List<Stroke> strokes;
	public float radius = 55;
	public Vector3 center;
	public GameObject display;
	public GameObject activeStroke;
	public AudioClip clip;
	public bool active = false;
	public float minScale = .05f;
	public float maxScale = .05f;
	public float pitch = 1;
	beatGlobals bGlobals;


	public GameObject Create(){
		bGlobals = beatGlobals.Instance;
		minScale = beatGlobals.Instance.minBulbScale;
		maxScale = beatGlobals.Instance.maxBulbScale;
		activeStroke = null;
		strokes = new List<Stroke> ();
		center = Vector3.zero;
		display = Instantiate (beatGlobals.Instance.bulbDisplay);// GameObject.CreatePrimitive (PrimitiveType.Sphere);
//		display.GetComponent<SphereCollider> ().enabled = false;
//		display.GetComponent<Renderer> ().sharedMaterial.color = new Color(1, 1, 1, .2f);
		return display;
	}

	public void trigger(){
		int down = 0;
		if (activeStroke != null)
			down = 1;
		for (int i = 0; i < strokes.Count-down; i++) {
			strokes [i].playButton ();
		}
		display.transform.localScale = Vector3.one*maxScale;
	}

	public void setCenter(Vector3 vec){
		center = vec;
		display.transform.localPosition = center;
	}

	public void clearStrokes(){
		foreach (Stroke s in strokes) {
			if (s.GetComponent<Stroke> ().subTrails.Count > 0) {
				foreach(GameObject st in s.GetComponent<Stroke> ().subTrails){
					Destroy (st);
				}
			}
			Destroy (s.gameObject);
		}
		strokes.Clear ();
	}

	public void drawStroke(Vector3 vec){
		if (activeStroke == null) {
			activeStroke = Instantiate (bGlobals.strokes[bGlobals.which]);
			Stroke s = activeStroke.GetComponent<Stroke>();
			s.setPitch (pitch);
			s.playStart();
			s.trailLength += 1-pitch;
			strokes.Add (s);
			pitch *= .9f;
		} else {
			activeStroke.GetComponent<Stroke>(). Draw (vec);
		}
	}

	public void finishStroke(){
		if (activeStroke != null) {
			activeStroke.GetComponent<Stroke> ().isPlaying = false;
			activeStroke = null;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (display.transform.localScale.x > minScale) {
			display.transform.localScale = Vector3.Scale (display.transform.localScale, new Vector3 (.95f, .95f, .95f));
		}
		int down = 0;
		if (activeStroke != null)
			down = 1;
		for (int i = 0; i < strokes.Count-down; i++) {
			strokes [i].playBack ();

		}
	}
}

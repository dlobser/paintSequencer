using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class beatGlobals : Singleton<beatGlobals> {

	//converts korg nano dials to an easily readable array of values

	public AudioClip[] clips;
	public int which = 0;
	public GameObject defaultStroke;
	public GameObject[] strokes;
	public GameObject[] subStrokes;
	public float strokeAge = 5;
	public float trailWidth = .1f;
	public GameObject bulbDisplay;
	public float minBulbScale;
	public float maxBulbScale;

}

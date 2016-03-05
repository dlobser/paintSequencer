using UnityEngine;
using System.Collections;

public class audioStroke : MonoBehaviour {

	AudioSource aud;
	Vector3 prev;
	float vol;
	// Use this for initialization
	void Start () {
		aud = this.GetComponent<AudioSource> ();
		prev = Vector3.zero;
		vol = 0;
	}
	
	// Update is called once per frame
	void Update () {
		vol += Vector3.Distance (prev, Input.mousePosition) * .001f;
		aud.volume = vol;
		vol*=.9f;
		prev = Input.mousePosition;
	}
}

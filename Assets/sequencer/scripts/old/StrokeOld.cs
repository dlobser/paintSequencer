using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StrokeOld: MonoBehaviour {

	public List<GameObject> subTrails;

	protected beatGlobals bGlobals;

	public GameObject root;
	public AudioSource aud;
	public LineRenderer lRend;
	public float trailLength = 1;
	public float trailWidth = .1f;
	public List<Vector3> trail;

	public Vector3[] Trail;
	public int trailAlloc = 5000;
	public int trailHead = 0;

	protected float timer = 0;
	protected int playbackHead = 0;
	public bool isPlaying = false;

	public bool drawing;
	public float age = 0;

	public bool readyToDie = false;


	public virtual void Awake(){
		bGlobals = beatGlobals.Instance;
		lRend = transform.GetComponent<LineRenderer> ();
		Trail = new Vector3[trailAlloc];
	}

	public void Create(AudioClip c){

		trail = new List<Vector3> ();
		root = transform.gameObject;//new GameObject ();
		aud = root.AddComponent<AudioSource> ();
//		aud.clip = bGlobals.clips[bGlobals.which];

	}

	public virtual void Draw(Vector3 vec){
		if (!isPlaying) {
			root.transform.localPosition = vec;
			timer += Time.deltaTime;
			if (!vec.Equals (Vector3.zero))
				strokeUtils.addToTrail (Trail, trailHead, vec);
//			trail.Add (vec);
			trailToLine (0,trailHead);
			if (timer > trailLength)
				strokeUtils.shiftArray (Trail, trailHead);//	trail.RemoveAt (0);
			else {
				trailHead++;
				playbackHead++;
			}
		}
	}

	public void trailToLine(int start, int end){
		lRend.SetVertexCount (end-start);
		lRend.SetPositions (strokeUtils.getArrayPortion(Trail,start,end));
	}

	public virtual void playButton(){
		if (!isPlaying && age < beatGlobals.Instance.strokeAge) {
			isPlaying = true;
			trailWidth = bGlobals.trailWidth;
			lRend.SetWidth (0, trailWidth);
			playStartSound ();
		}
	}

	public virtual void playBack(){
		age += Time.deltaTime;
		print (readyToDie);
		if (isPlaying && trailHead > 0) {
			traceLine ();
			if (playbackHead > trailHead - 1) {
				isPlaying = false;
				playbackHead = 0;
			}
		} else if (!isPlaying && playbackHead < trailHead - 1) {
			traceLine ();
		} else if (!isPlaying) {
			destruct ();
		} else if (readyToDie) {
			readyToDie = !readyToDie;
			timer = 0;
			age = 0;
			playbackHead = 0;
			root.transform.localPosition = Trail [playbackHead];
			trailToLine (0, 0);
			float sc = 0;
			root.transform.GetChild (0).transform.localScale = new Vector3 (sc, sc, sc);
		}
	}

	public virtual void traceLine(){
		root.transform.localPosition = Trail [playbackHead];
		trailToLine (0, playbackHead);
		//			Debug.Log (trail.Count + " , " + playbackHead);
		playbackHead++;

		float sc = (-Mathf.Cos (((float)playbackHead / (float)trailHead) * Mathf.PI * 2) + 1) * .08f;
		root.transform.GetChild (0).transform.localScale = new Vector3 (sc, sc, sc);
	}

	public virtual void destruct(){

		trailWidth *= .97f;
		if(lRend)
			lRend.SetWidth (0, trailWidth);

		float sc = root.transform.GetChild (0).transform.localScale.x;
		root.transform.GetChild (0).transform.localScale = (new Vector3 (sc*.9f, sc*.9f, sc*.9f));

		if (trailWidth < .05f) {
			readyToDie = true;
		}
	}

	public void setPitch(float p){
		aud.pitch = p;
	}

	public void playEndSound(){
//		Debug.Log (trail.Count);
	}

	public void playStartSound(){
		aud.Play ();
	}

}

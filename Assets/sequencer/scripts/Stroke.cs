using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Stroke : MonoBehaviour {

	public List<GameObject> subTrails;
	public List<animate> animators;

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
		animators = new List<animate> ();
		registerAnimators (this.gameObject);
		bGlobals = beatGlobals.Instance;
		lRend = transform.GetComponent<LineRenderer> ();
		Trail = new Vector3[trailAlloc];
	}

	public virtual void Draw(Vector3 vec){
		if (!isPlaying) {
			root.transform.localPosition = vec;
			timer += Time.deltaTime;
			if (!vec.Equals (Vector3.zero))
				strokeUtils.addToTrail (Trail, trailHead, vec);
			trailToLine (0,trailHead);
			if (timer > trailLength)
				strokeUtils.shiftArray (Trail, trailHead);//	trail.RemoveAt (0);
			else {
				trailHead++;
				playbackHead++;
			}
		}
	}

	public virtual void playBack(){
		age += Time.deltaTime;
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
			//float sc = 0;
			//root.transform.GetChild (0).transform.localScale = new Vector3 (sc, sc, sc);
		}
	}
		
	public void registerAnimators(GameObject g){

		animate[] playerScripts = g.GetComponents<animate>();
		if (playerScripts.Length > 0) {
			foreach (animate anims in playerScripts) {
				animators.Add (anims);
			}
		}
		for (int i = 0; i < g.transform.childCount; i++) {
			registerAnimators (g.transform.GetChild (i).gameObject);
		}
	}

	public void animateAnimators(float t){
		for (int i = 0; i < animators.Count; i++) {
			animators [i].Play (t);
		}
	}

	public void Create(AudioClip c){
		trail = new List<Vector3> ();
		root = transform.gameObject;
		aud = root.AddComponent<AudioSource> ();
	}

	public void trailToLine(int start, int end){
		Trail = strokeUtils.averageVecArray (Trail, trailHead, .97f);
		lRend.SetVertexCount (end-start);
		lRend.SetPositions (strokeUtils.getArrayPortion(Trail,start,end));
		lRend.SetWidth (0, trailWidth);
	}

	public virtual void playButton(){
		if (!isPlaying && age < beatGlobals.Instance.strokeAge) {
			isPlaying = true;
			trailWidth = bGlobals.trailWidth;
			lRend.SetWidth (0, trailWidth);
			playStart();
		}
	}
		
	public virtual void traceLine(){
		root.transform.localPosition = Trail [playbackHead];
		trailToLine (0, playbackHead);
		playbackHead++;
		animateAnimators ((float)playbackHead / (float)trailHead);
	}


	//runs until destroyed
	public virtual void destruct(){

		playEnd ();

		

		if(lRend)
			lRend.SetWidth (0, trailWidth);

        if(trailWidth > .01f) {
            trailWidth *= .97f;
        }
		else {
			readyToDie = true;
		}
	}

	public void setPitch(float p){
		aud.pitch = p;
	}

	public void playEnd(){
		for (int i = 0; i < animators.Count; i++) {
			animators [i].TriggerOff ();
		}
	}

	public void playStart(){
		aud.Play ();
	}

}

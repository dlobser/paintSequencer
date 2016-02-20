using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class longStrokeOld : Stroke {


	public override void Awake(){
		bGlobals = beatGlobals.Instance;
		lRend = transform.GetComponent<LineRenderer> ();
		Trail = new Vector3[trailAlloc];
		aud.loop = true;
	}

	public override void Draw(Vector3 vec){
		if (!isPlaying) {

			root.transform.localPosition = vec;
			timer += Time.deltaTime;
			if(!vec.Equals(Vector3.zero))
				strokeUtils.addToTrail (Trail, trailHead, vec);
			
//				trail.Add (vec);
			trailToLine (0,trailHead);
//			Debug.Log (timer+","+trailLength + "," + subTrails.Count);
			makeSubTrail ();
			foreach (GameObject t in subTrails) {
				Stroke s = t.GetComponent<Stroke> ();
				strokeUtils.addToTrail (s.Trail, ++s.trailHead, Trail [trailHead]);
//				t.GetComponent<Stroke> ().trail.Add (trail[trail.Count-1]);
			}
			trailHead++;
//			Debug.Log (trailHead);
//			if (timer > trailLength) {
//				timer = 0;
////				Debug.Log (timer+" , "+trailLength + " , " + subTrails.Count);
//				subTrails.Add (Instantiate (bGlobals.strokes[0]));
//				subTrails[subTrails.Count-1].GetComponent<Stroke>().trail = trail;
//				subTrails [subTrails.Count - 1].GetComponent<Stroke> ().playButton ();
//
//			}
//			foreach (GameObject t in subTrails) {
////				subTrails [subTrails.Count - 1].GetComponent<Stroke> ().
//				subTrails [subTrails.Count - 1].GetComponent<Stroke> ().trail.Add (vec);
//				subTrails [subTrails.Count - 1].GetComponent<Stroke> ().playBack ();
//			}
		}
//		foreach (GameObject s in subTrails) {
//			s.GetComponent<Stroke> ().Draw ();
//		}
	}

	public void makeSubTrail(){
		
		if (timer > trailLength) {
			timer = 0;
			subTrails.Add (Instantiate (bGlobals.strokes[0]));
			subTrails[subTrails.Count-1].GetComponent<Stroke>().Trail = (Vector3[])Trail.Clone();
			subTrails [subTrails.Count - 1].GetComponent<Stroke> ().trailHead = trailHead;
			subTrails [subTrails.Count - 1].GetComponent<AudioSource>().clip = bGlobals.clips[bGlobals.which];
			subTrails [subTrails.Count - 1].GetComponent<Stroke> ().playButton ();

		}
		for(int i = 0 ; i < subTrails.Count ; i++){
			subTrails[i].GetComponent<Stroke> ().playBack ();
		}
	}

	public bool subTrailsReadyToDie(){
		bool subTrail = false;
		for(int i = 0 ; i < subTrails.Count ; i++){
			
			if (subTrails [i].GetComponent<Stroke> ().readyToDie)
				subTrail = true;
		}
		return subTrail;
	}

//
//	root.transform.localPosition = Trail [playbackHead];
//	trailToLine (0, playbackHead);
//	//			Debug.Log (trail.Count + " , " + playbackHead);
//	playbackHead++;
//
//	float sc = (-Mathf.Cos (((float)playbackHead / (float)trailHead) * Mathf.PI * 2) + 1) * .08f;
//	root.transform.GetChild (0).transform.localScale = new Vector3 (sc, sc, sc);

	public override void playBack(){
		age += Time.deltaTime;
		if (isPlaying && trailHead > 0) {
			traceLine();
			timer += Time.deltaTime;
			makeSubTrail ();

			if (playbackHead > trailHead - 1) {
				isPlaying = false;
				playbackHead = 0;
				//				root.transform.localPosition = trail [0];
				playEnd();
			}
		} else if (!isPlaying) {
			if(subTrailsReadyToDie())
				destruct ();

			traceLine();
			makeSubTrail ();
//			foreach (GameObject s in subTrails) {
//				s.GetComponent<Stroke> ().destruct ();
//			}
			for(int i = 0 ; i < subTrails.Count ; i++){
				//		foreach (GameObject t in subTrails) {
				//			Debug.Log (subTrails.Count);
				//				subTrails [subTrails.Count - 1].GetComponent<Stroke> ().

				//			t.GetComponent<Stroke> ().trail.Add (trail[trail.Count-1]);
				subTrails[i].GetComponent<Stroke> ().isPlaying=false;
			}
		}
		if (readyToDie) {
			Debug.Log ("ready to die");
			strokeUtils.clearStrokes (subTrails);
			readyToDie = !readyToDie;
			timer = 0;
			age = 0;
			playbackHead = 0;
			root.transform.localPosition = Trail [playbackHead];
			trailToLine (0, 0);
			float sc =0;
			root.transform.GetChild (0).transform.localScale = new Vector3 (sc, sc, sc);
		
		}
	}

}

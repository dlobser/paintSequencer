using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class longStroke2 : Stroke {

	public int subTrailType = 0;

	public override void Awake(){
        base.Awake();
		animators = new List<animate> ();
		registerAnimators (this.gameObject);
		bGlobals = beatGlobals.Instance;
		lRend = transform.GetComponent<LineRenderer> ();
		Trail = new Vector3[trailAlloc];
		_audio.loop = true;
		timer = 1e6f;
	}

	public override void Draw(Vector3 vec){
		
		if (!this.isPlaying) {

			timer += Time.deltaTime;

			if(!vec.Equals(Vector3.zero))
				strokeUtils.addToTrail (Trail, trailHead, vec);

			trailToLine (0,trailHead);
			makeSubTrail ();

			foreach (GameObject t in subTrails) {
				Stroke s = t.GetComponent<Stroke>();
				strokeUtils.addToTrail(s.Trail, ++s.trailHead, Trail [trailHead]);
			}

			trailHead++;

		}

	}

	public void makeSubTrail(){
		if (timer > trailLength) {
			timer = 0;
			subTrails.Add (Instantiate (bGlobals.subStrokes[subTrailType]));
            subTrails[subTrails.Count - 1].GetComponent<Stroke>().Trail =  strokeUtils.noiseVecArray((Vector3[])  Trail.Clone(), trailHead, .9f);
			subTrails [subTrails.Count - 1].GetComponent<Stroke> ().trailHead = trailHead;
			subTrails [subTrails.Count - 1].GetComponent<Stroke> ().playButton ();
		}
		for(int i = 0 ; i < subTrails.Count ; i++){
			subTrails[i].GetComponent<Stroke> ().playBack ();
		}
	}

    //void Update() {
    //    if (this.age > GlobalBeatManager.instance.maxStrokeAge) {
    //        Destroy(this.gameObject);
    //    }
    //}



    //void Play() {
    //    if (isPlaying)
    //        return;

        
    //}

	public override void playButton(){
//		base.playButton ();
		if (!isPlaying && age < beatGlobals.Instance.strokeAge && subTrails.Count==0) {
			isPlaying = true;
			trailWidth = bGlobals.trailWidth;
			lRend.SetWidth (0, trailWidth);
			this.playAudio();
			timer = 1e6f;
		}

	}

	public bool subTrailsReadyToDie(){

		bool subTrail = false;
		List<int> removeWhich = new List<int> ();

		for(int i = 0 ; i < subTrails.Count ; i++){
			if (subTrails [i].GetComponent<Stroke> ().readyToDie) {
				removeWhich.Add (i);
			}
		}

		foreach (int j in removeWhich) {
			Destroy (subTrails [j]);
			subTrails.RemoveAt (j);
		}

		if (subTrails.Count<1)
			subTrail = true;

		return subTrail;
	}

	public override void playBack(){
		age += Time.deltaTime;

		if (isPlaying && trailHead > 0) {
			
			root.transform.localPosition = Trail [currentPlaybackIndex];
			currentPlaybackIndex++;
			timer += Time.deltaTime;
			makeSubTrail ();

			if (currentPlaybackIndex > trailHead - 1) {
				isPlaying = false;
				currentPlaybackIndex = 0;
				playEnd ();
			}
		} 
		else if (!isPlaying) {
				
			if(subTrailsReadyToDie())
				destruct ();
			
			makeSubTrail ();

			for(int i = 0 ; i < subTrails.Count ; i++){
				subTrails[i].GetComponent<Stroke> ().isPlaying=false;
			}
		}
		if (readyToDie) {
			strokeUtils.clearStrokes (subTrails);
			readyToDie = !readyToDie;
			timer = 1e6f;
			age = 0;
			currentPlaybackIndex = 0;
			root.transform.localPosition = Trail [currentPlaybackIndex];
			trailToLine (0, 0);
//			float sc =0;
//			root.transform.GetChild (0).transform.localScale = new Vector3 (sc, sc, sc);

		}
	}

}

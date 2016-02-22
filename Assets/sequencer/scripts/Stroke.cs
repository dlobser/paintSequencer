using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum StrokeState { START, PLAY, FINISH };
public class Stroke : MonoBehaviour {

	public List<GameObject> subTrails;
	public List<animate> animators;

	protected beatGlobals bGlobals;

	public GameObject root;
	public AudioSource _audio;
	public LineRenderer lRend;
	public float trailLength = 1;
	public float trailWidth = .1f;



	public List<Vector3> trail;

	public Vector3[] Trail;
	public int trailAlloc = 5000;
	public int trailHead = 0;

	protected float timer = 0;
	protected int currentPlaybackIndex = 0;


	public bool drawing;
	public float age = 0;

	public bool readyToDie = false;

    public bool isBeingDrawn = false;
    public bool isPlaying = false;

    public StrokeState state;
    public AudioClip audioClip;


    public virtual void Awake(){
		animators = new List<animate> ();
		registerAnimators (this.gameObject);
		bGlobals = beatGlobals.Instance;
		lRend = transform.GetComponent<LineRenderer> ();
		Trail = new Vector3[trailAlloc];
        trail = new List<Vector3>();
        root = transform.gameObject;
        _audio = root.AddComponent<AudioSource>();
    }


    void Update() {
        switch(this.state) {
            case StrokeState.START:
                HandlePlayStart();
                break;
            case StrokeState.PLAY:
                HandlePlay();
                break;
            case StrokeState.FINISH:
                HandlePlayFinish();
                break;
        }
    }

    //why was this protected?
    public void SwitchState(StrokeState newState) {
        this.state = newState;
        switch(this.state) {
            case StrokeState.START:
                this.OnPlayStart();
                break;
            case StrokeState.PLAY:
                this.OnPlay();
                break;
            case StrokeState.FINISH:
                this.OnPlayFinish();
                break;
        }
    }


    protected virtual void OnPlayStart() {
        //previously was in PlayButton() method
        if (!isPlaying && age < beatGlobals.Instance.strokeAge) {
            this.isPlaying = true;
            trailWidth = bGlobals.trailWidth;
            lRend.SetWidth(0, trailWidth);
            this.playAudio();
            SwitchState(StrokeState.PLAY);
        }
    }

    protected virtual void OnPlay() {

    }

    protected virtual void OnPlayFinish() {
        if(this.isPlaying) this.isPlaying = false;
    }

    protected virtual void HandlePlayStart() {

    }

    protected virtual void HandlePlay() {

    }

    protected virtual void HandlePlayFinish() {

    }


    public virtual void Reset() {
        timer = 0;
        age = 0;
        currentPlaybackIndex = 0;
        root.transform.localPosition = Trail[0];
        trailToLine(0, 0);
    }

    /*

        Below are called from Bulb.cs

    */



    public virtual void Draw(Vector3 vec) {
        if (!this.isPlaying) {
            this.isBeingDrawn = true;
            root.transform.localPosition = vec;
            timer += Time.deltaTime;
            if (!vec.Equals(Vector3.zero))
                strokeUtils.addToTrail(Trail, trailHead, vec);
            trailToLine(0, trailHead);
            if (timer > trailLength)
                strokeUtils.shiftArray(Trail, trailHead);//	trail.RemoveAt (0);
            else {
                trailHead++;
                currentPlaybackIndex++;
            }
        }
    }

    public virtual void playButton() {
        SwitchState(StrokeState.START);
    }

    public virtual void playBack(){
		this.age += Time.deltaTime;
		if (isPlaying && trailHead > 0) {
            //HANDLEPLAY
			traceLine ();
			if (currentPlaybackIndex > trailHead - 1) {
				isPlaying = false;
				currentPlaybackIndex = 0;
			}
		//} else if (!isPlaying && currentPlaybackIndex < trailHead - 1) {
//			traceLine ();
		} else if (!isPlaying) {
            //HANDLEPLAYFINISH
			destruct();
		} else if (readyToDie) {
			readyToDie = !readyToDie;
			timer = 0;
			age = 0;
			currentPlaybackIndex = 0;
			root.transform.localPosition = Trail [currentPlaybackIndex];
			trailToLine (0, 0);
		}
	}


    //FIND ALL "ANIMATE" FUNCTIONS IN SELF AND CHILDREN
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

    //START ANIMATE FUNCTIONS
	public void animateAnimators(float t){
		for (int i = 0; i < animators.Count; i++) {
			animators [i].Play (t);
		}
	}

	public void trailToLine(int start, int end){
		Trail = strokeUtils.averageVecArray (Trail, trailHead, .97f);
		lRend.SetVertexCount (end-start);
		lRend.SetPositions (strokeUtils.getArrayPortion(Trail,start,end));
		lRend.SetWidth (0, trailWidth);
	}


    public void Play() { 
        if (isPlaying)
            return;
        this.SwitchState(StrokeState.START);
    }

    public virtual void traceLine() {
        root.transform.localPosition = Trail[currentPlaybackIndex];
        trailToLine(0, currentPlaybackIndex);
        currentPlaybackIndex++;
        animateAnimators((float)currentPlaybackIndex / (float)trailHead);
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


	public void playEnd(){
		for (int i = 0; i < animators.Count; i++) {
			animators [i].TriggerOff ();
		}
	}

    public void setAudioClip(int clipIndex) {
        this.audioClip = this.bGlobals.clips[clipIndex];
    }

    public void setAudioPitch(float pitch) {
        this._audio.pitch = pitch;
    }

    public void playAudio() {
        this._audio.Play();
    }




}

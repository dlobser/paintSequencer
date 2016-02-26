﻿using UnityEngine;
using System.Collections.Generic;

namespace HolojamEngine {

    ////////////////////////////////////////
    //required components
    ////////////////////////////////////////
    [RequireComponent(typeof(LineRenderer))]
    [RequireComponent(typeof(AudioSource))]
    public abstract class Stroke : MonoBehaviour {

        ////////////////////////////////////////
        //state machine enumerator
        ////////////////////////////////////////
        public enum StrokeState {IDLE, START, PLAY, FINISH };

        ////////////////////////////////////////
        //public structs
        ////////////////////////////////////////
        public struct StrokePoint {
            public Vector3 vec;
            public float time;
            public Stroke subStroke;

            public StrokePoint(Vector3 v, float time, Stroke s = null) {
                this.vec = v;
                this.time = time;
                this.subStroke = s;
            }
        }


        ////////////////////////////////////////
        //public collections & vars
        ////////////////////////////////////////
        public bool IsPlaying {
            get { return this.isPlaying; }
        }

        public float StrokeWidth {
            get { return this.strokeWidth; }
            set { this.strokeWidth = value; }
        }

        ////////////////////////////////////////
        //protected/private collections & vars
        ////////////////////////////////////////
        protected List<StrokeAnimation> animations  = new List<StrokeAnimation>();
        protected List<StrokePoint> trail = new List<StrokePoint>();

        protected LineRenderer line;
        protected AudioSource audio;
        protected Transform root;
        protected int trailMaxVertexCount = 100;
        protected int currentPlaybackIndex = 0;
        protected float age;
        protected float strokeWidth = 0.1f;
        protected bool isFlaggedForDeath = false;
        protected bool hasBeenDrawn = false;
        protected bool isPlaying = false;
        private StrokeState state = StrokeState.IDLE;
        protected bool canRestartFromFinish = true;

        ////////////////////////////////////////
        //inherited functions
        ////////////////////////////////////////

        //occurs before all gameobject initialization. ( equivalent to constructor )
        protected void Awake() {
            //grab all local components
            this.line = this.GetComponent<LineRenderer>();
            this.audio = this.GetComponent<AudioSource>();
            this.root = this.transform;
            trailMaxVertexCount = GlobalValuesAndSettings.Instance.STROKE_MAX_VERTEX_COUNT;
            trail = new List<StrokePoint>(trailMaxVertexCount);
        }

        // Use this for initialization
        protected virtual void Start() {
        }

        // Update is called once per frame
        protected virtual void Update() {
            this.HandleStateMachine();
            this.HandleAge();
        }


        ////////////////////////////////////////
        //custom functions
        ////////////////////////////////////////

        public void Play() {
            if (!this.hasBeenDrawn) {
                return;
                //TO-DO: CHANGE
            }  else if (this.state.Equals(StrokeState.IDLE) || 
                       (this.canRestartFromFinish && this.state.Equals(StrokeState.FINISH))) {
                this.Reset();
                this.SwitchToState(StrokeState.START);
            } 

        }

        public void FlagForDeath() {
            this.isFlaggedForDeath = true;
        }

        public StrokeState GetState() {
            return this.state;
        }

        public void SetTrail(List<StrokePoint> newTrail)
        {
            this.hasBeenDrawn = true;
            this.trail = newTrail;
        }

        public void AddStrokePoint(StrokePoint newPoint) {
            this.trail.Add(newPoint);
        }

        protected void PlayAudio() {
            if (this.audio.isPlaying)
                this.audio.Stop();

            this.audio.Play();
        }

        public void SetAudioPitch(float pitch) {
            this.audio.pitch = pitch;
        }

        protected void HandleStateMachine() {
            switch(this.state) {
                case StrokeState.IDLE:
                    this.HandleIdle();
                    break;
                case StrokeState.START:
                    this.HandleStart();
                    break;
                case StrokeState.PLAY:
                    this.HandlePlay();
                    break;
                case StrokeState.FINISH:
                    this.HandleFinish();
                    break;
            }
        }

        protected void SwitchToState(StrokeState newState) {
            this.state = newState;
            switch(this.state) {
                case StrokeState.IDLE:
                    this.OnIdle();
                    break;
                case StrokeState.START:
                    this.OnStart();
                    break;
                case StrokeState.PLAY:
                    this.OnPlay();
                    break;
                case StrokeState.FINISH:
                    this.OnFinish();
                    break;
            }
        }

        protected void HandleAge() {
            this.age += Time.unscaledDeltaTime;

            if (this.age > GlobalValuesAndSettings.Instance.STROKE_MAX_AGE)
                this.FlagForDeath();
        }

        protected void PushTrailToLine(int start, int finish) {
            Vector3[] arr = StrokeUtils.ListToArray(trail,start, finish);
            line.SetVertexCount(arr.Length);
            line.SetPositions(arr);
            line.SetWidth(this.strokeWidth, this.strokeWidth);
        }

        protected virtual void Reset() {
            this.strokeWidth = GlobalValuesAndSettings.Instance.STROKE_START_WIDTH;
            this.currentPlaybackIndex = 0;
        }

        ////////////////////////////////////////
        //abstract functions
        ////////////////////////////////////////
        public abstract void Draw(Vector3 v);
        public abstract void FinishDraw(); //TODO: RENAME THIS
        //public abstract void Kill();

        protected abstract void OnIdle();
        protected abstract void OnStart();
        protected abstract void OnPlay();
        protected abstract void OnFinish();

        protected abstract void HandleIdle();
        protected abstract void HandleStart();
        protected abstract void HandlePlay();
        protected abstract void HandleFinish();


    }


    public class StrokeUtils {

        public static Vector3[] ListToArray(List<Vector3> list, int start, int finish) {
            Vector3[] arr = new Vector3[finish - start];
            for (int i = start; i < finish; i++) {
                arr[i] = list[i];
            }
            return arr;
        }

        public static Vector3[] ListToArray(List<Stroke.StrokePoint> list, int start, int finish) {
            Vector3[] arr = new Vector3[finish - start];
            for (int i = start; i < finish; i++) {
                arr[i] = list[i].vec;
            }
            return arr;
        }

        public static Vector3[] AverageArray(Vector3[] arr) {
            for (int i = 1; i < arr.Length - 1; i++) {
                arr[i] = (arr[i - 1] + arr[i + 1]) / 2f;
            }
            return arr;
        }

        public static void Average(List<Vector3> list) {
            for (int i = 1; i < list.Count-1; i++) {
                list[i] = (list[i - 1] + list[i] + list[i + 1]) / 3;
            } 
        }

        public static Vector3[] LerpAverageArray(Vector3[] arr, int count, float amount) {
            for (int i = 1; i < count-1; i++) {
                arr[i] = Vector3.Lerp( (arr[i-1] + arr[i+1]) / 2, arr[i], amount );
            }
            return arr;
        }

        public static List<Vector3> AddNoiseToList(List<Vector3> vecs, float amt)
        {
            
            for (int i = 1; i < vecs.Count - 1; i++)
            {
                Vector3 a = vecs[i - 1] * 10;
                Vector3 b = vecs[i + 1] * 10;
                Vector3 n = vecs[i] + new Vector3(
                    Mathf.Cos(Mathf.PI * Mathf.PerlinNoise(a.x, b.x + Time.time)),
                    Mathf.Cos(Mathf.PI * Mathf.PerlinNoise(a.y, b.y + Time.time)),
                    Mathf.Cos(Mathf.PI * Mathf.PerlinNoise(b.z, a.z + Time.time)));
                vecs[i] = Vector3.Lerp(n, vecs[i], amt);
            }
            return vecs;
        }

        public static List<Stroke.StrokePoint> AddNoiseToList(List<Stroke.StrokePoint> vecs, float amt) {

            for (int i = 1; i < vecs.Count - 1; i++) {
                Vector3 a = vecs[i - 1].vec * 10;
                Vector3 b = vecs[i + 1].vec * 10;
                Vector3 n = vecs[i].vec + new Vector3(
                    Mathf.Cos(Mathf.PI * Mathf.PerlinNoise(a.x, b.x + Time.time)),
                    Mathf.Cos(Mathf.PI * Mathf.PerlinNoise(a.y, b.y + Time.time)),
                    Mathf.Cos(Mathf.PI * Mathf.PerlinNoise(b.z, a.z + Time.time)));
                vecs[i] = new Stroke.StrokePoint(Vector3.Lerp(n, vecs[i].vec, amt),vecs[i].time);
            }
            Debug.Log(vecs.Count);
            return vecs;
        }
    }
}

using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace HolojamEngine {
    public class nStroke : Stroke {

        //fix timer

        public float drawThreshold = 0.05f;

		public List<Vector3> nTrail = new List<Vector3> ();
		public List<float> nLerp = new List<float> ();

        private float timer = 0f;
        private float timeOffset = 0f;
		private float endTimer = 0f;

		public float fadeSpeed = 1;

        private Vector3 previousDrawVector = Vector3.zero;

		public float noiseDispersion = .01f;

		public bool start = false;



        public override void Draw(Vector3 v) {
            this.root.position = v;

			if (!start) {

				foreach (StrokeAnimation a in animations) {
					a.OnStart ();
				}
				start = true;
			}
            if (Vector3.Distance(this.previousDrawVector, v) > this.drawThreshold) {

				AddStrokePoint (new StrokePoint (v, timer));
				lerpTrail ();

				if(this.currentPlaybackIndex<this.trail.Count-1)
					this.currentPlaybackIndex++;
					
                if (this.trail.Count == this.trailMaxVertexCount) {
                    this.trail.RemoveAt(0);
					this.nTrail.RemoveAt (0);
					this.nLerp.RemoveAt (0);
                }
					
				trail = StrokeUtils.SmoothList (trail, .9f);
				this.PushTrailToLine(Mathf.Max(0,this.currentPlaybackIndex-lineMaxVertexCount),trail.Count);
            }

			foreach (StrokeAnimation a in animations) {
				a.HandlePlay(.5f);
			}

            this.timer += Time.deltaTime;
        }

		public override void AddStrokePoint(StrokePoint newPoint) {
			
			this.trail.Add(newPoint);
			this.nTrail.Add (StrokeUtils.AddNoiseToVec(newPoint.vec));
			this.nLerp.Add (0);
		}

		public void lerpTrail(){
			for (int i = 0; i < currentPlaybackIndex; i++) {
				nLerp [i] += noiseDispersion;
			}
		}

		protected override void PushTrailToLine(int start, int finish) {
			
			Vector3[] arr = StrokeUtils.ListToArray(trail,start, finish);
			Vector3[] nArr = StrokeUtils.ListToArray(nTrail,start, finish);
			List<float> LLerp = nLerp.GetRange (start, finish - start);
			line.SetVertexCount(arr.Length);
			line.SetPositions(StrokeUtils.LerpArrays(arr,nArr,LLerp));
		}

		public override void SetTrail(List<StrokePoint> newTrail)
		{
			this.hasBeenDrawn = true;
			this.trail = newTrail;
			for (int i = 0; i < this.trail.Count; i++) {
				this.nTrail.Add (StrokeUtils.AddNoiseToVec(this.trail[i].vec));
				this.nLerp.Add (0);
			}
		}

		public void registerAnimators(GameObject g){

			StrokeAnimation[] playerScripts = g.GetComponents<StrokeAnimation>();
			if (playerScripts.Length > 0) {
				foreach (StrokeAnimation anims in playerScripts) {
					animations.Add (anims);
				}
			}
			for (int i = 0; i < g.transform.childCount; i++) {
				registerAnimators (g.transform.GetChild (i).gameObject);
			}
		}

        public override void FinishDraw() {
//			print(this.trail.Count);
            this.hasBeenDrawn = true;
            this.timeOffset = trail[0].time;
            this.SwitchToState(StrokeState.FINISH);
        }

        protected override void Reset() {
            base.Reset();
			if(trail.Count>0)
            	this.root.localPosition = trail[0].vec;
            this.timer = 0f;
            this.strokeWidth = GlobalValuesAndSettings.Instance.STROKE_START_WIDTH;
            this.PushTrailToLine(0, 0);
        }

        protected override void HandleIdle() {
            //throw new NotImplementedException();
        }

        protected override void HandleStart() {
            //throw new NotImplementedException();
        }

        protected override void HandlePlay() {
			//			if (currentPlaybackIndex < trail.Count-1) {
			//				while (trail [currentPlaybackIndex].time < timer + timeOffset && currentPlaybackIndex < trail.Count-1) {
			this.currentPlaybackIndex++;
			//				}
			//			}
			if (this.currentPlaybackIndex > trail.Count - 1)
				this.currentPlaybackIndex = trail.Count - 1;

			if (this.currentPlaybackIndex >= trail.Count - 1) {
				this.SwitchToState(StrokeState.FINISH);
				return;
			}
				
			this.root.position = trail[currentPlaybackIndex].vec;
			this.PushTrailToLine(Mathf.Max(0,currentPlaybackIndex-lineMaxVertexCount), currentPlaybackIndex);
			lerpTrail ();

			float v = currentPlaybackIndex / (float)(trail.Count-1);
			foreach (StrokeAnimation a in animations) {
				a.HandlePlay(v);
			}


			timer += Time.deltaTime;

        }


        protected override void HandleFinish() {

			lerpTrail ();
			this.PushTrailToLine(Mathf.Max(0,currentPlaybackIndex-lineMaxVertexCount), currentPlaybackIndex);

			foreach (StrokeAnimation a in animations) {
				endTimer += fadeSpeed * Time.deltaTime;
				a.HandleFinish (endTimer);
			}

            if (endTimer>=1) {
				endTimer = 0;
                this.SwitchToState(StrokeState.IDLE);

            }

        }
			

        protected override void OnIdle() {

            if (this.isFlaggedForDeath) {
                Destroy(this.gameObject);
                return;
            }

            this.Reset();
        }

		protected override void Start(){
			registerAnimators (this.gameObject);
		}


        protected override void OnStart() {
			
            this.PlayAudio();

            
            foreach(StrokeAnimation a in animations) {
                a.OnStart();
            }

            this.SwitchToState(StrokeState.PLAY);
        }

        protected override void OnPlay() {
			
			resetLerp ();

            foreach (StrokeAnimation a in animations) {
                a.OnPlay();
            }
        }

		public void resetLerp(){
			for (int i = 0; i < nLerp.Count; i++) {
				nLerp [i] = 0;
			}
		}

        protected override void OnFinish() {
            foreach(StrokeAnimation a in animations) {
                a.OnFinish();
            }
        }


    }
}


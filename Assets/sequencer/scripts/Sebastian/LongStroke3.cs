using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace HolojamEngine {
    public class LongStroke3 : Stroke {

		public float drawThreshold = 0.05f;
		public float strokeLength = 1f;
		public Stroke subStrokePrefab;

		private float timer = 0;
		private Vector3 previousDrawVector = Vector3.zero;
		private List<Stroke> subStrokes = new List<Stroke>();
		private bool hasSubStrokesFinished = false;

		private int strokeCount = 0;

		public float fadeSpeed = 1;
		private float endTimer = 0f;

		public int numStrokes = 5;

		public override void Draw(Vector3 v) {
			this.root.position = v;


			if (Vector3.Distance(this.previousDrawVector, v) > this.drawThreshold) {

				Stroke s = null;

				if (strokeCount<numStrokes) {
					timer = 0f;
					s = this.MakeSubStroke (new StrokePoint (v, timer, s));
					strokeCount++;
				}

				StrokePoint point = new StrokePoint(v, timer, s);
				this.trail.Add(point);

				float off = 0;

				foreach (Stroke stroke in subStrokes) {
					off += .335f;
					Vector3 v2 = StrokeUtils.AddNoiseToVec (v,.1f,off,averageDistance()*5);
					stroke.AddStrokePoint(new StrokePoint (v2, timer, s));
					stroke.Play ();

				}
			}

			foreach (StrokeAnimation a in animations) {
				a.HandlePlay(1);
			}

			this.timer += Time.deltaTime;
		}

		public float averageDistance(){
			float r = 0;
			int q = 5;
			if (trail.Count < 5)
				q = trail.Count;
			for (int i = trail.Count - 1; i > trail.Count - q; i--) {
				r += Vector3.Distance(trail [i].vec,trail[i-1].vec);
			}
			r /= q;
			return r;
		}

		public override void FinishDraw() {
			this.hasBeenDrawn = true;
			this.SwitchToState(StrokeState.FINISH);
		}

		protected override void Reset() {
			base.Reset();
			this.root.localPosition = trail[0].vec;
			this.timer = 0f;
			this.strokeWidth = GlobalValuesAndSettings.Instance.STROKE_START_WIDTH;
			this.PushTrailToLine(0, 0);
		}

		protected override void Start() {
			base.Start();
			registerAnimators (this.gameObject);
			this.audio.loop = true;
			this.canRestartFromFinish = false;
		}

		protected override void OnIdle() {
			if (this.isFlaggedForDeath) {
				Destroy(this.gameObject);
				//TO-DO: DESTROY SUBTRAILS
			}

			this.Reset();
		}

		protected override void OnStart() {
			this.PlayAudio();
			foreach (StrokeAnimation a in animations) {
				a.OnStart();
			}

			this.SwitchToState(StrokeState.PLAY);
		}

		protected override void OnPlay() {
			foreach (StrokeAnimation a in animations) {
				a.OnPlay();
			}
		}

		protected override void OnFinish() {
//			foreach (Stroke stroke in subStrokes) {
//				stroke.selfPlaying = true;
//			}
			foreach (StrokeAnimation a in animations) {
				a.OnFinish();
			}
		}

		protected override void HandleIdle() {
		}

//		public override void SelfPlay() {
//		}
//
		protected override void HandleStart() {
		}

		protected override void HandlePlay() {


			foreach (StrokeAnimation a in animations) {
				a.HandlePlay(.5f);
			}

			if (trail[currentPlaybackIndex].time >= timer) {
				this.root.position = trail[currentPlaybackIndex].vec;
				//this.PushTrailToLine(0, currentPlaybackIndex);

				if (trail[currentPlaybackIndex].subStroke != null) {
					trail[currentPlaybackIndex].subStroke.Play();
				}

				this.currentPlaybackIndex++;

			}

			if (this.currentPlaybackIndex == this.trail.Count) {
				this.SwitchToState(StrokeState.FINISH);
			}

//			foreach (Stroke stroke in subStrokes) {
//				stroke.Play ();
//			}
		}

		protected override void HandleFinish() {
//			line.SetWidth(this.strokeWidth, this.strokeWidth);


			foreach (StrokeAnimation a in animations) {
				a.HandleFinish (endTimer);
			}


			if (!this.hasSubStrokesFinished) {
				this.hasSubStrokesFinished = this.CheckSubStrokes();
			} else if (this.endTimer < 1.0) {
				endTimer += fadeSpeed * Time.deltaTime;
			} else {
				endTimer = 0;
				this.SwitchToState(StrokeState.IDLE);
			}
		}

		private bool CheckSubStrokes() {
			foreach (Stroke stroke in subStrokes) {
				if (!stroke.GetState().Equals(StrokeState.IDLE)) {
					return false;
				}
			}
			return true;
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

		private Stroke MakeSubStroke(StrokePoint p) {
			Stroke stroke = GameObject.Instantiate<Stroke>(subStrokePrefab);
			stroke.SetTrail(trail.GetRange(0,trail.Count));
			print (trail.Count);
			stroke.AddStrokePoint (p);
			subStrokes.Add(stroke);
			return stroke;
		}
	}
}


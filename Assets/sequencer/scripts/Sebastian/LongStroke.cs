using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HolojamEngine {
    public class LongStroke : Stroke {
        public float drawThreshold = 0.05f;
        public float strokeLength = 1f;
        public Stroke subStrokePrefab;

        private float timer = 0f;
        private Vector3 previousDrawVector = Vector3.zero;
        private List<Stroke> subStrokes = new List<Stroke>();
        private bool hasSubStrokesFinished = false;

		private bool firstStroke = false;

        public override void Draw(Vector3 v) {
            this.root.position = v;


            if (Vector3.Distance(this.previousDrawVector, v) > this.drawThreshold) {

                Stroke s = null;

                if (!firstStroke || this.timer > this.strokeLength) {
                    s = this.MakeSubStroke();
                    timer = 0f;
					firstStroke = true;
                }

                StrokePoint point = new StrokePoint(v, timer, s);
  
                this.trail.Add(point);
                
                foreach (Stroke stroke in subStrokes) {
					if (stroke != s)
                    	stroke.AddStrokePoint(point);
                }


                this.PushTrailToLine(0, trail.Count);
            }

            this.timer += Time.deltaTime;
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
            foreach (StrokeAnimation a in animations) {
                a.OnFinish();
            }
        }

        protected override void HandleIdle() {
        }

//		public override void SelfPlay() {
//		}

        protected override void HandleStart() {
        }

        protected override void HandlePlay() {


            if (trail[currentPlaybackIndex].time >= timer) {
                this.root.position = trail[currentPlaybackIndex].vec;
                this.PushTrailToLine(0, currentPlaybackIndex);

                if (trail[currentPlaybackIndex].subStroke != null) {
                    trail[currentPlaybackIndex].subStroke.Play();
                }

                this.currentPlaybackIndex++;

            }

            if (this.currentPlaybackIndex == this.trail.Count) {
                this.SwitchToState(StrokeState.FINISH);
            }
        }

        protected override void HandleFinish() {
            line.SetWidth(this.strokeWidth, this.strokeWidth);

            if (!this.hasSubStrokesFinished) {
                this.hasSubStrokesFinished = this.CheckSubStrokes();
            } else if (this.strokeWidth > 0.01f) {
                this.strokeWidth *= 0.97f;
            } else {
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

        private Stroke MakeSubStroke() {
            Stroke stroke = GameObject.Instantiate<Stroke>(subStrokePrefab);
            stroke.SetTrail(trail);
            stroke.Play();
            subStrokes.Add(stroke);
            return stroke;
        }
    }
}


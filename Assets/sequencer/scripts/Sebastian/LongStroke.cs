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

        public override void Draw(Vector3 v) {
            this.root.position = v;
            this.timer += Time.deltaTime;

            if (Vector3.Distance(this.previousDrawVector, v) > this.drawThreshold) {
                this.trail.Add(v);
                this.PushTrailToLine(0, trail.Count);

                if (this.timer > this.strokeLength) {
                    this.MakeSubStroke();
                }
            }
        }

        public override void FinishDraw() {
            this.hasBeenDrawn = true;
            this.SwitchToState(StrokeState.FINISH);
        }

        protected override void Reset() {
            base.Reset();
            this.root.localPosition = trail[0];
            this.strokeWidth = GlobalValuesAndSettings.Instance.STROKE_START_WIDTH;
            this.PushTrailToLine(0, 0);
        }

        protected override void Start() {
            base.Start();
            this.audio.loop = true;
        }

        protected override void OnIdle() {
            this.Reset();

            if (this.isFlaggedForDeath) {
                Destroy(this.gameObject);
                //TO-DO: DESTROY SUBTRAILS
            }
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

        protected override void HandleStart() {
        }

        protected override void HandlePlay() {
            this.root.position = trail[currentPlaybackIndex];
            this.PushTrailToLine(0, this.currentPlaybackIndex);
            this.currentPlaybackIndex++;

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

        private void MakeSubStroke() {
            Stroke stroke = GameObject.Instantiate<Stroke>(subStrokePrefab);
            stroke.SetTrail(StrokeUtils.AddNoiseToList(this.trail.GetRange(0, trail.Count), 0.9f));
            stroke.Play();
        }
    }
}


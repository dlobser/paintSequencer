using UnityEngine;
using System.Collections;
using System;

namespace HolojamEngine {
    public class ShortStroke : Stroke {

        public float drawThreshold = 0.05f;

        private float timer = 0f;
        private Vector3 previousDrawVector = Vector3.zero;

        public override void Draw(Vector3 v) {
            this.root.position = v;
            this.timer += Time.deltaTime;
            if (Vector3.Distance(this.previousDrawVector, v) > this.drawThreshold) {
                this.trail.Add(v);

                if (this.trail.Count > this.trailMaxVertexCount)
                    this.trail.RemoveAt(0);

                this.PushTrailToLine(0,trail.Count);
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

        protected override void HandleIdle() {
            //throw new NotImplementedException();
        }

        protected override void HandleStart() {
            //throw new NotImplementedException();
        }

        protected override void HandlePlay() {
            this.root.position = trail[currentPlaybackIndex];
            this.PushTrailToLine(0, currentPlaybackIndex);
            this.currentPlaybackIndex++;

            float v = currentPlaybackIndex / (float)(trail.Count-1);
            foreach (StrokeAnimation a in animations) {
                a.HandlePlay(v);
            }

            if (this.currentPlaybackIndex == trail.Count)
                this.SwitchToState(StrokeState.FINISH);
        }

        protected override void HandleFinish() {
            line.SetWidth(this.strokeWidth, this.strokeWidth);

            if (this.strokeWidth > 0.01f) {
                this.strokeWidth *= 0.97f;
            } else {
                this.SwitchToState(StrokeState.IDLE);
            }
        }





        protected override void OnIdle() {
            this.Reset();

            if (this.isFlaggedForDeath) {
                Destroy(this.gameObject);
            }

        }

        protected override void OnStart() {
            this.PlayAudio();
            
            foreach(StrokeAnimation a in animations) {
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
            foreach(StrokeAnimation a in animations) {
                a.OnFinish();
            }
        }


    }
}


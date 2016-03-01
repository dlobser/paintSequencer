using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace HolojamEngine {
    public class ShortStroke : Stroke {

        //fix timer

        public float drawThreshold = 0.05f;



        private float timer = 0f;
        private float timeOffset = 0f;
        private Vector3 previousDrawVector = Vector3.zero;

        public override void Draw(Vector3 v) {
            this.root.position = v;
            if (Vector3.Distance(this.previousDrawVector, v) > this.drawThreshold) {
                this.trail.Add(new StrokePoint(v,timer));
                if (this.trail.Count == this.trailMaxVertexCount) {
                    this.trail.RemoveAt(0);
                }

				trail = StrokeUtils.SmoothList (trail, .95f);

                this.PushTrailToLine(0,trail.Count);
            }

            this.timer += Time.deltaTime;
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
            this.hasBeenDrawn = true;
            this.timeOffset = trail[0].time;
            this.SwitchToState(StrokeState.FINISH);
        }

        protected override void Reset() {
            base.Reset();
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
//		public override void SelfPlay() {
//			//throw new NotImplementedException();
//		}

        protected override void HandlePlay() {


						if (currentPlaybackIndex < trail.Count-1) {
							while (trail [currentPlaybackIndex].time < timer + timeOffset && currentPlaybackIndex < trail.Count-1) {
			this.currentPlaybackIndex++;
							}
						}
			if (this.currentPlaybackIndex > trail.Count - 1)
				this.currentPlaybackIndex = trail.Count - 1;
			

//            if (trail[currentPlaybackIndex].time <= timer + timeOffset) {
                this.root.position = trail[currentPlaybackIndex].vec;
                this.PushTrailToLine(0, currentPlaybackIndex);
//                this.currentPlaybackIndex++;
//            }

            float v = currentPlaybackIndex / (float)(trail.Count-1);
            foreach (StrokeAnimation a in animations) {
                a.HandlePlay(v);
            }

			if (this.currentPlaybackIndex >= trail.Count-1)
				this.SwitchToState(StrokeState.FINISH);

            timer += Time.deltaTime;
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

            if (this.isFlaggedForDeath) {
                Destroy(this.gameObject);
                return;
            }

            this.Reset();
        }

        protected override void OnStart() {
            this.PlayAudio();


			registerAnimators (this.gameObject);
            
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


using UnityEngine;
using System.Collections;


namespace HolojamEngine {
    public class AnimateTrailOff : StrokeAnimation {
		TrailRenderer trail;
		public float length;
		void Awake(){
			trail = GetComponent<TrailRenderer> ();
		}

		public override void OnStart(){
		}
		public override void OnPlay(){
			trail.time = length;
		}
		public override void OnFinish(){
		}
		public override void HandlePlay(float t){
			trail.time = length;
		}
		public override void HandleFinish(float t){
			trail.time = 0;
		}
    }
}


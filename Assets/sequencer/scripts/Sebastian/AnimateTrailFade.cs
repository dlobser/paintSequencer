using UnityEngine;
using System.Collections;


namespace HolojamEngine {
    public class AnimateTrailFade : StrokeAnimation {
		
		LineRenderer line;
		public float lineWidth;
		public float growSpeed;
		private float lineCounter;

		void Awake(){
			line = this.gameObject.GetComponent<LineRenderer>();

		}
		public override void OnStart(){
			line.SetWidth (0, 0);

		}
		public override void OnPlay(){
//			aud.Play ();
		}
		public override void OnFinish(){
		}
		public override void HandlePlay(float t){
			
			if (lineCounter < lineWidth) {
				lineCounter += growSpeed*Time.deltaTime;
				line.SetWidth (0, lineCounter);
			}
	
		}
		public override void HandleFinish(float t){
//			if (aud.volume > 0)
			lineCounter  = (1-t)*lineCounter;
			line.SetWidth (0, lineCounter);
		}
    }
}


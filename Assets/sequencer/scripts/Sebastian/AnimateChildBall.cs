using UnityEngine;
using System.Collections;


namespace HolojamEngine {
    public class AnimateChildBall : StrokeAnimation {

		public GameObject child;

		public float scale = .02f;

		public override void OnStart(){
		}
		public override void OnPlay(){
		}
		public override void OnFinish(){
		}
		public override void HandlePlay(float t){
			Debug.Log (t);
			float sc = Mathf.Max(.001f, (-Mathf.Cos (t * Mathf.PI * 2) + 1) * scale);
			child.transform.localScale = new Vector3 (sc, sc, sc);	
		}
		public override void HandleFinish(float t){
			float sc = Mathf.Max(.001f, transform.localScale.x);
			sc *= .95f;
			child.transform.localScale = new Vector3 (sc, sc, sc);	
		}
    }
}


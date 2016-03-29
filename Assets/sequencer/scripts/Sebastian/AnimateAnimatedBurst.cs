using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HolojamEngine {
	public class AnimateAnimatedBurst : StrokeAnimation {

		public GameObject burst;
		GameObject thisBurst;

		void Awake(){
			thisBurst = Instantiate(burst);
			burst.SetActive (false);
		}

		public override void OnStart(){
			thisBurst.transform.localPosition = transform.localPosition;
		}

		public override void OnPlay(){
			thisBurst.GetComponent<Animator> ().SetTrigger ("play");
//			print ("play");
		}
		public override void OnFinish(){}

		public override void HandlePlay(float t){
			
		}

		public override void HandleFinish(float t){
					
		}

	}
}

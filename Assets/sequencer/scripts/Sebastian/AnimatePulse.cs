using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HolojamEngine {
	public class AnimatePulse : StrokeAnimation {

		Material mat;
		public GameObject pulsar;
		public float speed = 3;
		public bool burstAtStart = true;
		public bool burstAtEnd = false;

		void Awake(){
			mat = pulsar.GetComponent<MeshRenderer> ().material;
		}

		public override void OnStart(){}

		public override void OnPlay(){}
		public override void OnFinish(){}

		public override void HandlePlay(float t){
			if (burstAtStart) {
				t *= speed;
				mat.SetFloat ("_Radius", (((1 - t) * 1.5f) - 1));
				mat.SetColor ("_Color", new Color (1, 1, 1, 1 - t));
			}
		}

		public override void HandleFinish(float t){
			if (burstAtEnd) {
				t *= speed;
				mat.SetFloat ("_Radius", (((1 - t) * 1.5f) - 1));
				mat.SetColor ("_Color", new Color (1, 1, 1, 1 - t));
			}
		}

	}
}

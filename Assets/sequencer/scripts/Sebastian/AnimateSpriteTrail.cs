using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HolojamEngine {
	public class AnimateSpriteTrail : StrokeAnimation {

		public int amount;
		public int spacing;
		public GameObject sprite;
		GameObject[] sprites;
		public float spriteScale = .2f;
		public List<Vector3> trail;
		public List<float> spScale;

		void Awake(){
			sprites = new GameObject[amount];
			for (int i = 0; i < amount ; i++) {
				sprites [i] = Instantiate (sprite);
			}
			trail = new List<Vector3> ();
			spScale = new List<float> ();
			for (int i = 0; i < amount * spacing; i++) {
				trail.Add (transform.localPosition);
				spScale.Add (0);
			}
		}

		public override void OnStart(){

		}

		public override void OnPlay(){}
		public override void OnFinish(){}
		public override void HandlePlay(float t){
			animate(t,true);
		}
		public override void HandleFinish(float t){
			animate (t,false);
		}

		public void animate(float t, bool doScale){
			trail.Add (transform.localPosition);
			trail.RemoveAt (0);
		
			float sc = Mathf.Max(.001f, (-Mathf.Cos (t * Mathf.PI * 2) + 1) * spriteScale);
			if (!doScale) {
				sc = transform.localScale.x * (1 - t);
				for (int i = 0; i < amount * spacing; i++) {
					spScale [i] = spScale [i] * (1 - t);
				}
			}

			spScale.Add (sc);
			spScale.RemoveAt (0);

			for (int i = 0; i < sprites.Length; i++) {
				sprites [i].transform.localPosition = trail [i*spacing];
				sprites [i].transform.LookAt (Camera.main.transform.position);
//				if(doScale)
				sprites[i].transform.localScale = new Vector3 (spScale[i*spacing],spScale[i*spacing],spScale[i*spacing]);	
			}
		}
	}
}

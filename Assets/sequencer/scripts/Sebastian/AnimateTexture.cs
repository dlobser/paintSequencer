using UnityEngine;
using System.Collections;


namespace HolojamEngine {
    public class AnimateTexture : StrokeAnimation {
		
		Material mat;
		public Vector2 speed = Vector2.zero;
		private Vector2 off = Vector2.zero;

		void Awake(){
			mat = this.GetComponent<LineRenderer> ().material;
		}

		public override void OnStart(){}
		public override void OnPlay(){}
		public override void OnFinish(){}

		public override void HandlePlay(float t){
			off = new Vector2 (off.x + speed.x * Time.deltaTime, off.y + speed.y * Time.deltaTime);
			mat.mainTextureOffset = off;
//			mat.SetTextureOffset ("_MainTex", off);
		}

		public override void HandleFinish(float t){
			off = new Vector2 (off.x + speed.x * Time.deltaTime, off.y + speed.y * Time.deltaTime);
			mat.mainTextureOffset = off;
		}
    }
}


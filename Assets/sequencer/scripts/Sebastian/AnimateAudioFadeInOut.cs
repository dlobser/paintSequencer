using UnityEngine;
using System.Collections;


namespace HolojamEngine {
    public class AnimateAudioFadeInOut : StrokeAnimation {
		
		public float speed = 1;
		AudioSource aud;
		public float maxVolume = 1;

		void Awake(){
			aud = this.gameObject.GetComponent<AudioSource>();

		}
		public override void OnStart(){
			aud.volume = 0;
			aud.loop = true;

		}
		public override void OnPlay(){
//			aud.Play ();
		}
		public override void OnFinish(){
		}
		public override void HandlePlay(float t){
			if (aud.volume < maxVolume)
				aud.volume += speed*Time.deltaTime;
	
		}
		public override void HandleFinish(float t){
			if (aud.volume > 0)
				aud.volume  = maxVolume*(1-t);
		}
    }
}


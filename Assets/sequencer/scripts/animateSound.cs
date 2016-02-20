using UnityEngine;
using System.Collections;

public class animateSound : animate {

	AudioSource aud;
	public float speed;
	float endLength;
	float counter = 0;

	bool rising = false;
	bool falling = false;

	void Awake(){
		aud = GetComponent<AudioSource> ();
		aud.volume = 0;
		endLength = aud.clip.length-speed;
	}
	public override void TriggerOn(){}
	public override void TriggerOff(){	
        if(aud.volume>0)
		aud.volume -= speed * Time.deltaTime;
	}

	public override void Play(float t){
//		Debug.Log (aud.volume);	

			if (!rising && !falling) {
				rising = true;
				aud.volume = 0;
			} else if (rising && aud.volume < 1 && !falling) {
				counter = speed * Time.deltaTime;
				aud.volume += counter;
			} 
//			else if (rising && !falling) {
//				falling = true;
//				rising = false;
//			} else if (!rising && falling && counter > endLength && aud.volume > 0) {
//				aud.volume -= speed * Time.deltaTime;
//			} else if (!rising && falling && counter > aud.clip.length || aud.volume < 0) {
//				falling = false;
//				counter = 0;
//			}

	}

	public void VolumeDown(){
//		Debug.Log ("hi");
//		takeOver = true;
	}
}

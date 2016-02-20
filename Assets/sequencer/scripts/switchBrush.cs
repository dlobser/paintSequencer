using UnityEngine;
using System.Collections;

public class switchBrush : MonoBehaviour {

	beatGlobals beat;
	public int which = 0;

	void Awake(){
		beat = beatGlobals.Instance;
	}

	public void OnTriggerEnter(Collider other){
		beat.which = which;
	}
}

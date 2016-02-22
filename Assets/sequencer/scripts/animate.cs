using UnityEngine;
using System.Collections;

public abstract class animate : MonoBehaviour {

    //HANDLE START
	public abstract void TriggerOn ();
    //HANDLE FINISH
	public abstract void TriggerOff ();

    //HANDLE PLAY - VALUE BETWEEN 0 and 1 BASED ON AGE STROKE
	public abstract void Play (float t);

}

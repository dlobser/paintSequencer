using UnityEngine;
using System.Collections;

public abstract class animate : MonoBehaviour {

	public abstract void TriggerOn ();
	public abstract void TriggerOff ();

	public abstract void Play (float t);

}

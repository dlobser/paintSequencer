using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class animatePropellor : animate {

	List<Vector3> previous;
	int amount = 10;

	void Awake () {
		previous = new List<Vector3> ();
		previous.Add (Vector3.zero);
	}

	public override void TriggerOn(){}
	public override void TriggerOff(){}

	public override void Play(float t){
		transform.LookAt (previous[0]);
		previous.Add(transform.position);
		if (previous.Count > amount)
			previous.RemoveAt (0);
		float sc = Mathf.Max(.001f, (-Mathf.Cos (t * Mathf.PI * 2) + 1) * .1f);
		transform.GetChild(0).localScale = new Vector3 (sc, sc, sc);
	}
}

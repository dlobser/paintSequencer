using UnityEngine;
using System.Collections;

namespace HolojamEngine{
	public class RigidUIClear : UIObject {

		public Bulb bulb;
		public GameObject indicator;
		public float minScale;
		public float maxScale;

		void Update(){
			if (indicator.transform.localScale.x > minScale)
				indicator.transform.localScale = indicator.transform.localScale * .95f;
		}

		public override void OnEnter (Collider other){
			bulb.ClearStrokes ();
			indicator.transform.localScale = new Vector3 (maxScale, maxScale, maxScale);
		}
		public override void OnExit (Collider other){}
		public override void OnDrag (){}

	}
}
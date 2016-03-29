using UnityEngine;
using System.Collections;

namespace HolojamEngine{
	public class RigidUIIndicate : UIObject {

		public GameObject indicator;
		public float minScale;
		public float maxScale;

		void Start(){
			if (indicator == null)
				indicator = transform.GetChild (0).gameObject;
		}
		void Update(){
			if (indicator.transform.localScale.x > minScale)
				indicator.transform.localScale = indicator.transform.localScale * .95f;
		}

		public override void OnEnter (Collider other){
			indicator.transform.localScale = new Vector3 (maxScale, maxScale, maxScale);
		}
		public override void OnExit (Collider other){}
		public override void OnDrag (){}

	}
}
using UnityEngine;
using System.Collections;

namespace HolojamEngine{
	public class RigidUISlide : UIObject {

		public Bulb bulb;
		public Vector3 limit = Vector3.zero;
		GameObject controller;

		private bool controlling = false;

		void Update(){
			if(controlling)
				OnDrag ();
		}

		public override void OnEnter (Collider other){
			controller = other.gameObject;
			controlling = true;
		}

		public override void OnExit (Collider other){
			controller = null;
			controlling = false;
		}

		public override void OnDrag (){
			transform.position = controller.transform.position;
			Vector3 pos = transform.localPosition;

		

			transform.localPosition = new Vector3 (
				Mathf.Max(0.0f,Mathf.Min (limit.x, pos.x)),
				Mathf.Max(0.0f,Mathf.Min (limit.y, pos.y)),
				Mathf.Max(0.0f,Mathf.Min (limit.z, pos.z)));

			foreach(Stroke s in bulb.strokes){
				s.gameObject.GetComponent<AudioSource> ().pitch = transform.localPosition.magnitude + .5f;
			}
		}

	}
}
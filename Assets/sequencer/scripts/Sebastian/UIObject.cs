using UnityEngine;
using System.Collections;

namespace HolojamEngine{
	public abstract class UIObject : MonoBehaviour {

		void OnTriggerEnter(Collider other) {
			OnEnter (other);
		}

		void OnTriggerExit(Collider other) {
			OnExit (other);
		}

		abstract public void OnEnter (Collider other);
		abstract public void OnExit (Collider other);
		abstract public void OnDrag ();
	}
}
using UnityEngine;
using System.Collections;

namespace HolojamEngine{
	public class RigidUI : UIObject {

		public BulbManagerCollision BMC;


		public override void OnEnter (Collider other){BMC.ClearAllStrokes ();}
		public override void OnExit (Collider other){}
		public override void OnDrag (){}

	}
}
using UnityEngine;
using System.Collections;

namespace HolojamEngine {

	public class BrushCollision : MonoBehaviour {

		public Stroke paletteBrush;
		public bool isDrawing = false;
		public BulbManagerCollision BMC;
		public float minDistance = .01f;

		private Vector3[] checkTrail;
		private Bulb activeBulb;

		void Start(){
			checkTrail = new Vector3[4];
		}

		void OnTriggerEnter(Collider other) {
//			print (activeBulb);
			if (activeBulb == null) {
				print (other.GetComponent<paletteBrush> ()!=null);
				if (other.GetComponent<paletteBrush> () != null) {
					print ("hi");
					paletteBrush = other.GetComponent<paletteBrush> ().BrushPrefab;
					isDrawing = true;
					activeBulb = BMC.FindClosestBulb (other.transform.position);

					activeBulb.strokePrefab = paletteBrush;// strokePrefabs [currentStrokeIndex];
					activeBulb.DrawStroke (transform.position);
					activeBulb.display.transform.localScale = Vector3.one * 3;
				}

			}
		}

		void Update(){

			if (isDrawing) {

				activeBulb.DrawStroke (transform.position);

	            

			} else {
				if (activeBulb) {
					activeBulb.FinishStroke();
					activeBulb = null;
				}
			}

			checkForStopping (transform.position);

		}

		void checkForStopping(Vector3 v){

			for (int i = 0; i < checkTrail.Length-1; i++) {
				checkTrail [i] = checkTrail [i + 1];
			}
			checkTrail [checkTrail.Length - 1] = v;
			float d = 0;
			for (int i = 0; i < checkTrail.Length-1; i++) {
				d += Vector3.Distance (checkTrail [i], checkTrail [i + 1]);
			}
			if (d < minDistance)
				isDrawing = false;
		}
	}
}

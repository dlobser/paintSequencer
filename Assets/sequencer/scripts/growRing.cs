using UnityEngine;
using System.Collections;

namespace HolojamEngine {

	public class growRing : MonoBehaviour {

		BulbManager bm;
		Material mat;
		float bulbAmount;
		float ring = 0;
		int whichRing = 0;
		private float bpm;
		public GameObject Ring;
		public Color col;
		// Use this for initialization
		void Start () {
			bm = this.GetComponent<BulbManager> ();
			mat = Ring.GetComponent<MeshRenderer> ().material;
			bulbAmount = bm.amount;
			bpm = bm.bpm;
		}
		
		// Update is called once per frame
		void Update () {
			float scale = Mathf.Lerp ((float)bm.bulbCounter, (float)bm.bulbCounter + 1, map (bm.timer, 0, bm.bpm, 0, 1));
			mat.SetFloat ("_Radius", map(scale,0,(float)bm.amount,.5f,-.5f));
			mat.SetColor("_Color",col*new Color(1,1,1, (Mathf.Cos( map(scale,0,(float)bm.amount,0,6.24f))*-1+1)*.2f ));
//			print (map(scale,0,(float)bm.amount,.5f,-.5f));
		}

		float map(float s, float a1, float a2, float b1, float b2)
		{
			return b1 + (s-a1)*(b2-b1)/(a2-a1);
		}
	}
}
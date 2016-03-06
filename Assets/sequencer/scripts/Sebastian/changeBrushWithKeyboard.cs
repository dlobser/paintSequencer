using UnityEngine;
using System.Collections;

namespace HolojamEngine {
	public class changeBrushWithKeyboard : MonoBehaviour {

		BulbManager BM;
		// Use this for initialization
		void Start () {
			BM = this.GetComponent<BulbManager> ();
		}
		
		// Update is called once per frame
		void Update () {
//			for (int i = 0; i < 9; i++) {
			if (Input.GetKeyUp (KeyCode.Alpha0)) BM.currentStrokeIndex = 0;
			if (Input.GetKeyUp (KeyCode.Alpha1)) BM.currentStrokeIndex = 1;
			if (Input.GetKeyUp (KeyCode.Alpha2)) BM.currentStrokeIndex = 2;
			if (Input.GetKeyUp (KeyCode.Alpha3)) BM.currentStrokeIndex = 3;
			if (Input.GetKeyUp (KeyCode.Alpha4)) BM.currentStrokeIndex = 4;
			if (Input.GetKeyUp (KeyCode.Alpha5)) BM.currentStrokeIndex = 5;
			if (Input.GetKeyUp (KeyCode.Alpha6)) BM.currentStrokeIndex = 6;
			if (Input.GetKeyUp (KeyCode.Alpha7)) BM.currentStrokeIndex = 7;
			if (Input.GetKeyUp (KeyCode.Alpha8)) BM.currentStrokeIndex = 8;
			if (Input.GetKeyUp (KeyCode.Alpha9)) BM.currentStrokeIndex = 9;
			if (Input.GetKeyUp (KeyCode.A)) BM.currentStrokeIndex = 10;
			if (Input.GetKeyUp (KeyCode.B)) BM.currentStrokeIndex = 11;
			if (Input.GetKeyUp (KeyCode.C)) BM.currentStrokeIndex = 12;
			if (Input.GetKeyUp (KeyCode.D)) BM.currentStrokeIndex = 13;
			if (Input.GetKeyUp (KeyCode.E)) BM.currentStrokeIndex = 14;
			if (Input.GetKeyUp (KeyCode.F)) BM.currentStrokeIndex = 15;
			if (Input.GetKeyUp (KeyCode.G)) BM.currentStrokeIndex = 16;
//			}
		}
	}
}

using UnityEngine;
using System.Collections;

namespace HolojamEngine{
	public class extendController : MonoBehaviour {

		public float speed;
		public float max;
		public float min;
		public GameObject stem;
		public GameObject ball;
		BrushCollision BC;

		private Vector3 prev;

		private float counter =0 ;
		private float countUp = 0;

		// Use this for initialization
		void Start () {
			BC = this.GetComponent<BrushCollision> ();	
			counter = min;
		}
		
		// Update is called once per frame
		void Update () {
			float dist = Vector3.Distance (prev, transform.position);
			if (BC.isDrawing) {
				if (countUp < speed && counter<max)
					countUp += .00002f ;
				if (counter < max)
					counter += countUp * Time.deltaTime + dist*.05f;
//				else
//					countUp -= .0002f;
				
			} else {
				if (countUp < speed && counter > min)
					countUp += .1f;
				if (counter > min)
					counter -= countUp * Time.deltaTime;
				else if(countUp>0)
					countUp -= .1f;
			}
			if (counter < min)
				counter = min;
			if (countUp < 0)
				countUp = 0;
			prev = transform.position;
			float x = stem.transform.localScale.x;
			stem.transform.localScale = new Vector3 (x, counter, x);
			stem.transform.localPosition = new Vector3 (0, counter*.5f, 0);
			ball.transform.localPosition = new Vector3 (0, counter, 0);
		}
	}
}
using UnityEngine;
using System.Collections.Generic;

namespace HolojamEngine {
    public class Bulb : MonoBehaviour {


        ////////////////////////////////////////
        //state machine enumerator
        ////////////////////////////////////////


        ////////////////////////////////////////
        //public collections & vars
        ////////////////////////////////////////

        public float pitch = 1f;
        public Stroke strokePrefab;
		public GameObject display;
		public float minScale = 1;
		public float maxScale = 2;
		public GameObject Indicator;

        ////////////////////////////////////////
        //protected/private collections & vars
        ////////////////////////////////////////
        public List<Stroke> strokes = new List<Stroke>();

        private Stroke activeStroke;

        ////////////////////////////////////////
        //inherited functions
        ////////////////////////////////////////

        //occurs before all gameobject initialization. ( equivalent to constructor )
        void Awake() {
			if (display == null)
				display = this.gameObject;
        }

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {
            if (display.transform.localScale.x > minScale) {
				display.transform.localScale = Vector3.one*display.transform.localScale.x*.95f;

//                display.transform.localScale = Vector3.Scale(this.transform.localScale, Vector3.one * 0.95f);
            }
			if (Indicator.transform.localScale.x > 0) {
				Indicator.transform.localScale = Vector3.one*Indicator.transform.localScale.x*.95f;
			}
			for (int i = 0; i < this.strokes.Count; i++) {
				if (this.strokes [i] == null)
					this.strokes.Remove (this.strokes [i]);
			}
        }

        ////////////////////////////////////////
        //custom functions
        ////////////////////////////////////////
        
        public void Pulse() {
            foreach (Stroke stroke in this.strokes) {
                stroke.Play();
            }
            display.transform.localScale = Vector3.one * maxScale;
        }

        public void DrawStroke(Vector3 v) {
            if (!this.activeStroke) {
                activeStroke = GameObject.Instantiate<Stroke>(strokePrefab);
                activeStroke.SetAudioPitch(pitch);
                strokes.Add(activeStroke);
                pitch *= 0.9f;
                // activeStroke.transform.SetParent(this.transform);
            } else {
                activeStroke.Draw(v);
            }
        }

        public void FinishStroke() {
            if (activeStroke) {
                activeStroke.FinishDraw();
                activeStroke = null;
            }
        }

        public void ClearStrokes() {
//			for (int i = 0; i < this.strokes.Count; i++) {
//				Stroke stroke = this.strokes [0];
//				GameObject.Destroy (stroke.gameObject);
//				this.strokes.RemoveAt (0);
//			}
            foreach (Stroke stroke in this.strokes) {
//				print (stroke);
//				GameObject.Destroy (stroke);
                stroke.FlagForDeath();
            }
//            this.strokes.Clear();
            this.pitch = 1f;
        }

        ////////////////////////////////////////
        //abstract functions
        ////////////////////////////////////////


    }
}


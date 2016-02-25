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

        ////////////////////////////////////////
        //protected/private collections & vars
        ////////////////////////////////////////
        private List<Stroke> strokes = new List<Stroke>();

        private Stroke activeStroke;

        ////////////////////////////////////////
        //inherited functions
        ////////////////////////////////////////

        //occurs before all gameobject initialization. ( equivalent to constructor )
        void Awake() {

        }

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {
            if (this.transform.localScale.x > GlobalValuesAndSettings.Instance.BULB_MIN_SCALE) {
                this.transform.localScale = Vector3.Scale(this.transform.localScale, Vector3.one * 0.95f);
            }
        }

        ////////////////////////////////////////
        //custom functions
        ////////////////////////////////////////
        
        public void Pulse() {
            foreach (Stroke stroke in this.strokes) {
                stroke.Play();
            }
            this.transform.localScale = Vector3.one * GlobalValuesAndSettings.Instance.BULB_MAX_SCALE;
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
            foreach (Stroke stroke in this.strokes) {
                stroke.FlagForDeath();
            }
            this.strokes.Clear();
            this.pitch = 1f;
        }

        ////////////////////////////////////////
        //abstract functions
        ////////////////////////////////////////


    }
}


using UnityEngine;
using System.Collections.Generic;

namespace HolojamEngine {
    public class BulbManager : MonoBehaviour {


        ////////////////////////////////////////
        //state machine enumerator
        ////////////////////////////////////////


        ////////////////////////////////////////
        //public collections & vars
        ////////////////////////////////////////
        public List<Bulb> bulbs = new List<Bulb>();

        public Bulb bulbPrefab;
        public float radius = 5f;
        public float amount = 16;
        public float bpm = 0.1f;
        public float height = 0f;

        ////////////////////////////////////////
        //protected/private collections & vars
        ////////////////////////////////////////
        private Bulb activeBulb;
        private float timer = 0f;
        private int bulbCounter = 0;
        ////////////////////////////////////////
        //inherited functions
        ////////////////////////////////////////

        //occurs before all gameobject initialization. ( equivalent to constructor )
        void Awake() {

        }

        // Use this for initialization
        void Start() {
            for (int i = 0; i < amount; i++) {
                Bulb nb = GameObject.Instantiate<Bulb>(bulbPrefab);
                nb.transform.position = new Vector3(
                Mathf.Sin(((float)i / (float)amount) * Mathf.PI * 2) * radius,
                height,
                Mathf.Cos(((float)i / (float)amount) * Mathf.PI * 2) * radius);
                bulbs.Add(nb);
            }
        }

        // Update is called once per frame
        void Update() {
            if (Input.GetKeyDown(KeyCode.D)) {
                Bulb b = this.FindClosestBulb(this.HitPoint());
                if (b)
                    b.ClearStrokes();
            }

            if (Input.GetMouseButton(0)) {
                Vector3 hit = this.HitPoint();
                if (activeBulb == null) {
                    activeBulb = this.FindClosestBulb(hit);
                    activeBulb.DrawStroke(hit);
                } else {
                    activeBulb.DrawStroke(hit);
                }
            }

            if (Input.GetMouseButtonUp(0)) {
                if (activeBulb) {
                    activeBulb.FinishStroke();
                    activeBulb = null;
                }
            }


            timer += Time.deltaTime;
            if (timer > bpm) {
                timer = 0;

                bulbs[bulbCounter++].Pulse();

                if (bulbCounter > bulbs.Count - 1) {
                    bulbCounter = 0;
                }
            }
        }

        ////////////////////////////////////////
        //custom functions
        ////////////////////////////////////////
        public Bulb FindClosestBulb(Vector3 v) {
            Bulb closestBulb = null;
            float min = 1e6f;

            foreach (Bulb bulb in bulbs) {
                float dist = Vector3.Distance(bulb.transform.position, v);
                if (dist < min) {
                    min = dist;
                    closestBulb = bulb;
                }
            }
            return closestBulb;
        }

        public Vector3 HitPoint() {
            Vector3 vec = Vector3.zero;
            RaycastHit vHit = new RaycastHit();
            Ray vRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(vRay, out vHit, 1000)) {
                vec = vHit.point;
            }
            return vec;
        }

        ////////////////////////////////////////
        //abstract functions
        ////////////////////////////////////////
    }
}


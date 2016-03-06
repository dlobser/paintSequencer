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
        public List<Stroke> strokePrefabs;

        public Bulb bulbPrefab;
        public float radius = 5f;
        public float amount = 16;
        public float bpm = 0.1f;
        public float height = 0f;

        ////////////////////////////////////////
        //protected/private collections & vars
        ////////////////////////////////////////
        private Bulb activeBulb;
        public int currentStrokeIndex;
        public float timer = 0f;
        public int bulbCounter = 0;

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
				if (i % 4 == 0)
					nb.minScale = nb.minScale * 3;
				nb.transform.position = new Vector3 ((1+i)*radius, 0, 0);
//                nb.transform.position = new Vector3(
//                Mathf.Sin(((float)i / (float)amount) * Mathf.PI * 2) * radius,
//                height,
//                Mathf.Cos(((float)i / (float)amount) * Mathf.PI * 2) * radius);
                bulbs.Add(nb);
            }

            currentStrokeIndex = 0;
        }

        // Update is called once per frame
        void Update() {
            //SWITCH STROKE PREFAB
            if (Input.GetKeyDown(KeyCode.S))
            {
                this.currentStrokeIndex = (currentStrokeIndex + 1 == strokePrefabs.Count ? 0 : currentStrokeIndex + 1);
            }
			Vector3 hit = this.HitPoint();
			this.FindClosestBulb (hit).Indicator.transform.localScale = Vector3.one *.5f;

            //START/DRAW STROKE
            if (Input.GetMouseButton(0)) {
//                Vector3 hit = this.HitPoint();
                if (activeBulb == null) {
					//JUST FOR SCREENSPACE 
					if (hit.x * radius > 0) {
//						print (hit.x);
						activeBulb = this.FindClosestBulb (hit);
						activeBulb.strokePrefab = strokePrefabs [currentStrokeIndex];
						activeBulb.DrawStroke (hit);
						activeBulb.display.transform.localScale = Vector3.one * 3;
					}
                } else {
                    activeBulb.DrawStroke(hit);
                }
            }

            //FINISH STROKE
            if (Input.GetMouseButtonUp(0)) {
                if (activeBulb) {
                    activeBulb.FinishStroke();
                    activeBulb = null;
                }
            }


            //DELETE
            if (Input.GetMouseButtonDown(1) && !activeBulb)
            {
                this.FindClosestBulb(this.HitPoint()).ClearStrokes();
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


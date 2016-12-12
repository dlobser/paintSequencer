using UnityEngine;
using System.Collections.Generic;

namespace HolojamEngine {
    public class BulbManagerCollision : MonoBehaviour {


        ////////////////////////////////////////
        //state machine enumerator
        ////////////////////////////////////////


        ////////////////////////////////////////
        //public collections & vars
        ////////////////////////////////////////
        public List<Bulb> bulbs = new List<Bulb>();
        public List<Stroke> strokePrefabs;
		public GameObject[] brushes;

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
				nb.transform.localEulerAngles = new Vector3 (nb.transform.localEulerAngles.x, (i / amount) * 360, 0);
				if (i % 4 == 0)
					nb.minScale = nb.minScale * 2;
//				nb.transform.position = new Vector3 ((1+i)*radius, 0, 0);
                nb.transform.position = new Vector3(
                Mathf.Sin(((float)i / (float)amount) * Mathf.PI * 2) * radius,
                height,
                Mathf.Cos(((float)i / (float)amount) * Mathf.PI * 2) * radius);
                bulbs.Add(nb);
            }

            currentStrokeIndex = 0;
        }

        // Update is called once per frame
        void Update() {
            //SWITCH STROKE PREFAB
   //         if (Input.GetKeyDown(KeyCode.S))
   //         {
   //             this.currentStrokeIndex = (currentStrokeIndex + 1 == strokePrefabs.Count ? 0 : currentStrokeIndex + 1);
   //         }
			//Vector3 hit = this.HitPoint();
			//this.FindClosestBulb (hit).Indicator.transform.localScale = Vector3.one *.5f;
            
            timer += Time.deltaTime;
            if (timer > bpm) {
                timer = 0;

				bulbs[bulbCounter++].Pulse();

                if (bulbCounter > bulbs.Count - 1) {
                    bulbCounter = 0;
                }
            }
        }

		public void ClearAllStrokes(){
			foreach(Bulb b in bulbs){
				b.ClearStrokes ();
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

        //public Vector3 HitPoint() {
        //    Vector3 vec = Vector3.zero;
        //    RaycastHit vHit = new RaycastHit();
        //    Ray vRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    if (Physics.Raycast(vRay, out vHit, 1000)) {
        //        vec = vHit.point;
        //    }
        //    Debug.Log(vec);
        //    return vec;
        //}

        ////////////////////////////////////////
        //abstract functions
        ////////////////////////////////////////
    }
}


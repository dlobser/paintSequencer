using UnityEngine;
using System.Collections;

namespace HolojamEngine {

	public class spring : nStroke {

		public float mass = .5f;
		public float damp =.1f;
		public Vector3 Position = Vector3.zero;
		public Vector3 Velocity = Vector3.one;
		public Vector3 Force = Vector3.one;




		public float maxMass=.15f;
		public float minMass=.075f;

		public GameObject follow;
		int avgAmount = 25;
		float[] ax;
		float[] ay;
		float[] az;

		void Start(){
			ax = new float[avgAmount];
			ay = new float[avgAmount];
			az = new float[avgAmount];
		}

		protected override void HandleFinish() {

			lerpTrail ();
			this.PushTrailToLine(Mathf.Max(0,currentPlaybackIndex-lineMaxVertexCount), currentPlaybackIndex);

			foreach (StrokeAnimation a in animations) {
				endTimer += fadeSpeed * Time.deltaTime;
				a.HandleFinish (endTimer);
			}

			UpdateSpring ();

			if (Force.magnitude < .001f) {
				endTimer = 0;
				this.SwitchToState(StrokeState.IDLE);

			}

		}


		public override void AddStrokePoint(StrokePoint newPoint) {
			Vector3 F = Vector3.zero;
			if(trail.Count>1)
				F = trail [trail.Count - 1].vec - trail [trail.Count - 2].vec;
//			elapse (Time.deltaTime);
			Vector3 avgForge = new Vector3 (average (ax, F.x), average (ay, F.y), average (az, F.z));
			Force = avgForge;
			this.trail.Add(newPoint);
			this.nTrail.Add (StrokeUtils.AddNoiseToVec(newPoint.vec));
			this.nLerp.Add (0);
		}

		void UpdateSpring(){
			Vector3 F = Vector3.zero - transform.position;
			float dist = Vector3.Distance (Vector3.zero, transform.position);
			mass = Mathf.Max (Mathf.Min(maxMass,map (dist, 0, 5, maxMass, minMass)),minMass);
			print (transform.position);
			Vector3 avgForge = new Vector3 (average (ax, F.x), average (ay, F.y), average (az, F.z));
			Force = F;//avgForge;
			transform.position = elapse (Time.deltaTime);
		}

		Vector3 elapse(float elapsed){
			print (Position);
			Velocity += (Force - Position) / mass * elapsed;
			Vector3 v = (Position + Velocity) * (1 - damp * elapsed);
//			print (v);
			Position = v;
			if (v.x > 0 || v.x < 0)
				return v;
			else
				return Vector3.one;

		}

		float average(float[] a, float n){
			for (int i = 0; i < a.Length-1; i++) {
				a[i] = a[i+1];
			}
			a[a.Length-1] = n;
			float avg =0f;
			for (int i = 0; i < a.Length; i++) {
				avg+=a[i];
			}
			avg/=a.Length;
			return avg;
			
		}
		float map(float s, float a1, float a2, float b1, float b2)
		{
			return b1 + (s-a1)*(b2-b1)/(a2-a1);
		}


	}
}
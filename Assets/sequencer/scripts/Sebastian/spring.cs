using UnityEngine;
using System.Collections;

namespace HolojamEngine {

	public class spring : nStroke {

		public float mass = .5f;
		public float damp =.1f;
		public Vector3 Position = Vector3.zero;
		public Vector3 Velocity = Vector3.zero;
		public Vector3 Force = Vector3.zero;
		private bool ReadyForExplosion = false;


		public Vector3 center = Vector3.zero;
		public float maxMass=.15f;
		public float minMass=.075f;

		public GameObject follow;
		int avgAmount = 25;
		float[] ax;
		float[] ay;
		float[] az;

		void Start(){
			registerAnimators (this.gameObject);
			ax = new float[avgAmount];
			ay = new float[avgAmount];
			az = new float[avgAmount];
		}

		protected override void HandleFinish() {

			lerpTrail ();
			this.PushTrailToLine(Mathf.Max(0,currentPlaybackIndex-lineMaxVertexCount), currentPlaybackIndex);



			UpdateSpring ();
//			AddStrokePoint (new StrokePoint (this.root.position, timer));
//			print(Force.magnitude);
			if (Force.magnitude < .1f || ReadyForExplosion) {
				ReadyForExplosion = true;
//				print (endTimer);
				endTimer += fadeSpeed * Time.deltaTime;
				foreach (StrokeAnimation a in animations) {
					a.HandleFinish (endTimer);
				}
				if (endTimer > 1) {
					endTimer = 0;
					this.SwitchToState (StrokeState.IDLE);
					ReadyForExplosion = false;
				}
			}

		}



		public void registerAnimators(GameObject g){

			StrokeAnimation[] playerScripts = g.GetComponents<StrokeAnimation>();
			if (playerScripts.Length > 0) {
				foreach (StrokeAnimation anims in playerScripts) {
					animations.Add (anims);
				}
			}
			for (int i = 0; i < g.transform.childCount; i++) {
				registerAnimators (g.transform.GetChild (i).gameObject);
			}
		}

		public override void AddStrokePoint(StrokePoint newPoint) {

			Force = Vector3.zero;
			Velocity = Vector3.zero;
//			print (Velocity);
//			Vector3 F = Vector3.zero;
			if(trail.Count>1)
				Velocity = trail [trail.Count - 1].vec - trail [trail.Count - 2].vec;
//			elapse (Time.deltaTime);
			Position = this.root.position;
//			Vector3 avgForge = new Vector3 (average (ax, F.x), average (ay, F.y), average (az, F.z));
//			Force = avgForge;
			this.trail.Add(newPoint);
			this.nTrail.Add (StrokeUtils.AddNoiseToVec(newPoint.vec));
			this.nLerp.Add (0);
		}

		void UpdateSpring(){

			Vector3 F = center - this.root.transform.position;
			Force = F;
			this.root.transform.position = elapse (Time.deltaTime);
		}

		protected override void HandlePlay() {

			this.currentPlaybackIndex++;

			if (this.currentPlaybackIndex > trail.Count - 1)
				this.currentPlaybackIndex = trail.Count - 1;

			if (this.currentPlaybackIndex >= trail.Count - 1) {
				this.SwitchToState(StrokeState.FINISH);
				return;
			}

			this.root.position = trail[currentPlaybackIndex].vec;
			this.PushTrailToLine(Mathf.Max(0,currentPlaybackIndex-lineMaxVertexCount), currentPlaybackIndex);
			lerpTrail ();
			float v = currentPlaybackIndex / (float)(trail.Count-1);
			foreach (StrokeAnimation a in animations) {
				a.HandlePlay(v);
			}

			Force = Vector3.zero;
			Velocity = Vector3.zero;
			//			print (Velocity);
			//			Vector3 F = Vector3.zero;
			if(trail.Count>1)
				Velocity = trail [trail.Count - 1].vec - trail [trail.Count - 2].vec;
			//			elapse (Time.deltaTime);
			Position = this.root.position;


			timer += Time.deltaTime;

		}



		Vector3 elapse(float elapsed){
//			print (Position);
			Velocity += (Force - Position) / mass * elapsed;
			Vector3 v = (Position + Velocity) * (1 - damp * elapsed);
//			print (v);
			Position = v;
			if (v.x > 0 || v.x < 0)
				return v;
			else
				return Vector3.zero;

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
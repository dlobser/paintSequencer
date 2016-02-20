using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class strokeUtils {

	public static Vector3[] listToArray(List<Vector3> l, int k, int j){
		Vector3[] returner;
		returner = new Vector3[j-k];
		for(int i = k ; i < j ; i++){
			returner [i] = l [i];
		}
		return returner;
	}

	public static Vector3[] averageVecArray(Vector3[] vecs){
		for (int i = 1; i < vecs.Length-1; i++) {
			Vector3 a = vecs [i - 1];
			Vector3 b = vecs [i + 1];
			vecs[i] = (a+b)/2;
		}
		return vecs;
	}

	public static Vector3[] averageVecArray(Vector3[] vecs, int head, float amt){
		for (int i = 1; i < head-1; i++) {
			Vector3 a = vecs [i - 1];
			Vector3 b = vecs [i + 1];
			vecs[i] = Vector3.Lerp( (a+b)/2 , vecs[i], amt);
		}
		return vecs;
	}

	public static Vector3[] noiseVecArray(Vector3[] vecs, int head, float amt){
		for (int i = 1; i < head-1; i++) {
			Vector3 a = vecs [i - 1]*3;
			Vector3 b = vecs [i + 1]*3;
			Vector3 n = vecs[i] + new Vector3 (
				            (Mathf.PI*Mathf.PerlinNoise (a.x, b.x+Time.time)),
				            (Mathf.PI*Mathf.PerlinNoise (a.y, b.y+Time.time)),
				            (Mathf.PI*Mathf.PerlinNoise (b.z, a.z+Time.time)));
			vecs [i] =  Vector3.Lerp( n , vecs[i], amt);
		}
		return vecs;
	}

	public static List<Vector3> noiseList(List<Vector3> vecs, float amt){
		for (int i = 1; i < vecs.Count-1; i++) {
			Vector3 a = vecs [i - 1]*10;
			Vector3 b = vecs [i + 1]*10;
			Vector3 n = vecs[i] + new Vector3 (
				Mathf.Cos (Mathf.PI*Mathf.PerlinNoise (a.x, b.x+Time.time)),
				Mathf.Cos (Mathf.PI*Mathf.PerlinNoise (a.y, b.y+Time.time)),
				Mathf.Cos (Mathf.PI*Mathf.PerlinNoise (b.z, a.z+Time.time)));
			vecs [i] =  Vector3.Lerp( n , vecs[i], amt);
		}
		return vecs;
	}


	public static void averageVecList(List<Vector3> vecs){
		for (int i = 1; i < vecs.Count-1; i++) {
			Vector3 a = vecs [i - 1];
			Vector3 b = vecs [i + 1];
			vecs[i] = (a+b+vecs[i])/3;
		}
	}

	public static Vector3[] getArrayPortion(Vector3[] vecs, int k, int j){
		Vector3[] returner;
		returner = new Vector3[j-k];
		for(int i = k ; i < j ; i++){
			returner [i] = vecs[i];
		}
		return returner;
	}

	public static void shiftArray(Vector3[] vecs, int head){
		for (int i = 0; i < head; i++) {
			vecs [i] = vecs [i + 1];
		}
	}
		
	public static void clearStrokes(List<GameObject> strokes){
		for(int i = 0 ; i < strokes.Count ; i++){
			GameObject.Destroy(strokes[i]);
		}
		strokes.Clear ();
	}

	public static void addToTrail(Vector3[] vecs, int head, Vector3 val){
		vecs [head] = val;
	}
}

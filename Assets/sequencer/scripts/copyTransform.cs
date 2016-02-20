using UnityEngine;
using System.Collections;

public class copyTransform : MonoBehaviour {

    public GameObject track;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = track.transform.position;
        transform.rotation = track.transform.rotation;
	}
}

using UnityEngine;
using System.Collections;


namespace HolojamEngine {
    public abstract class StrokeAnimation : MonoBehaviour {


        public abstract void OnStart();
        public abstract void OnPlay();
        public abstract void OnFinish();


        public abstract void HandlePlay(float t);
        public abstract void HandleFinish(float t);
    }
}


using UnityEngine;
using System.Collections;

namespace HolojamEngine {
    public class GlobalValuesAndSettings : Singleton<GlobalValuesAndSettings> {

        public int STROKE_MAX_AGE = 120;
        public int STROKE_MAX_VERTEX_COUNT = 100;
        public float STROKE_START_WIDTH = 0.01f;
        public float BULB_MIN_SCALE = 0.01f;
        public float BULB_MAX_SCALE = 0.05f;

        void Start() {
            Application.targetFrameRate = 60;
        }
    }
}


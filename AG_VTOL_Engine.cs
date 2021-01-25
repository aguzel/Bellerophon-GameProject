using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtlasStudio
{

    public class AG_VTOL_Engine : MonoBehaviour
    {
        #region Variables
        public float maxKW = 4000f;
        public float maxRPM = 24000f;
        public float powerDelay = 1f;
        public AnimationCurve powerCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));
        //public AG_VTOL_Engine_Carrier carrier = new AG_VTOL_Engine_Carrier();

        private Vector3 flatFwdEngine;
        private float forwardDotEngine;


        #region Properties

        private float currentKW;
        public float CurrentKW
        {
            get { return currentKW; }
        }
        private float currentRPM;
        public float CurrentRPM
        {
            get { return currentRPM; }
        }
        #endregion


        #endregion

        #region Builtin Methods
        void Start()
        {
            //carrier = GetComponent<AG_VTOL_Engine_Carrier>();
        }

        // Update is called once per frame
        void Update()
        {

        }
        #endregion

        #region Custom Methods 

        public Vector3 UpdateEngine(Rigidbody rb, float throttle)
        {
            //Calculate Power
            float finalThrottle = Mathf.Clamp01(throttle);
            finalThrottle = powerCurve.Evaluate(finalThrottle);

            //Calculate RPM's 
            float currentRPM = finalThrottle * maxRPM;
           

            //Calculate Final Forces
            float finalPower = finalThrottle * maxKW;
            Vector3 finalForce = rb.transform.forward * finalPower;
            return finalForce;
        }

        public float CalculateAngleEngine()
        {
            flatFwdEngine = transform.forward;
            flatFwdEngine.y = 0f;
            flatFwdEngine = flatFwdEngine.normalized;
            Debug.DrawRay(transform.position, flatFwdEngine, Color.blue);
            return forwardDotEngine = Vector3.Dot(transform.up, flatFwdEngine);           
            
        }
        public float EngineRPM(float throttle)
        {
            //Calculate Power
            float finalThrottle = Mathf.Clamp01(throttle);
            finalThrottle = powerCurve.Evaluate(finalThrottle);

            //Calculate RPM's 
            float currentRPM = finalThrottle * maxRPM;
            return currentRPM;

        }


        #endregion
    }
}

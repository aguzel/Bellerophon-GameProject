using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtlasStudio
{
    public class AG_VTOL_Characteristics : MonoBehaviour
    {
        #region Variables
        [Header("Forward Speed Properties")]
        public float forwardSpeed;
        public float mph;
        public float maxMPH = 400f;
        private float maxMPS;
        private float normalizedMPH;
       

        [Header("Lift Properties")]
        public float maxLiftForce = 50f;
        public float maxWingLiftForce = 200f;
        private float startDrag = 0.01f;
        private float startAngularDrag = 0.01f;
        public float dragFactor = 0.01f;
        // public Transform EngineCarrier;      
        public AG_VTOL_Engine engine = new AG_VTOL_Engine();
        [Space]

        [Header("Cyclic Properties")]
        public float cyclicForce = 1f;
        public float cyclicForceMultiplier = 1000f;

        [Header("Auto Level Properties")]
        public float autoLevelForce = 2f;

        private Vector3 flatFwd;
        private float forwardDot;
        private Vector3 flatRight;
        private float rightDot;

        [Header("Airplane Roll-Pitch-Yaw Properties")]
        public float pitchAngle;
        public float rollAngle;
        public float pitchSpeed = 2000f;
        public float rollSpeed = 2000f;
        public float yawSpeed = 2000f;

        #endregion

        #region Constants
        const float mpsToMph = 2.2369f;
        #endregion

        #region Builtin Methods
        // Start is called before the first frame update
        void Start()
        {
          
        }


        // Update is called once per frame
        void Update()
        {

        }
        #endregion

        #region Custom Methods
        public void UpdateCharacteristics(Rigidbody rb, AG_VTOL_Inputs input, AG_VTOL_Engine_Carrier carrier, AG_VTOL_Engine engine)
        {
            HandleLift(rb, input, carrier, engine);
            HandleCyclic(rb, input);
            // HandlePedals(rb, input);
            // HandleHover(rb, input);
            // HandlePitch(rb, input);
            HandleRoll(rb, input);
            HandleYaw(rb, input);

            CalculateDrag(rb);
            CalculateLift(rb);
            CalculateAngle();
            AutoLevel(rb);
            CalculateSpeed(rb, input);

        }

        protected virtual void HandleLift(Rigidbody rb, AG_VTOL_Inputs input, AG_VTOL_Engine_Carrier carrier, AG_VTOL_Engine engine)
        {
            //VTOL Reference
            Vector3 LiftForce = Vector3.zero;

            //VTOL Position Based Force
            LiftForce = Mathf.Clamp01(1 - engine.CalculateAngleEngine()) * transform.up * ((Physics.gravity.magnitude * rb.mass) + (input.Collective * maxLiftForce));

            //Motor Position Based Force.
            //LiftForce = carrier.transform.up * ((Physics.gravity.magnitude * rb.mass) + (input.Collective * maxLiftForce));

            rb.AddForce(LiftForce, ForceMode.Force);
            //Debug.Log(1 - engine.CalculateAngleEngine());


        }

        protected virtual void HandleCyclic(Rigidbody rb, AG_VTOL_Inputs input)
        {
            //Debug.Log("Handling Cyclic");
            float cyclicZForce = -input.Cyclic.x * cyclicForce;
            //rb.AddRelativeTorque(cyclicZForce * Vector3.forward, ForceMode.Acceleration);

            float cyclicXForce = input.Cyclic.y * cyclicForce;
            rb.AddRelativeTorque(cyclicXForce * Vector3.right, ForceMode.Acceleration);

            //Apply force based off the Dot Product Values
            Vector3 forwardVector = flatFwd * forwardDot;
            Vector3 rightVector = flatRight * rightDot;
            Vector3 finalCyclicDir = Vector3.ClampMagnitude(forwardVector + rightVector, 1f) * (cyclicForce * cyclicForceMultiplier);
            rb.AddForce(finalCyclicDir, ForceMode.Force);


        }

        protected virtual void HandlePedals(Rigidbody rb, AG_VTOL_Inputs input)
        {
            if (input.Pedals != 0f && input.Throttle < 0.01f)
            {
                //rb.AddTorque(Vector3.up * input.Pedals * 1.5f, ForceMode.Acceleration);
                //EngineCarrier.localRotation = Quaternion.Euler(controlInput * maxRot, 0f, 0f);
            }
        }

        protected virtual void CalculateAngle()
        {
            //Calculate the flat forward 
            flatFwd = transform.forward;
            flatFwd.y = 0f;
            flatFwd = flatFwd.normalized;
            // Debug.DrawRay(transform.position, flatFwd, Color.blue);

            //Calculate the flat right
            flatRight = transform.right;
            flatRight.y = 0f;
            flatRight = flatRight.normalized;
            // Debug.DrawRay(transform.position, flatRight, Color.red);

            //Calculate Angles 
            forwardDot = Vector3.Dot(transform.up, flatFwd);
            rightDot = Vector3.Dot(transform.up, flatRight);
            //Debug.Log(forwardDot + " fwd " +  rightDot + "right");
        }

        protected virtual void AutoLevel(Rigidbody rb)
        {
            float rightForce = -forwardDot * autoLevelForce;
            float forwardForce = rightDot * autoLevelForce;

            rb.AddRelativeTorque(Vector3.right * rightForce, ForceMode.Acceleration);
            rb.AddRelativeTorque(Vector3.forward * forwardForce, ForceMode.Acceleration);

        }

        protected virtual void HandleHover(Rigidbody rb, AG_VTOL_Inputs input)
        {
            if (input)
            {

            }
        }  // not used

        protected virtual void CalculateSpeed(Rigidbody rb, AG_VTOL_Inputs input)
        {
            maxMPS = maxMPH / mpsToMph;

            Vector3 localVelocity = transform.InverseTransformDirection(rb.velocity);
            forwardSpeed = Mathf.Max(0, localVelocity.z);
            forwardSpeed = Mathf.Clamp(forwardSpeed, 0f, maxMPS);

            mph = forwardSpeed * mpsToMph;
            mph = Mathf.Clamp(mph, 0f, maxMPH);
            normalizedMPH = Mathf.InverseLerp(0f, maxMPH, mph);
            //Debug.Log(mph);
        }

        protected virtual void HandlePitch(Rigidbody rb, AG_VTOL_Inputs input)
        {
            Vector3 flatForward = transform.forward;
            flatForward.y = 0f;
            pitchAngle = Vector3.Angle(transform.forward, flatForward);

            Vector3 pithcTorque = input.Cyclic.y * pitchSpeed * transform.right;
            rb.AddTorque(pithcTorque);

        }

        protected virtual void HandleRoll(Rigidbody rb, AG_VTOL_Inputs input)
        {
            Vector3 flatRight = transform.right;
            flatRight.y = 0f;
            rollAngle = Vector3.SignedAngle(transform.right, flatRight, transform.forward);

            Vector3 rollTorque = -input.Cyclic.x * rollSpeed * transform.forward;
            rb.AddTorque(rollTorque);

        }

        protected virtual void HandleYaw(Rigidbody rb, AG_VTOL_Inputs input)
        {
            Vector3 yawTorque = input.Pedals * yawSpeed * transform.up;
            rb.AddTorque(yawTorque);
        }

        protected virtual void CalculateLift(Rigidbody rb)
        {
            //Create the lift direction
            Vector3 liftDir = transform.up;
            float liftPower = normalizedMPH * maxWingLiftForce;

            //Apply to final lift force to rigidbody
            Vector3 finalLiftForce = liftDir * liftPower;
            rb.AddForce(finalLiftForce);

            //Debug.DrawRay(transform.position, finalLiftForce, Color.green);

        }

        protected virtual void CalculateDrag(Rigidbody rb)
        {
            float speedDrag = forwardSpeed * dragFactor;
            float finalDrag = startDrag + speedDrag;
            rb.drag = finalDrag;
            rb.angularDrag = startAngularDrag * forwardSpeed;
        }
        #endregion
    }
}

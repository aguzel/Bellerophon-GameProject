using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AtlasStudio

{
    [RequireComponent(typeof(PlayerInput))]

    public class AG_VTOL_Inputs : MonoBehaviour
    {
        #region Variables

        private Vector2 cyclic;
        private float pedals;
        private float throttle;
        private float collective;
        

        public float stickyThrottle;
        public float throttleSpeed = 0.1f;
        public float stickyCollective;
        public float collectiveSpeed = 0.1f;

        public Vector2 Cyclic { get => cyclic; }
        public float Pedals { get => pedals; }
        public float Throttle { get => throttle; }
        public float Collective { get => collective; }
        public float StickyThrottle { get => OnStickThrottle(); }
        public float StickyCollective { get => OnStickCollective(); }
        #endregion

        #region Builtin Methods
        void Update()
        {

        }
        #endregion

        #region Input Methods

        private void OnCyclic(InputValue value)
        {
            cyclic = value.Get<Vector2>();
        }

        private void OnPedals(InputValue value)
        {
            pedals = value.Get<float>();
        }

        private void OnThrottle(InputValue value)
        {
            throttle = value.Get<float>();
        }

        private void OnCollective(InputValue value)
        {
            collective = value.Get<float>();
        }

        public float OnStickThrottle()
        {
            stickyThrottle = stickyThrottle + (Throttle * Time.deltaTime * throttleSpeed);
            stickyThrottle = Mathf.Clamp01(stickyThrottle);
            //Debug.Log(stickyThrottle);
            return stickyThrottle;           
        }

        public float OnStickCollective()
        {
            stickyCollective = stickyCollective + (Collective * Time.deltaTime * collectiveSpeed);
            stickyCollective = Mathf.Clamp01(stickyCollective);
            //Debug.Log(stickyThrottle);
            return stickyCollective;
        }
        #endregion

    }
}

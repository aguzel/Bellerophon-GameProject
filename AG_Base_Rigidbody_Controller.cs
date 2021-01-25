using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtlasStudio
{
    [RequireComponent(typeof(Rigidbody))]

    public class AG_Base_Rigidbody_Controller : MonoBehaviour
    {

        #region Variables

        [Header("Rigidbody Properties")]
        public Transform cog;
        protected Rigidbody rb;
               
        #endregion

        #region Builtin Methods
        public virtual void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.centerOfMass = cog.localPosition;            
        }
                
        void FixedUpdate()
        {
            if(!rb)
            {
                return;
            }
            HandlePhysics();
        }
        #endregion

        #region Custom Methods
        protected virtual void HandlePhysics() { }
        #endregion
    }
}


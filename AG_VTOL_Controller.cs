using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace AtlasStudio
{
    [RequireComponent(typeof(AG_VTOL_Inputs))]

    public class AG_VTOL_Controller : AG_Base_Rigidbody_Controller
    {

        #region Variables
        [Header("VTOL Properties")]
        public AG_VTOL_Engine engine = new AG_VTOL_Engine();
        public AG_VTOL_Engine_Carrier carrier = new AG_VTOL_Engine_Carrier();
        
        private AG_VTOL_Inputs input;
        private AG_VTOL_Characteristics characteristics;
        #endregion

        #region Builtin Methods
        public override void Start()
        {
            base.Start();

            characteristics = GetComponent<AG_VTOL_Characteristics>();            
            
        }
        #endregion

        #region Custom Methods
        protected override void HandlePhysics()
        {
            input = GetComponent<AG_VTOL_Inputs>();

            if (input)
            {
                HandleEngines();
                HandleCarrierRotation();
                HandleCharacterisctics();
            }                        
        }
        #endregion

        #region VTOL Control Methods        
        protected virtual void HandleEngines()
        {
           
            if(engine)
            {
                rb.AddForce(engine.UpdateEngine(rb, input.StickyThrottle));
                //transform.localRotation = Quaternion.Euler(input.StickyThrottle * 10f, 0f, 0f);
                engine.CalculateAngleEngine();
            }
                                          
        }

        protected virtual void HandleCharacterisctics()
        {
            if(characteristics)
            {
                characteristics.UpdateCharacteristics(rb, input, carrier, engine);             
            }
        }

        protected virtual void HandleCarrierRotation()
        {
           if(carrier)
            {
                carrier.UpdateCarrierRotation(input.StickyThrottle);
                //Debug.Log("Handle Carrier");
                
            }

        }


        #endregion


    }
}


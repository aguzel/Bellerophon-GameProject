using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AtlasStudio
{

    public class AG_VTOL_Engine_Carrier : MonoBehaviour
    {
        #region Variables
        [Header("Main Carrier Paramaters")]
        public Transform EngineCarrier;
        public float maxRot = 60f;
        #endregion

        #region Builtin Methods
        void Start()
        {

        }
        #endregion 


        #region Custom Methods
        public void UpdateCarrierRotation(float controlInput)
        {
            if (EngineCarrier)
            {
                EngineCarrier.localRotation = Quaternion.Euler(controlInput * maxRot, 0f, 0f);              
            }            

        }
        #endregion
    }
}


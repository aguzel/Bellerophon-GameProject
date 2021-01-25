using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

namespace AtlasStudio
{

    public class AG_Serial_Controller : MonoBehaviour
    {

        [Header("Engines")]
        public AG_VTOL_Engine engine = new AG_VTOL_Engine();
        [Header("Input - GameObject")]
        public AG_VTOL_Inputs input = new AG_VTOL_Inputs();

        #region Serial Port
        SerialPort serial = new SerialPort("COM3", 9600);
        #endregion

        #region Builtin Methods
        // Start is called before the first frame update
        void Awake()
        {
            serial.Open();
            //StartCoroutine(WriteSerialDataIEnum());
            InvokeRepeating("ClosePortClearData", 5f, 1.5f);
        }


        // Update is called once per frame

        void FixedUpdate()
        {
            if (engine)
            {
                float motorSpeed = engine.EngineRPM(input.StickyThrottle);
                float motorLoad = motorSpeed / engine.maxRPM;

                float motorPWM = Mathf.Lerp(70, 140, motorLoad);
                float motoPWMf = Mathf.Lerp(70, 140, motorLoad);
                motorPWM = Mathf.Round(motorPWM);

                string signalPWM = motorPWM.ToString();
                //Debug.Log(signalPWM);
                Debug.Log(motoPWMf + " = PWM");

                WriteDataToSerialPort(signalPWM);

            }
        }
        #region Standard Method

        void WriteDataToSerialPort(string PWM)
        {
            serial.WriteLine(PWM);
            serial.BaseStream.Flush();
            //serial.WriteTimeout = 5;
            //serial.Close();
            //serial.Open();

            //Debug.Log("Data is being tranferred!");        

        }
        void ClosePortClearData()
        {
            serial.Close();
            serial.Open();
        }

        #endregion
        #endregion
    }

}

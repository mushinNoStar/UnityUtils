using UnityEngine;
using System.Collections;
namespace Tools
{
    public delegate void voidMehtod();
    public class TimeManager : MonoBehaviour
    {
        /// <summary>
        /// Subscribe to this to be notified once every tick.
        /// </summary>
        public static event voidMehtod OnTick;
        private float f = 1;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            f -= Time.deltaTime;
            if (f < 0)
            {
                if (OnTick != null)
                    OnTick();
                f = 1;
            }
        }
    }

}
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
        private static event voidMehtod OnTick;
        private static event voidMehtod OnTick1;
        private static event voidMehtod OnTick2;
        private static event voidMehtod OnTick3;
        private static event voidMehtod OnTick4;
        private static event voidMehtod OnTick5;

        private static float f = 1;
        private static float f2 = 0.75f;
        private static float f3 = 0.50f;
        private static float f4 = 0.25f;
        private static float f5 = 0.12f;
        private static float f6 = 0.62f;


        static int t = 0;
        // Use this for initialization
        void Start()
        {

        }

        public static void subscribe(voidMehtod m)
        {
            switch (t)
            {
                case (0):
                    OnTick += m;
                    break;
                case (1):
                    OnTick1 += m;
                    break;
                case (2):
                    OnTick2 += m;
                    break;
                case (3):
                    OnTick3 += m;
                    break;
                case (4):
                    OnTick3 += m;
                    break;
                case (5):
                    OnTick3 += m;
                    break;

            }
            t++;
            if (t == 1)
                t = 0;
        }

        // Update is called once per frame
        void Update()
        {
            f -= Time.deltaTime;
            f2 -= Time.deltaTime;
            f3 -= Time.deltaTime;
            f4 -= Time.deltaTime;
            f5 -= Time.deltaTime;
            f6 -= Time.deltaTime;
            if (f < 0)
            {
                if (OnTick != null)
                    OnTick();
                f = 1;
            }
            if (f2 < 0)
            {
                if (OnTick1 != null)
                    OnTick1();
                f2 = 1;
            }
            if (f3 < 0)
            {
                if (OnTick2 != null)
                    OnTick2();
                f3 = 1;
            }
            if (f4 < 0)
            {
                if (OnTick3 != null)
                    OnTick3();
                f4 = 1;
            }
            if (f5 < 0)
            {
                if (OnTick4 != null)
                    OnTick4();
                f5 = 1;
            }
            if (f6 < 0)
            {
                if (OnTick5 != null)
                    OnTick5();
                f6 = 1;
            }
        }
    }

}
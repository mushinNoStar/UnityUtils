using UnityEngine;
using System.Collections.ObjectModel;
using Plane.Internal;

public delegate void VoidMethod();
public delegate void IntMethod(int number);

namespace Plane
{
    namespace Internal
    {
        public class PlaneBehaviour : MonoBehaviour
        {
            public event IntMethod OnClicked;
            public event VoidMethod OnOver;

            void Start()
            {

            }

            void Update()
            {

            }

            public static PlaneBehaviour loadOne()
            {
                GameObject gm = Instantiate(Resources.Load<GameObject>("Plane"));
                return gm.GetComponent<PlaneBehaviour>();
            }

            public void setVertices(ReadOnlyCollection<IVertex> vertices)
            {
                Debug.Log("Not implemented yet");
            }

            public void remove()
            {
                Destroy(gameObject);
            }

            public void OnMouseDown()
            {
                if (Input.GetMouseButtonDown(0) && OnClicked != null)
                    OnClicked(0);

                if (Input.GetMouseButtonDown(2) && OnClicked != null)
                    OnClicked(1);
            }

            public void OnMouseOver()
            {
                if (OnOver != null)
                    OnMouseOver();
            }
        }
    }
}
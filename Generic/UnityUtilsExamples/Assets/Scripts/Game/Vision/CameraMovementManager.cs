using UnityEngine;
using System.Collections.Generic;

public class CameraMovementManager : MonoBehaviour
{

    public List<KeyCode> keys;
    public List<Vector3> direction;
    public static Vector3 target;
    public static int locked = 0;

    void Start()
    {
        Camera.main.transparencySortMode = TransparencySortMode.Orthographic;
        target = transform.position;
    }

    void Update()
    {
        if (locked > 0)
            return;
        foreach (KeyCode code in keys)
            if (Input.GetKey(code))
                target += direction[keys.IndexOf(code)] * Time.deltaTime * -1 * transform.position.z;

        var d = Input.GetAxis("Mouse ScrollWheel");
        if (d < 0 && target.z < -300)
            target = new Vector3(target.x, target.y, -300);
        if (d > 0 && target.z > -0.2f)
            target = new Vector3(target.x, target.y, -1);

        target += new Vector3(0, 0, d * Mathf.Abs(transform.position.z));
        transform.position = Vector3.Lerp(transform.position, target, 3 * Time.deltaTime);
    }
}

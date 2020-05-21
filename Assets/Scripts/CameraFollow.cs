using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    Vector3 defaultPos = new Vector3(0, 153, -18);
    public bool following;

    void Update()
    {
        if (following)
        {
            if (target != null)
                transform.position = new Vector3(target.position.x, 35, target.position.z - 1);
            else
                transform.position = defaultPos;
        }
    }
}

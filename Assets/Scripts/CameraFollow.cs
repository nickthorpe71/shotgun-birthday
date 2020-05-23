using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    Vector3 defaultPos = new Vector3(0, 180, 0);
    public bool following;

    void Update()
    {
        if (following)
        {
            if (target != null)
                transform.position = new Vector3(target.position.x, 35, target.position.z);
            else
                transform.position = defaultPos;
        }
    }
}

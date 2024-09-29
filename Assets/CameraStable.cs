using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStable : MonoBehaviour
{

    public Transform Car;

    float rotX, rotY, rotZ;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rotX = Car.eulerAngles.x;
        rotY = Car.eulerAngles.y;
        rotZ = Car.eulerAngles.z;

        transform.eulerAngles = new Vector3(rotX - rotX, rotY, rotZ - rotZ);

    }
}

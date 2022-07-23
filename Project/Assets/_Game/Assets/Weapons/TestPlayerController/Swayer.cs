using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swayer : MonoBehaviour
{
    public float swaySmoothing;
    public float swayingAmount;

    private void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * swayingAmount;
        float mouseY = Input.GetAxisRaw("Mouse Y") * swayingAmount;




        Quaternion rotationX = Quaternion.AngleAxis(mouseY, Vector3.left);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);


        Quaternion targetRotation = rotationX * rotationY;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, swaySmoothing * Time.deltaTime);
    }

}

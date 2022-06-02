using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class EscapeRoom : MonoBehaviour
{
    public Slider VelocitySlider;
    public Slider ScaleSlider;
    public Slider RotationSlider;
    public CharacterController CharacterController;
    public GameObject DoorObject;


    private void Update()
    {
        transform.eulerAngles = new Vector3(0, RotationSlider.value);
        transform.localScale = new Vector3(ScaleSlider.value, ScaleSlider.value, ScaleSlider.value);
        CharacterController.Move(transform.forward * VelocitySlider.value * Time.deltaTime);
        
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {

            RaycastHit hitInfo = new RaycastHit();
            Vector3 coor = Mouse.current.position.ReadValue();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(coor), out hitInfo);
            if (hit) 
            {
                Debug.Log("Hit " + hitInfo.transform.gameObject.name);
                if (hitInfo.transform.gameObject.tag == "Enemy")
                {
                    Debug.Log ("DOOR!!!!!");
                    DoorObject = hitInfo.transform.gameObject;
                }
            } 

        } 
    }
}

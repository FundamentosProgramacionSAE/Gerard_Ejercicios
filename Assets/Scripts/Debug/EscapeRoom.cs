using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class EscapeRoom : MonoBehaviour
{
    
    public static EscapeRoom Instance { get; private set; }
    public Slider VelocitySlider;
    public Slider ScaleSlider;
    public Slider RotationSlider;
    public Button OpenCloseButton;
    public CharacterController CharacterController;
    public DoorSystem DoorObject;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        OpenCloseButton.onClick.AddListener(OpenCloseDoor);
        OpenCloseButton.gameObject.SetActive(false);
    }

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
                    DoorObject = hitInfo.transform.GetComponent<DoorSystem>();
                    DoorObject.InitializeDoor();
                }
                else
                {
                    OpenCloseButton.gameObject.SetActive(false);
                }
            }

        } 
    }

    public void OpenCloseDoor()
    {
        if(DoorObject == null) return;
        DoorSystem doorSystem = DoorObject;
        if (doorSystem.IsOpen)
        {
            doorSystem.IsOpen = false;
            doorSystem.gameObject.transform.position -= new Vector3(0, 4, 0);
        }
        else
        {
            doorSystem.gameObject.transform.position += new Vector3(0, 4, 0);
            doorSystem.IsOpen = true;
        }
        doorSystem.InitializeDoor();
    }
}


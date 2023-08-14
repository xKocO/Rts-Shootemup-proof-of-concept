using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraScript : MonoBehaviour {

    float targetRatio = 16f / 9f, camX, camY; //Aspect ratio
    public float velocidadX,velocidadY,zoomMin,zoomMax,zoomSens,MIN_X,MAX_X,MIN_Z,MAX_Z,MIN_Y,MAX_Y, boundary;
    Camera cam;
    private int screenWidth, screenHeigth;

    void Start()
    {
        cam = GetComponent<Camera>();
        cam.aspect = targetRatio;
        screenHeigth = Screen.height;
        screenWidth = Screen.width;
    }

    void Update() {

        //if (Input.GetMouseButton(0)) {
        //    camX = Input.GetAxis("Mouse X") * velocidadX * Time.deltaTime;
        //    camY = Input.GetAxis("Mouse Y") * velocidadY * Time.deltaTime;
        //    transform.Translate(-camX, -camY, 0);
        //}

        Vector3 camPos = transform.position;

        if (Input.mousePosition.x > screenWidth - boundary) {
            camPos.x += velocidadX * Time.deltaTime;
            transform.position = camPos;
        }

        if (Input.mousePosition.x < boundary)
        {
            camPos.x -= velocidadX * Time.deltaTime;
            transform.position = camPos;
        }

        if (Input.mousePosition.y > screenHeigth - boundary)
        {
            camPos.z += velocidadY * Time.deltaTime;
            transform.position = camPos;
        }

        if (Input.mousePosition.y < boundary)
        {
            camPos.z -= velocidadY * Time.deltaTime;
            transform.position = camPos;
        }

        float fov = cam.fieldOfView;
        fov -= Input.GetAxis("Mouse ScrollWheel") * zoomSens;
        fov = Mathf.Clamp(fov, zoomMin, zoomMax);
        cam.fieldOfView = fov;

        transform.position = new Vector3(
        Mathf.Clamp(transform.position.x, MIN_X, MAX_X),
        Mathf.Clamp(transform.position.y, MIN_Y, MAX_Y),
        Mathf.Clamp(transform.position.z, MIN_Z, MAX_Z));

        //Debug.Log(transform.position.x);
    }
}

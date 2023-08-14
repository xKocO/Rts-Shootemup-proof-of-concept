using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneShooter : MonoBehaviour {

    public GameObject control,press;

    void Update() {

        if (Input.GetKeyDown(KeyCode.Return) && press.activeSelf) {
            control.GetComponent<ControlScript>().CambioDeCamara();
            press.SetActive(false);
            loadScene.GanasteTexto.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
            control.GetComponent<ControlScript>().parar();
            ControlScript.rts = false;
            loadScene.previo = true;
            press.SetActive(true);
        }
    }
}

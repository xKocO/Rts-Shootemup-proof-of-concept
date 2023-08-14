using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyshot : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "pared")
        {
            gameObject.SetActive(false);
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        if (other.tag == "shooterPlayer") {
            if (!PlayerScript.dead && !loadScene.GanasteTexto.activeSelf) {
                other.gameObject.GetComponent<PlayerScript>().muerte();
                gameObject.SetActive(false);
                gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                loadScene.GameOver();
            }
        }
    }
}

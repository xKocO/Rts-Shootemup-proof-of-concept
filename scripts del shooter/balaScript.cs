using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class balaScript : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "pared")
        {
            gameObject.SetActive(false);
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        if (other.tag == "enemy")
        {
            other.gameObject.GetComponent<gunner>().StartCoroutine("morir");
            gameObject.SetActive(false);
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        if (other.tag == "enemy1")
        {
            other.gameObject.GetComponent<KnifeScript>().StartCoroutine("morir");
            gameObject.SetActive(false);
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        if (other.tag == "Goliath") {
            gameObject.SetActive(false);
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            other.GetComponent<GoliathScript>().RestarVida();
        }
    }
}

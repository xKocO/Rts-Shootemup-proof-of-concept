using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dangerScript : MonoBehaviour {

    public GameObject control,fill;
    float charge = 0;
    public float chargeSpeed;

    void Update() {
        if (charge >= 100)
        {
            ControlScript.Danger -= 5; 
            gameObject.SetActive(false);
            charge = 0;
        }

        fill.transform.localScale = new Vector3(charge/100,1,charge/100);
        //Debug.Log("charge:  " + charge);
    }

    void OnTriggerStay(Collider other)
    {
        if (control.GetComponent<ControlScript>().jugando) { 
            if (other.gameObject.tag == "Player")
        {
            charge += chargeSpeed * Time.deltaTime;
        }
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunner : MonoBehaviour {

    public GameObject cañon;
    GameObject player;
    GameObject[] bala;
    Rigidbody rb,rbPropio;
    public float fuerza,intervaloDeDisparo,velocidadMovimiento,maximaVelocidadDeMovimiento;
    int xMov, zMov;
    Animator animator;
    bool dead = false;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("shooterPlayer");
        bala = loadScene.bala;
        rbPropio = GetComponent<Rigidbody>();
        StartCoroutine("disparo");
	}
	
	// Update is called once per frame
	void Update () {
        if(!PlayerScript.dead && !dead)
        transform.LookAt(player.transform);
        animator.SetFloat("velocityX", rbPropio.velocity.x);
        animator.SetFloat("velocityZ", rbPropio.velocity.z);
    }

    void FixedUpdate() {
        rbPropio.velocity = Vector3.ClampMagnitude(rbPropio.velocity, maximaVelocidadDeMovimiento);
        if(!PlayerScript.dead && !dead)
        rbPropio.AddForce(new Vector3(xMov, 0, zMov) * velocidadMovimiento);
    }

    IEnumerator disparo() {
        while (gameObject.activeSelf && !PlayerScript.dead && !dead) {
            xMov = Random.Range(-1, 2);
            zMov = Random.Range(-1, 2);
            yield return new WaitForSeconds(intervaloDeDisparo);
            foreach (GameObject bala in bala) {
                if (!bala.activeSelf && !dead) {
                    bala.transform.position = cañon.transform.position;
                    bala.SetActive(true);
                    rb = bala.GetComponent<Rigidbody>();
                    rb.AddForce(transform.forward * fuerza, ForceMode.Impulse);
                    break;
                }
            }
            
        }
    }

    void OnCollisionStay(Collision other) {
        if(other.gameObject.tag == "pared")
        {
            xMov = Random.Range(-1, 2);
            zMov = Random.Range(-1, 2);
            //Debug.Log("toco pared");
        }
    }

    public IEnumerator morir()
    {
        animator.Play("dead");
        dead = true;
        rbPropio.useGravity = false;
        GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
        loadScene.comprobar();
    }

}

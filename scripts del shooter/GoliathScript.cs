using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoliathScript : MonoBehaviour {

    public GameObject front, back, left, right;
    GameObject[] bala;
    Rigidbody rb, rbPropio;
    public float fuerza, intervaloDeDisparo, velocidadMovimiento, maximaVelocidadDeMovimiento,velocidadDeRotacion,intervaloDeGiro;
    int yRot;
    public int vida;
    Vector3 inverso;
    bool girar,dead = false;
    Animator animator;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        bala = loadScene.bala;      
        rbPropio = GetComponent<Rigidbody>();
        StartCoroutine("disparo");
        StartCoroutine("giro");
        inverso = transform.forward;
    }

    void Update() {


        if (!dead)
        {
            if (girar) { transform.Rotate(new Vector3(0, yRot, 0) * velocidadDeRotacion * Time.deltaTime); }
            else
            {
                transform.LookAt(transform.position + rbPropio.velocity);
            }
        }

    }

    void FixedUpdate()
    {
        if(!dead)
        rbPropio.AddForce(inverso * velocidadMovimiento);
        rbPropio.velocity = Vector3.ClampMagnitude(rbPropio.velocity, maximaVelocidadDeMovimiento);
    }
    

    IEnumerator disparo()
    {
        while (gameObject.activeSelf && !PlayerScript.dead && !dead)
        {
            int aux = 0;
            yield return new WaitForSeconds(intervaloDeDisparo);
            foreach (GameObject bala in bala)
            {
                if (!bala.activeSelf)
                {
                    if (dead) { break; }
                    bala.SetActive(true);
                    rb = bala.GetComponent<Rigidbody>();
                    switch (aux) {
                        case 0:
                            bala.transform.position = front.transform.position;
                            rb.AddForce(transform.forward * fuerza, ForceMode.Impulse);
                                break;
                        case 1:
                            bala.transform.position = back.transform.position;
                            rb.AddForce(-transform.forward * fuerza, ForceMode.Impulse);
                            break;

                        case 2:
                            bala.transform.position = left.transform.position;
                            rb.AddForce(-transform.right * fuerza, ForceMode.Impulse);
                            break;

                        case 3:
                            bala.transform.position = right.transform.position;
                            rb.AddForce(transform.right * fuerza, ForceMode.Impulse);
                            break;
                    }
                    aux++;
                }
                if (aux == 4) { break; }
            }
        }
    }

    IEnumerator giro() {
        while (gameObject.activeSelf && !PlayerScript.dead && !dead) {
            while (yRot == 0) { yRot = Random.Range(-1, 2); }
            yield return new WaitForSeconds(intervaloDeGiro);
            girar = true;
            yield return new WaitForSeconds(intervaloDeGiro);
            girar = false;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "pared")
        {
            inverso = -inverso;
        }
    }

    public void RestarVida() {
        vida--;
        if (vida == 0) { StartCoroutine("morir"); }
    }

    IEnumerator morir()
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

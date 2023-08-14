using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeScript : MonoBehaviour {

    GameObject player;
    public GameObject knife;
    Rigidbody rb;
    public float velocidad,maxSpeed;
    bool dead = false;
    Animator animator;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("shooterPlayer");
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        if (player.activeSelf && !dead)
        {
            transform.LookAt(player.transform);
            animator.SetFloat("velocityX", rb.velocity.x);
            animator.SetFloat("velocityZ", rb.velocity.z);
        }
	}

    void FixedUpdate() {
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        if (player.activeSelf && !dead)
        rb.AddForce(Vector3.Normalize(transform.forward) * velocidad);
    }

    public IEnumerator morir() {
        animator.Play("dead");
        dead = true;
        rb.useGravity = false;
        knife.GetComponent<BoxCollider>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
        loadScene.comprobar();
    }
}

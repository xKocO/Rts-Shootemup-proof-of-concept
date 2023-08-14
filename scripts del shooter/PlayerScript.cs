using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

    public float velocidad, maxSpeed,balaSpeed;
    public Rigidbody rb;
    public Camera cam;
    public static GameObject[] bala;
    GameObject poolDeBalas;
    public GameObject cañon;
    Ray mouseray,ray;
    RaycastHit mouseinfo,hit;
    Vector3 normal;
    Animator animator;
    public static bool dead = false;

    void Start() {
        animator = GetComponent<Animator>();
        poolDeBalas = new GameObject("PoolDeBalas");
        bala = new GameObject[15];
        cargarPool();
        gameObject.SetActive(false);
    }

    void Update() {
        if (!dead) { 

        animator.SetFloat("velocityX", rb.velocity.x);
        animator.SetFloat("velocityZ", rb.velocity.z);
        if (Input.GetMouseButtonDown(0)) {
            shoot();
        }

        }
    }

    void FixedUpdate() {
        if (!dead) { 

        mouseray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(mouseray, out mouseinfo))
        {
            normal = new Vector3(mouseinfo.point.x,transform.position.y, mouseinfo.point.z);
            transform.LookAt(normal);
        }
           

        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);

        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(Vector3.left * velocidad);
        }

        if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(Vector3.back * velocidad);
        }

        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(Vector3.right * velocidad);
        }

        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(Vector3.forward * velocidad);
        }

        }
    }

    void OnCollisionEnter(Collision other) {
        if (other.collider.gameObject.CompareTag("knife") && !dead || other.collider.gameObject.CompareTag("Goliath") && !dead ) {
            muerte();
            loadScene.GameOver();
        } 
    }

    void cargarPool() {
        for (int x = 0; x <= 14; x++) {
            bala[x] = Instantiate(Resources.Load("Balas1", typeof(GameObject)), cañon.transform.position, Quaternion.identity,poolDeBalas.transform) as GameObject;
            bala[x].SetActive(false);
        }
    }

    void shoot() {
        foreach (GameObject bala in bala) {
            if (!bala.activeSelf) {
                ray = cam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    bala.SetActive(true);
                    bala.transform.position = cañon.transform.position;
                    bala.GetComponent<Rigidbody>().AddForce(-Vector3.Normalize(transform.position - hit.point) * balaSpeed, ForceMode.Impulse);
                    break;
                }
            }
        }
    }

    public static void reiniciarPool() {
        foreach (GameObject bala in bala) {
            bala.SetActive(false);
            bala.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    public void muerte() {
        animator.Play("dead");
        dead = true;              
    }
}

using UnityEngine;
using UnityEngine.AI;

public class playerController : MonoBehaviour {

    public Camera cam;
    public NavMeshAgent agent;
    [SerializeField]
    public bool activo;
    public GameObject control,auto;


    void Start() {
 
    }

    // Update is called once per frame
    void Update() {
        if (control.GetComponent<ControlScript>().jugando)
        {
            agent.isStopped = false;
            if (Input.GetMouseButtonDown(1))
            {
                if (activo)
                {
                    Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit))
                    {
                        agent.SetDestination(hit.point);
                        //Debug.Log("mover");
                    }
                }
            }



        }
        else if (control.GetComponent<ControlScript>().jugando == false) { agent.isStopped = true; }
    }

    void OnMouseDown() {

        if (control.GetComponent<ControlScript>().jugando) { 

             Material mat;
             control.GetComponent<ControlScript>().DesactivarAuto();
             activo = true;
             mat = auto.GetComponent<MeshRenderer>().material;
             mat.EnableKeyword("_EMISSION");
            //Debug.Log("active");
        }

    }
    
}

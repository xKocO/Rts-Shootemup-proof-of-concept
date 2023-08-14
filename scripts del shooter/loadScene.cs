using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadScene : MonoBehaviour {
    
    public GameObject player,pausa,control, goodjobTxt;
    public static GameObject GanasteTexto, PERDISTETEXTO;
    public static bool ganasteGil ,perdistegil ,previo;
    public static GameObject[] enemigos,bala;
    public static int cantidadEnemigos;
    List<GameObject> tags;
    AsyncOperation carga;
    int aux;
    bool start;

    // Use this for initialization
    void Start()
    {
        ganasteGil = false;
        perdistegil = false;
        previo = false;
        start = true;
        tags = new List<GameObject>();
        Invoke("cargaEnemigos", 0.1f);
        enemigos = tags.ToArray();
        bala = GameObject.FindGameObjectsWithTag("EnemyShot");
        DesactivarPool();
        GanasteTexto = GameObject.FindGameObjectWithTag("winText");
        PERDISTETEXTO = GameObject.FindGameObjectWithTag("loseTexr");
        PERDISTETEXTO.SetActive(false);
        GanasteTexto.SetActive(false);
        StartCoroutine("cargar");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && GanasteTexto.activeSelf && Time.timeScale == 1 && !ControlScript.rts ||
            Input.GetKeyDown(KeyCode.Space) && goodjobTxt.activeSelf && Time.timeScale == 1 && !ControlScript.rts) {

            if (previo)
            {
                PlayerScript.dead = false;
                player.SetActive(true);
                GanasteTexto.SetActive(false);
                carga.allowSceneActivation = true;
                Invoke("cargaEnemigos", 0.1f);
                previo = false;
            }
            else {
                PlayerScript.reiniciarPool();
                DesactivarPool();
                player.transform.localPosition = new Vector3(0, 0.5f, 0);
                player.SetActive(false);
                control.GetComponent<ControlScript>().CambioDeCamara();
                control.GetComponent<ControlScript>().empezar();
                GanasteTexto.SetActive(false);
                goodjobTxt.SetActive(false);
                ControlScript.rts = true;
            }

        } else if (Input.GetKeyDown(KeyCode.Space) && PERDISTETEXTO.activeSelf && Time.timeScale == 1 && !ControlScript.rts) {
            StartCoroutine("cargar");
            PERDISTETEXTO.SetActive(false);
            GanasteTexto.SetActive(true);
    }

        if (ganasteGil || perdistegil)
        {
            mostrarUI();
            ganasteGil = false;
        }

        if (Input.GetKeyDown(KeyCode.P) && !ControlScript.rts) {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
                pausa.SetActive(true);
            } else{
                Time.timeScale = 1;
                pausa.SetActive(false);
            }
        }

    }

    IEnumerator cargar() {
        if (!start)
        {
            Scene hey = SceneManager.GetSceneByBuildIndex(aux);
            if (hey.isLoaded) { SceneManager.UnloadSceneAsync(hey); }
        }
        if (start) { start = false; }
        aux = Random.Range(2, 10);    
        carga = SceneManager.LoadSceneAsync(aux, LoadSceneMode.Additive);
        carga.allowSceneActivation = false;

        while (carga.progress <= 0.89f)
        {
            yield return null;
        }    
    }

    public static void comprobar() {
        cantidadEnemigos = 0;
        foreach (GameObject ene in enemigos) {
            if (ene.activeSelf)
                cantidadEnemigos++;
        }
        //Debug.Log("enemigos:  " + cantidadEnemigos);
        if (cantidadEnemigos == 0 && !PERDISTETEXTO.activeSelf) { ganasteGil = true; }
    }

    public static void GameOver() {
        perdistegil = true; 
    }

    void cargaEnemigos()
    {
        tags.Clear();
        tags.AddRange(GameObject.FindGameObjectsWithTag("enemy"));
        tags.AddRange(GameObject.FindGameObjectsWithTag("Goliath"));
        tags.AddRange(GameObject.FindGameObjectsWithTag("enemy1"));
        enemigos = tags.ToArray();
    }

    void mostrarUI() {

        if (perdistegil)
        {
            PERDISTETEXTO.SetActive(true);
            perdistegil = false;
            ControlScript.Danger += 20;
        }
        else {
            if (!PERDISTETEXTO.activeSelf) {
                goodjobTxt.SetActive(true);
                StartCoroutine("cargar");
                ControlScript.Danger -= 20;
            }
        }
    }

    void DesactivarPool()
    {
        foreach (GameObject bala in bala)
        {
            bala.SetActive(false);
            bala.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    public void ReiniciarEscena() {
        SceneManager.LoadScene(0);
    }

    public void cargarMenu() {
        SceneManager.LoadScene(0);
    }
}

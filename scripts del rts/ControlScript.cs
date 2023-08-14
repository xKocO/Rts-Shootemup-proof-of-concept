using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ControlScript : MonoBehaviour {

    public float dificultad,spawnerTimer,matrizIniX,matrizFinX,matrizIniZ,matrizFinz,sumaX,restaZ;
    public static float Danger;
    public int matrizSizeX, matrizSizeZ,cantidadDeEnemigos;
    int incremento = 0;
    float tiempoTotal = 0,auxtiempo;
    public Slider slider;
    public Text Timer;
    public bool jugando;
    public static bool rts;
    bool pausa = false;
    GameObject[] zonasD,autos,select;
    public GameObject pausedTxt,camera1,camera2, reiniciarEscena,menubtn;
    Vector3[,] posicion;
    List<GameObject> zonas;


    // Use this for initialization
    void Start () {

        //Debug.Log(PlayerPrefs.GetFloat("highscore"));
        auxtiempo = spawnerTimer;
        select = GameObject.FindGameObjectsWithTag("select");
        Danger = 0;
        zonas = new List<GameObject>();
        rts = true;
        posicion = new Vector3[matrizSizeX, matrizSizeZ];

        crearMatrizPos(matrizIniX,matrizFinX,matrizIniZ,matrizFinz,sumaX,restaZ);

        autos = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject autos in autos)
        {
            autos.GetComponent<playerController>().activo = false;
        }

        zonas.AddRange(GameObject.FindGameObjectsWithTag("DANGER"));
        zonas.AddRange(GameObject.FindGameObjectsWithTag("Shooter"));
        zonasD = zonas.ToArray();
        foreach (GameObject a in zonasD)
        {
            if (a.activeSelf)
            {
                //Debug.Log("hola");
                a.SetActive(false);
            }
        }
        empezar();
	}
	
	// Update is called once per frame
	void Update () {

        if (Danger < 0)
        {
            Danger = 0;
        }

        if (Danger >= 100 && jugando) { 
            jugando = false;
            RIP();
        }

        if (jugando) {
            auxtiempo -= Time.deltaTime;
            //Debug.Log("tiempo:  " + Mathf.RoundToInt(auxtiempo));
            if (auxtiempo <= 0) { auxtiempo = spawnerTimer; }
            tiempoTotal += Time.deltaTime;
            Timer.text = "City Survival Time: " + Mathf.Round(tiempoTotal);
            slider.value = Danger;
        }

        if (Input.GetKeyDown(KeyCode.P) && rts)
        {
            if (jugando) { parar(); pausedTxt.SetActive(true); }
            else
            if (!jugando) { empezar(); pausedTxt.SetActive(false); }
        }

	}

    void RIP()
    {
        if(tiempoTotal > PlayerPrefs.GetFloat("highscore"))
        PlayerPrefs.SetFloat("highscore", Mathf.Round(tiempoTotal));
        reiniciarEscena.SetActive(true);
        menubtn.SetActive(true);
        Debug.Log(PlayerPrefs.GetFloat("highscore"));
    }

    public void empezar() {
        jugando = true;
        StartCoroutine("spawner");
        StartCoroutine("danger");
    }

    public void parar() {
        StopAllCoroutines();
        jugando = false;
        pausa = true;
    }

    void crearMatrizPos(float xPosIni,float xPosFin,float zPosIni,float zPosFin,float sumaX,float SumaZ) {

        int xx = 0, zz = 0;
        for (float x = xPosIni; x <= xPosFin; x += sumaX)
        {
            for (float z = zPosIni; z >= zPosFin; z -= restaZ)
            {
                posicion[xx, zz] = new Vector3(x, 0.75f, z);
                //Debug.Log("POSICIONES:  " + posicion[xx, zz]);
                zz++;
            }
            xx++;
            zz = 0;
        }
    }

    public void DesactivarAuto() {
        int aux = 0;
        foreach (GameObject autos in autos)
        {
            Material MATT;
            autos.GetComponent<playerController>().activo = false;
            MATT = select[aux].GetComponentInChildren<MeshRenderer>().material;
            MATT.DisableKeyword("_EMISSION");
            aux++;
            //Debug.Log("desactivando");
        }
    }

    IEnumerator danger() {
        if (jugando) { 
        while (Danger < 100) {
            Danger -= 0.1f;
            foreach (GameObject zona in zonasD)
            {
                if (zona.activeSelf)
                {
                    Danger += dificultad;
                }
            }
            //Debug.Log("danger:  " + Danger);
            yield return new WaitForSeconds(0.1f);
        }
        }
        //Debug.Log("TERMINO CORUTINA");
    }

    IEnumerator spawner() {
        if (jugando) { 
        while(Danger < 100) {
                if (!pausa) { yield return new WaitForSeconds(spawnerTimer); }
                else if (pausa) { yield return new WaitForSeconds(auxtiempo); pausa = false;auxtiempo = spawnerTimer; }
                int x = Random.Range(0, cantidadDeEnemigos),xx = Random.Range(0,matrizSizeX),zz = Random.Range(0,matrizSizeZ);

            if (!zonasD[x].activeSelf)
            {
                zonasD[x].SetActive(true);

                foreach (GameObject yaesta in zonasD) {
                        while(yaesta.transform.position == posicion[xx,zz]) {
                            xx = Random.Range(0, matrizSizeX);
                            zz = Random.Range(0, matrizSizeZ);
                        }
                }

                zonasD[x].transform.position = posicion[xx, zz];
                incremento++;         
            }
            if (incremento == 10)
            {
                if (spawnerTimer > 1) { spawnerTimer -= 0.25f; }
                incremento = 0;                
            }
            }
        }
        //Debug.Log("TERMINO CORUTINA");

    }

    public void CambioDeCamara() {
        if (camera1.activeSelf)
        {
            camera1.SetActive(false);
            camera2.SetActive(true);
        }else if (camera2.activeSelf)
        {
            camera1.SetActive(true);
            camera2.SetActive(false);
        }
    }

}

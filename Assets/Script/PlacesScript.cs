using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System;

public class PlacesScript : MonoBehaviour
{
    public Canvas CanvasPista;
    public Image Map;
    public Image Logo;
    public Button BackButtonMap;
    public TMP_Text objetivo_actual, conjunto, lugares_restantes;
    public TextMeshProUGUI lugarActual;
    public TextMeshProUGUI sigLugar;
    public TextMeshProUGUI primerLugar;
    public int ruta;
    public int banderaLugAct;
    public int banderaLugSig;

    public Sprite conjuntoNorteSprite, conjuntoSurSprite;

    public int MarcadorLeido;
    public int id;
    private int areas_abarcadas;

    public GameObject vacio, mapaPrincipal, mapaAnexo;

    [Serializable]
    public class ubicacion
    {
        public string nombre;
        public GameObject posicion, marcador;
    }

    private int[] anexo = new int[] { 0, 2, 3, 4, 6, 7, 8, 9, 11};
    private int[] principal = new int[] { 1, 5, 10};

    bool EstUno = false;
    public EstacionUNO verificar;

    public GameObject[] marcadores;

    public string[] rutasTXT = new string[] {
    "Sociedad de Desarrollo en Videojuegos",
    "PumaHAT",
    "Redes y Seguridad",
    "PROTECO",
    "Multimedia e Internet",
    "Club de Robótica FI",
    "Laboratorio de Intel",
    "Club de Programación Competitiva",
    "Laboratorio iOs",
    "Lab. Inteligencia Artificial de Microsoft",
    "Biorobótica",
    "Laboratorio de Microcomputadoras",
    "Aula Híbrida para la Educación Híbrida",
    "Biblioteca Principal",
    "Biblioteca Posgrado",
    "Vagón Tren",
    "Leonardita"
};
    public List<ubicacion> ubicacions = new List<ubicacion>();


    public List<int> ruta1 = new List<int>();

    public List<int> genRoute(int areas_abarcadas)
    {
        anexo = ShuffleArray(anexo);
        principal = ShuffleArray(principal);
        List<int> ruta = new List<int>();

        switch (areas_abarcadas) 
        {
            case 0:
                foreach(int lugar in anexo)
                    ruta.Add(lugar);
                break;

            case 1:
                foreach(int lugar in principal)
                    ruta.Add(lugar);
                break;

            default:
                if (UnityEngine.Random.Range(0, 2) == 0)
                {
                    foreach (int lugar in CombineArrays(anexo, principal))
                    {
                        ruta.Add(lugar);
                    }
                }
                else
                {
                    foreach (int lugar in CombineArrays(principal, anexo))
                    {
                        ruta.Add(lugar);
                    }
                }
                break;
        }
        return ruta;
    }

    public void UpdateMap()
    {
        foreach(int lugar in anexo)
        {
            Debug.Log("Lugar actual: " + lugar);
            Debug.Log("ruta1: " + ruta1[0]);
            if(lugar == ruta1[0])
            {
                conjunto.text = "Conjunto Sur";
                Map.sprite = conjuntoSurSprite;
                break;
            }

            else
            {
                conjunto.text = "Conjunto Norte";
                Map.sprite = conjuntoNorteSprite;
            }
        }
        
        ubicacions[ruta1[0]].posicion.SetActive(true);
        ubicacions[ruta1[0]].marcador.SetActive(true);
        objetivo_actual.text = ubicacions[ruta1[0]].nombre;
        lugares_restantes.text = "Lugares restantes: \n" + ruta1.Count;
            
    }

    private static int[] ShuffleArray(int[] array)
    {
        System.Random rand = new System.Random();
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = rand.Next(i + 1);
            int temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
        return array;
    }

    private static int[] CombineArrays(int[] first, int[] second)
    {
        int[] result = new int[first.Length + second.Length];
        first.CopyTo(result, 0);
        second.CopyTo(result, first.Length);
        return result;
    }

    [System.Serializable]
    public class Conjunto
    {
        public int norte;
        public int sur;
    }

    [System.Serializable]
    public class ConfigAppData
    {
        public int duracionMinutos;
        public Conjunto conjuntos;
    }


    void Start()
    {

        ConfigAppData configData = LoadJsonFromFile("ConfigAppJson.json");

        if (configData != null)
        {
            // Muestra los datos deserializados
            Debug.Log("Duración (minutos): " + configData.duracionMinutos);
            Debug.Log("Norte: " + configData.conjuntos.norte);
            Debug.Log("Sur: " + configData.conjuntos.sur);

            if (configData.conjuntos.norte == 0 && configData.conjuntos.sur == 0)
            {
                // TODO: Mensaje de app no disponible
                areas_abarcadas = -1;
            }
            else if (configData.conjuntos.norte == 0 && configData.conjuntos.sur == 1)
            {
                areas_abarcadas = 0;
            }
            else if (configData.conjuntos.norte == 1 && configData.conjuntos.sur == 0)
            {
                areas_abarcadas = 1;
            }
            else {
                areas_abarcadas = 2;
            }
        }

        ruta1 = genRoute(areas_abarcadas);
        // Imprimir la lista generada en la consola de Unity
        foreach (int index in ruta1)
        {
            Debug.Log(rutasTXT[index]);
        }

        UpdateMap();

    }

    ConfigAppData LoadJsonFromFile(string fileName)
    {
        // Obtén la ruta completa al archivo JSON en la raíz del proyecto
        string fullPath = Path.Combine(Application.dataPath, "..", fileName);

        if (File.Exists(fullPath))
        {
            // Lee todo el contenido del archivo
            string jsonContent = File.ReadAllText(fullPath);

            // Deserializa el JSON a la clase ConfigAppData
            ConfigAppData configData = JsonUtility.FromJson<ConfigAppData>(jsonContent);
            return configData;
        }
        else
        {
            Debug.LogError("No se pudo encontrar el archivo JSON: " + fullPath);
            return null;
        }
    }

    public void CargarEstUno()
    {
        string ruta = Path.Combine(Application.persistentDataPath, "EstacionUno.data");
        string EstUnopJSON = File.ReadAllText(ruta);

        //Coversion a objeto 
        EstacionUNO EstUnoLoaded = JsonUtility.FromJson<EstacionUNO>(EstUnopJSON);

        banderaLugAct = EstUnoLoaded.existe;
        banderaLugSig = EstUnoLoaded.lugarSiguiente;
        marcadores[banderaLugAct].SetActive(true);
        MarcadorLeido = banderaLugAct;
    }


    public void GuardarEnEstUno()
    {
        verificar.existe = banderaLugAct;
        verificar.lugarSiguiente = banderaLugSig;
        string Verifi = JsonUtility.ToJson(verificar).ToString();
        string camino = Path.Combine(Application.persistentDataPath, "EstacionUno.data");
        File.WriteAllText(camino, Verifi);
    }


    public void ActivarMapa()
    {
        Map.gameObject.SetActive(true);
        BackButtonMap.gameObject.SetActive(true);
    }

    public void DesactivarMapa()
    {
        Map.gameObject.SetActive(false);
        BackButtonMap.gameObject.SetActive(false);
    }



    void Desactivar()
    {
        for (int i = 0; marcadores.Length > i; i++)
        {
            marcadores[i].SetActive(false);
        }
    }




    public void primeraEstacion(int ruta)
    {
        primerLugar.text = "Tu siguiente lugar es:";
        if (ruta == 1)
        {
            int lugar1 = ruta1[0];
            int lugarSig = ruta1[1];
            //bandera actual guarda lo que hay dentro 
            banderaLugSig = lugarSig;
            banderaLugAct = lugar1;

            lugarActual.text = rutasTXT[banderaLugAct];
            Desactivar();
            marcadores[banderaLugAct].SetActive(true);
            MarcadorLeido = banderaLugAct;

            //verificar el valor de la bandera al iniciar
            Debug.Log("EL VALOR DE LA BANDERA PRIMERA INSTANCIA");
            Debug.Log(banderaLugAct);
            Debug.Log(banderaLugSig);
        }

        //colocar JSON
        verificar.existe = banderaLugAct;
        verificar.lugarSiguiente = banderaLugSig;
        string Verifi = JsonUtility.ToJson(verificar).ToString();
        string camino = Path.Combine(Application.persistentDataPath, "EstacionUno.data");
        File.WriteAllText(camino, Verifi);
        Debug.Log("Creado");
        Debug.Log(camino);


    }

    public void sodvi()
    {
        id = 0;
    }
    public void pumahat()
    {
        id = 1;
    }
    public void redes()
    {
        id = 2;
    }
    public void proteco()
    {
        id = 3;
    }
    public void multi()
    {
        id = 4;
    }
    public void crofi()
    {
        id = 5;
    }
    public void intel()
    {
        id = 6;
    }
    public void clubprog()
    {
        id = 7;
    }
    public void ios()
    {
        id = 8;
    }
    public void ia()
    {
        id = 9;
    }
    public void biorobotica()
    {
        id = 10;
    }
    public void micros()
    {
        id = 11;
    }
    public void aulahinrida()
    {
        id = 12;
    }
    public void biblioprin()
    {
        id = 13;
    }
    public void bibliopos()
    {
        id = 14;
    }
    public void vagon()
    {
        id = 15;
    }
    public void leonardita()
    {
        id = 16;
    }




    public void escanear()
    {


        if (ruta == 1)
        {
            if (marcadores[banderaLugAct].activeInHierarchy == true && marcadores[id] != vacio)
            {
                if (id == banderaLugAct)
                {
                    M();
                    MarcadorLeido = banderaLugSig;
                    marcadores[banderaLugAct].SetActive(false);
                    marcadores[banderaLugAct] = vacio;



                    banderaLugAct = banderaLugSig;
                    SigEstacion();

                    GuardarEnEstUno();


                    marcadores[banderaLugAct].SetActive(true);

                }

            }

            Debug.Log("estoy en la funcion de scanner Ruta 1");
        }
        else if (ruta == 2)
        {
            if (marcadores[banderaLugAct].activeInHierarchy == true && marcadores[id] != vacio)
            {
                if (id == banderaLugAct)
                {
                    M();
                    MarcadorLeido = banderaLugSig;
                    marcadores[banderaLugAct].SetActive(false);
                    marcadores[banderaLugAct] = vacio;



                    banderaLugAct = banderaLugSig;
                    SigEstacion();

                    GuardarEnEstUno();


                    marcadores[banderaLugAct].SetActive(true);

                }

            }

            Debug.Log("estoy en la funcion de scanner Ruta 2");

        }
        else if (ruta == 3)
        {

            if (marcadores[banderaLugAct].activeInHierarchy == true && marcadores[id] != vacio)
            {
                if (id == banderaLugAct)
                {
                    M();
                    MarcadorLeido = banderaLugSig;
                    marcadores[banderaLugAct].SetActive(false);
                    marcadores[banderaLugAct] = vacio;



                    banderaLugAct = banderaLugSig;
                    SigEstacion();

                    GuardarEnEstUno();


                    marcadores[banderaLugAct].SetActive(true);

                }

            }

            Debug.Log("estoy en la funcion de scanner Ruta 3");
        }
    }






    public void SigEstacion()
    {
        int contador = 0;
        while (ruta1[contador] != banderaLugAct && ruta1[contador] < banderaLugAct)
        {
            contador++;
        }
        banderaLugSig = ruta1[contador + 1];

        Debug.Log("valor siguiente que se va a mandar a la funcion");
        Debug.Log(banderaLugSig);
        Debug.Log("valor actual mostrar estación que se va a mandar a la funcion");
        Debug.Log(banderaLugAct);


    }




    //Aqui no sepuede actulizar el valor del indicie de la siguiente estación porque lo hace muy rapido 
    public void M()
    {
        switch (banderaLugAct)
        {

            case 0: //SODVI
                lugarActual.text = rutasTXT[banderaLugSig];
                break;

            case 1: //PUMAHAT
                lugarActual.text = rutasTXT[banderaLugSig];
                break;

            case 2: //REDES
                lugarActual.text = rutasTXT[banderaLugSig];
                break;

            case 3: //PROTECO
                lugarActual.text = rutasTXT[banderaLugSig];
                break;

            case 4: //MULTIMEDIA
                lugarActual.text = rutasTXT[banderaLugSig];
                break;

            case 5: //CROFI
                lugarActual.text = rutasTXT[banderaLugSig];
                break;

            case 6: //INTEL
                lugarActual.text = rutasTXT[banderaLugSig];
                break;

            case 7: // CLUB DE PROGRAMACIÓN
                lugarActual.text = rutasTXT[banderaLugSig];
                break;

            case 8: //iOs
                lugarActual.text = rutasTXT[banderaLugSig];
                break;

            case 9: // Inteligencia Artificial
                lugarActual.text = rutasTXT[banderaLugSig];
                break;

            case 10: //Biorobotica
                lugarActual.text = rutasTXT[banderaLugSig];
                break;

            case 11: //Microcomputadoras
                lugarActual.text = rutasTXT[banderaLugSig];
                break;

            case 12: //Aula H�brida
                lugarActual.text = rutasTXT[banderaLugSig];
                break;

            case 13: //Biblioteca Principal
                lugarActual.text = rutasTXT[banderaLugSig];
                //Se activa juego
                sigLugar.text = rutasTXT[banderaLugSig];
                break;

            case 14: //Biblioteca Posgrado
                lugarActual.text = rutasTXT[banderaLugSig];
                //Se activa juego
                sigLugar.text = rutasTXT[banderaLugSig];
                break;

            case 15: //Vagon tren 
                lugarActual.text = rutasTXT[banderaLugSig];
                //Se activa juego
                sigLugar.text = rutasTXT[banderaLugSig];
                break;

            case 16: //Leonardita
                lugarActual.text = rutasTXT[banderaLugSig];
                //Se activa juego
                sigLugar.text = rutasTXT[banderaLugSig];
                break;

        }
    }


}
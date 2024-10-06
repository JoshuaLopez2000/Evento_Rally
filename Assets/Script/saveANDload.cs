using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using TMPro;
public class saveANDload : MonoBehaviour
{
    // el IF es de Input Field
    public List<string> AlumnosNames;
    public List<string> AlumnosCuentas;

    public TMP_Text jugadores, errorMessage;

    public TMP_InputField IFNombre;
    public TMP_InputField IFNoCuenta;
    public List<int> lugares;

    public GameObject CanvasError, tutorial, menu_registro;

    private int numJugador = 0;
    public Animator panel_animator;

    private int maxPlayers = 3;
    public int ruta;

    public PlacesScript places_gen;

    //Hace visible los campos del objeto tipo DATOS
    [SerializeField]
    public DATOS informacion;
    private string filePath;
    //sirve para dar la funcion de guardar en el inspector
    [ContextMenu("Save")]

    //sirve para dar la funcion de guardar en el inspector
    private void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "DataUsers.data");
    }

    //funcion para guardar los datos en formato JSON 
    public void save()
    {
         string RegistroGuardado = JsonUtility.ToJson(informacion);
         string ruta = Path.Combine(Application.persistentDataPath, "DataUsers.data");
         File.WriteAllText(ruta,RegistroGuardado);
         Debug.Log(RegistroGuardado);
         Debug.Log(ruta);
    }


    // esta funcion recibe los datos del inputfield y los rescribre en el objeto informacion 
    //para posteriormente hacer el manejo de los datos en el JSON
    public void RegistrarDatos()
    {
        if (AlumnosNames.Count <= 0)
        {
            CanvasError.SetActive(true);
            errorMessage.text = "Debe haber mínimo un participante por equipo.";
        }
        else
        {
            informacion.nombres = AlumnosNames;
            informacion.cuentas = AlumnosCuentas;
            lugares = places_gen.genRoute(ruta);
            informacion.lugaresPendientes = lugares;
            save();
            Tutorial();
        }
 
    }

    //funcion para cargar los datos de formato JSO transformandolos en onjeto tipo DATO
    //tambei los carga a las variables del objeto DATO (los carga en el objeto "infoLoaded")
    [ContextMenu("Load")]
    public void Load()
    {
        string ruta = Path.Combine(Application.persistentDataPath, "DataUsers.data");
        string infoJson = File.ReadAllText(ruta);

        //Coversion a objeto 
        DATOS infoLoaded = JsonUtility.FromJson<DATOS>(infoJson);

        //cargar información a la instancia anterior 
        informacion.nombres = infoLoaded.nombres;
        informacion.cuentas = infoLoaded.cuentas;
        informacion.lugaresPendientes = infoLoaded.lugaresPendientes;

        AlumnosNames = informacion.nombres;
        AlumnosCuentas = informacion.cuentas;
    }

    //activeInHirtarchy sirve para ver el esatdo del objeto si esta activo o no
    public void AgregarJugador()
    {
        if(IFNoCuenta.text.Length == 9)
        {
            if (numJugador < maxPlayers)
            {
                AlumnosNames.Add(IFNombre.text);
                AlumnosCuentas.Add(IFNoCuenta.text);
                IFNombre.text = "";
                IFNoCuenta.text = "";
                jugadores.text = jugadores.text + AlumnosNames[numJugador] + " " + AlumnosCuentas[numJugador] + "\n";
                numJugador++;
            }
            else
            {
                CanvasError.SetActive(true);
                errorMessage.text = "Se llegó al máximo de jugadores permitidos.";
            }
        }
        else
        {
            CanvasError.SetActive(true);
            errorMessage.text = "El número de cuenta debe ser de 9 dígitos.";
        }
    }

    //Funcion para desactivar el canvas de error
    public void Tutorial()
    {
        panel_animator.SetTrigger("Tutorial");
    }


}

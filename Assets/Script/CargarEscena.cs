using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;// para la clase button
using System.IO;// para el manejo de datos (save and load archivos)
using System;//para el debug
using UnityEngine.SceneManagement;// para cargar escena

public class CargarEscena : MonoBehaviour
{
    [SerializeField]
    public NUMESCENA NumeroEscena;


    bool existePartida= false;
    public Button botonContinuar, botonNuevaPartida;
    public GameObject menu_principal, menu_registro;
    public Animator panel_animator;

    // Start is called before the first frame update
    void Start()
    {
        existePartida = File.Exists(Path.Combine(Application.persistentDataPath, "date.data"));
        if (existePartida == true)
        {
            //botonNuevaPartida.gameObject.SetActive(false);
            botonContinuar.gameObject.SetActive(true);
        }
    }

   public void CargarScene()
    {
        SceneManager.LoadScene(1);
    }

    public void NuevaPartida()
    {
        string estUnoPath = Path.Combine(Application.persistentDataPath, "EstacionUno.data");

        if (File.Exists(estUnoPath))
        {
            File.Delete(estUnoPath);
            Debug.Log("Archivo EstacionUno.data eliminado.");
        }

        panel_animator.SetTrigger("Closing");
        panel_animator.SetTrigger("Registro");
    }

    public void mapa()
    {
        panel_animator.SetTrigger("Closing");
        panel_animator.SetTrigger("Mapa");
    }

    public void InicioScanner()//Tutorial
    {
        SceneManager.LoadScene(1);
    }
}

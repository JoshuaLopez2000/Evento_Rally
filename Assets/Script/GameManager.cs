using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public tiempo tiempo;
    public saveANDload datos_guardados;
    public PlacesScript places;
    public GameObject vista_principal;
    public AnimacionRuleta ruleta;
    public GameObject ruleta_canvas;
    public TMP_Text ruleta_objetivo;
    void Start()
    {
        tiempo.LoadFecha();
        datos_guardados.Load();
        places.ruta1 = datos_guardados.informacion.lugaresPendientes;
        places.UpdateMap();
        Debug.Log(tiempo.ToString());
    }

    public void ObjetivoEscaneado()
    {
        if (places.ruta1.Count > 1)
        {
            places.ubicacions[places.ruta1[0]].posicion.SetActive(false);
            places.ubicacions[places.ruta1[0]].marcador.SetActive(false);
            places.ruta1.RemoveAt(0);
            places.UpdateMap();
            datos_guardados.informacion.lugaresPendientes = places.ruta1;
            datos_guardados.save();
            vista_principal.SetActive(false);
            ruleta.lugarSeleccionado = places.ruta1[0];
            ruleta_objetivo.text = places.ubicacions[places.ruta1[0]].nombre;
            ruleta_canvas.SetActive(true);
        }
        else
        {
            PartidaTerminada();    
        }
    }

    public void PartidaTerminada()
    {
        Debug.Log("Ganaste!!");
    }

    // Update is called once per frame
}

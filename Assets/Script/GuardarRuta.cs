using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;


public class GuardarRuta : MonoBehaviour
{

    [SerializeField]
    public RUTA rutaCal;
    bool RutaAsignada = false;

    public void CalculaRuta()
    {
        //Random rutaRand = new Random();
        int rutaEntero = 1;
        if (rutaEntero == 1 || rutaEntero == 2 || rutaEntero == 3)
        {

            RutaAsignada = File.Exists(Path.Combine(Application.persistentDataPath, "NumRuta.data"));
            if(RutaAsignada == false)
            {
                GuardarRutaCalculada(rutaEntero);
            }
            else
            {
                Debug.Log("ya tiene ruta asignada");
            }
        }
        else
        {
            CalculaRuta();
        }
       
        
    }

    public void GuardarRutaCalculada(int ruta)
    {
        rutaCal.numRuta = ruta;
        string rutaStr = JsonUtility.ToJson(rutaCal).ToString();
        string camino = Path.Combine(Application.persistentDataPath, "NumRuta.data");
        File.WriteAllText(camino, rutaStr);

        Debug.Log(rutaStr);
        Debug.Log(camino);
    }
}

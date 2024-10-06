using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Networking;
using TMPro;

public class tiempo : MonoBehaviour
{
    [SerializeField]
    public DATE calendario;

    private const string timeApiUrl = "https://worldtimeapi.org/api/timezone/Etc/UTC";
    private DateTime currentDateTime;

    DateTime time;
    DateTime tiempoInicial;
    DateTime tiempoFinal;
    TimeSpan tiempoTotal;
    public TMP_Text HorarioInicio;
    public Text HorarioFin;
    public Text HorarioTotal;

    public GameObject errorCanvas;
    public TMP_Text errorText;

    public saveANDload saveANDload;

    //sirve para dar la funcion de guardar en el inspector
    [ContextMenu("SaveFecha")]

    private IEnumerator GetTimeFromInternet()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(timeApiUrl))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error al obtener la hora: " + webRequest.error);
                errorCanvas.SetActive(true);
                errorText.text = "Error de conexión";
            }
            else
            {
                // Procesar la respuesta
                string jsonResponse = webRequest.downloadHandler.text;
                TimeResponse timeResponse = JsonUtility.FromJson<TimeResponse>(jsonResponse);

                // Guardar la hora obtenida en una variable DateTime
                currentDateTime = DateTime.Parse(timeResponse.utc_datetime);
                Debug.Log("Hora obtenida de internet (UTC): " + currentDateTime);
                GuardarFecha();
            }
        }
    }

    public void TryRequest()
    {
        // Oculta el canvas de error antes de reintentar
        StartCoroutine(GetTimeFromInternet());
    }

    //funcion para guardar los datos en formato JSON 
    public void GuardarFecha()
    {
        calendario.FechaHora = currentDateTime.ToString();
        string RelojGuardado = JsonUtility.ToJson(calendario);
        string ruta = Path.Combine(Application.persistentDataPath, "date.data");
        File.WriteAllText(ruta,RelojGuardado);
        Debug.Log(RelojGuardado);
        Debug.Log(ruta);
        saveANDload.RegistrarDatos();
    }


    [ContextMenu("LoadFecha")]
    public void LoadFecha()
    {
        string ruta = Path.Combine(Application.persistentDataPath, "date.data");
        string FechahoraJson = File.ReadAllText(ruta);

        //Coversion a objeto 
        DATE dateLoaded = JsonUtility.FromJson<DATE>(FechahoraJson);

        //cargar información a la instancia anterior 
        calendario.FechaHora = dateLoaded.FechaHora;
    

        //Cargar la info del nuevo objeto "informacion" a los carteles de texto
        HorarioInicio.text = "Hora de inicio: \n" + calendario.FechaHora;



        //para convertir el tiempo inicio a dataTime ya que mas adelante lo ocupamos
        //tiempoInicial = DateTime.Parse(calendario.FechaHora);
        

        //Se calcula el tiempo final y se muestra en el texto
        //tiempoFinal = DateTime.Now;
        //GuardarFechaFinal(tiempoFinal);

        //HorarioFin.text = calendario.FechaHoraFin;

        //tiempoFinal = DateTime.Parse(calendario.FechaHoraFin);
        

        // se tiene que calcular el tiempo total = tiempoFinal - tiempoInicial
        //tiempoTotal = tiempoFinal - tiempoInicial ;
        
        //HorarioTotal.text = tiempoTotal.ToString();

    }

    public void GuardarFechaFinal(DateTime fh)
    {
        time = fh;
        calendario.FechaHoraFin = time.ToString();
        string RelojGuardado = JsonUtility.ToJson(calendario);
        string ruta = Path.Combine(Application.persistentDataPath, "date.data");
        File.WriteAllText(ruta, RelojGuardado);

        Debug.Log(RelojGuardado);
        Debug.Log(ruta);
    }



}

[Serializable]
public class TimeResponse
{
    public string utc_datetime;
}

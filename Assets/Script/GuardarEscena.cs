using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class GuardarEscena : MonoBehaviour
{
    [SerializeField]
    public NUMESCENA escena;
    public int numEscena;
    
    // Start is called before the first frame update
    void Start()
    {
       SaveEscena();
    }

    public void SaveEscena()
    {
        escena.NumEscena = numEscena;
        string CadenaNum = JsonUtility.ToJson(escena).ToString();
        string ruta = Path.Combine(Application.persistentDataPath, "NumEscena.data");
        File.WriteAllText(ruta, CadenaNum);

        Debug.Log(CadenaNum);
        Debug.Log(ruta);
    }

}

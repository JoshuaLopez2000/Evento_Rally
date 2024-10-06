using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;// para cargar escena

public class FuncionesMenu : MonoBehaviour
{
    public Image Map;
    public Button BackButtonMap;
    public Image Tutorial;
    public Button BackButtonTutorial;

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

    public void ActivarTutorial()
    {
        Tutorial.gameObject.SetActive(true);
        BackButtonTutorial.gameObject.SetActive(true);
    }

    public void DesactivarTutorial()
    {
        Tutorial.gameObject.SetActive(false);
        BackButtonTutorial.gameObject.SetActive(false);
    }

    public void salir()
    {
        Application.Quit();
    }


    public void Menu()
    {
        //cargar la escena
        SceneManager.LoadScene(0);
    }
}

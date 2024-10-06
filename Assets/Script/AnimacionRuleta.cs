using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimacionRuleta : MonoBehaviour
{
    public ScrollRect scrollRect;  // El ScrollRect del ScrollView
    public RectTransform content;  // El RectTransform que contiene los elementos
    public float velocidad = 0.0001f;   // Velocidad inicial de la ruleta
    public float desaceleracion = 0.9f; // Factor de desaceleración
    public float aceleracion = 1.0f;
    public int lugarSeleccionado;  // Índice del lugar donde se detendrá la ruleta
    public GameObject texto_objetivo, boton_ok;
    private float itemWidth;       // Ancho de un elemento individual
    private float contentWidth;    // Ancho del contenido del ScrollView
    private bool animacionEnCurso = false; // Para evitar múltiples llamadas a la animación
    float tiempo = 0f;

    private void OnEnable()
    {
        IniciarRuleta();
    }

    void Start()
    {
        // Calcular el ancho del contenido y el de un solo elemento
        itemWidth = content.GetChild(0).GetComponent<RectTransform>().rect.width;
        contentWidth = itemWidth * content.childCount;

        // Duplicar el contenido para hacer que la ruleta sea cíclica
        //DuplicarContenido();

        // Simular un lugar seleccionado de antemano
        //lugarSeleccionado = Random.Range(0, content.childCount / 2);

        // Comprobar que el contenido fue duplicado correctamente
        //Debug.Log("Contenido duplicado, total de hijos: " + content.childCount);
    }

    public void IniciarRuleta()
    {
        if (!animacionEnCurso)
        {
            Debug.Log("Ruleta iniciada");
            StartCoroutine(AnimarRuleta());
        }
    }

    IEnumerator AnimarRuleta()
    {

        animacionEnCurso = true;
        float velocidadActual = 0;
        float targetPosition = GetPositionForIndex(lugarSeleccionado);
        //Debug.Log("Velocidad: " + velocidad);
        int spinCount = 0;
        //Debug.Log("Desaceleracion:" + desaceleracion);
        bool start_desaceleracion = false;


        while (velocidadActual < velocidad)
        {
            velocidadActual = Mathf.Lerp(velocidadActual, velocidad+1, aceleracion * Time.deltaTime);
            scrollRect.horizontalNormalizedPosition += (velocidadActual / (content.childCount)) * Time.deltaTime;
            if (scrollRect.horizontalNormalizedPosition >= 1)
            {
                scrollRect.horizontalNormalizedPosition = 0;
            }
            Debug.Log(velocidadActual);
            yield return null;
        }

        while (velocidadActual > 0.035f)
        {
            // Desplazar el contenido hacia la izquierda
            scrollRect.horizontalNormalizedPosition += (velocidadActual / (content.childCount)) * Time.deltaTime;

            // Si el contenido ha pasado al final, volver a la parte inicial (simular ciclo)
            if (scrollRect.horizontalNormalizedPosition >= 1)
            {
                scrollRect.horizontalNormalizedPosition = 0;
                spinCount++;

            }

            //Debug.Log(scrollRect.horizontalNormalizedPosition - targetPosition);

            if ((spinCount > 0) && (Mathf.Abs(scrollRect.horizontalNormalizedPosition - targetPosition) < 0.05f))
            {
                //Debug.Log(scrollRect.horizontalNormalizedPosition - targetPosition);
                start_desaceleracion = true;
                //Debug.Log("Inicia frenado");
                //velocidadActual = velocidad * Mathf.Exp(-tiempo * 0.5f); // Ajusta el factor de exponencial
                //tiempo += Time.deltaTime;
            }

            if (start_desaceleracion)
            {
                velocidadActual = Mathf.Lerp(velocidadActual, 0f, desaceleracion * Time.deltaTime);
                //velocidadActual *= desaceleracion;
            }
            //Debug.Log("Spin count: " + spinCount);
            //Debug.Log("Distancia: " + Mathf.Abs(scrollRect.horizontalNormalizedPosition - targetPosition));

            yield return null;
        }

        //Debug.Log("Sali del while");
        //Debug.Log(velocidadActual);

        // Alinear la ruleta con el lugar seleccionado
        StartCoroutine(AlinearRuleta(targetPosition));
    }

    IEnumerator AlinearRuleta(float targetPosition)
    {
        // Desplazar suavemente hasta el lugar seleccionado
        while (Mathf.Abs(scrollRect.horizontalNormalizedPosition - targetPosition) > 0.001f)
        {
            scrollRect.horizontalNormalizedPosition = Mathf.Lerp(scrollRect.horizontalNormalizedPosition, targetPosition, 1f * Time.deltaTime);
            //Debug.Log("Distancia: " + Mathf.Abs(scrollRect.horizontalNormalizedPosition - targetPosition));
            yield return null;
        }

        // Asegurarse de que se detiene exactamente en la posición seleccionada
        scrollRect.horizontalNormalizedPosition = targetPosition;
        animacionEnCurso = false;
        texto_objetivo.SetActive(true);
        boton_ok.SetActive(true);
        //Debug.Log("Ruleta detenida en posición: " + targetPosition);
    }

    void DuplicarContenido()
    {
        // Clonar los hijos del contenido para que la ruleta parezca cíclica
        int childCount = content.childCount;
        for (int i = 0; i < childCount; i++)
        {
            GameObject clone = Instantiate(content.GetChild(i).gameObject, content);
            clone.transform.SetParent(content, false);

            // Ajustar la posición del clon para que quede al lado del original
            RectTransform cloneRectTransform = clone.GetComponent<RectTransform>();
            cloneRectTransform.anchoredPosition = new Vector2((i + childCount) * itemWidth, cloneRectTransform.anchoredPosition.y);
        }

        // Ajustar el ancho del contenido para abarcar los elementos duplicados
        content.sizeDelta = new Vector2(itemWidth * content.childCount, content.sizeDelta.y);
        Debug.Log("Tamaño del contenido ajustado: " + content.sizeDelta.x);
    }

    float GetPositionForIndex(int index)
    {
        // Calcular la posición normalizada del ScrollView para el índice dado
        int totalElements = content.childCount;  // Debido a que duplicamos los elementos
        Debug.Log(content.GetChild(index));
        float unitWidth = 1f / (totalElements - 1);
        return Mathf.Clamp(index * unitWidth, 0f, 1f);
    }
}

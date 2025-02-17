using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BarraVida : MonoBehaviour
{
    public Sprite corazonEntero; 
    public Sprite corazonMedio; 
    public Sprite corazonVacio; 

    private VidaJugador vidaJugador; // Referencia al script que maneja la vida del jugador
    private List<Image> corazones = new List<Image>(); // Lista para los objetos Image
    private Canvas canvasUI; // Canvas donde se mostrarán los corazones
    private GameObject panelCorazones; // Contenedor para los corazones

    private void Start()
    {
        // Crear el Canvas
        canvasUI = new GameObject("Canvas").AddComponent<Canvas>();
        canvasUI.renderMode = RenderMode.ScreenSpaceOverlay;

        // Agregar CanvasScaler para manejar diferentes resoluciones
        CanvasScaler canvasScaler = canvasUI.gameObject.AddComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(1920, 1080); // Ajusta según la resolución base del juego
        canvasUI.gameObject.AddComponent<GraphicRaycaster>();

        // Crear el panel contenedor de los corazones
        panelCorazones = new GameObject("PanelCorazones");
        panelCorazones.transform.SetParent(canvasUI.transform);

        // Configurar el RectTransform del panel para que esté en la esquina superior izquierda
        RectTransform rectPanel = panelCorazones.AddComponent<RectTransform>();
        rectPanel.anchorMin = new Vector2(0, 1); // Anclado a la esquina superior izquierda
        rectPanel.anchorMax = new Vector2(0, 1);
        rectPanel.pivot = new Vector2(0, 1); // Punto de pivote en la esquina superior izquierda
        rectPanel.anchoredPosition = new Vector2(20, -20); // Margen de 20 píxeles desde la esquina

        CrearCorazones(); // Crear las imágenes de los corazones
        
        // Buscar el objeto con el tag "Jugador" y obtener el componente VidaJugador
        GameObject jugador = GameObject.FindWithTag("Jugador");
        if (jugador != null)
        {
            vidaJugador = jugador.GetComponent<VidaJugador>();
        }

        ActualizarVidas();// Inicializa la UI de los corazones
    }

    private void Update()
    {
        ActualizarVidas();
    }

    private void CrearCorazones()
    {
        int numeroDeCorazones = 3; 
        
        for (int i = 0; i < numeroDeCorazones; i++)
        {
            GameObject corazonObj = new GameObject("Corazon_" + i);
            corazonObj.transform.SetParent(panelCorazones.transform);
            Image corazonImg = corazonObj.AddComponent<Image>();

            // Configurar RectTransform para que se alineen horizontalmente
            RectTransform rectCorazon = corazonObj.GetComponent<RectTransform>();
            rectCorazon.sizeDelta = new Vector2(50, 50); // Tamaño de cada corazón
            rectCorazon.anchorMin = new Vector2(0, 1);
            rectCorazon.anchorMax = new Vector2(0, 1);
            rectCorazon.pivot = new Vector2(0, 1);
            rectCorazon.anchoredPosition = new Vector2(i * 60, 0); // Espaciado horizontal

            corazonImg.sprite = corazonVacio;
            corazones.Add(corazonImg);
        }
    }

    private void ActualizarVidas()
    {
        if (vidaJugador == null) return; 

        float saludActual = vidaJugador.ObtenerSalud();

        if (saludActual <= 0)
        {
            foreach (var corazon in corazones)
            {
                corazon.sprite = corazonVacio;
            }
            return;
        }

        int vidasCompletas = Mathf.FloorToInt(saludActual / 2);
        bool tieneCorazonMedio = saludActual % 2 != 0;

        for (int i = 0; i < corazones.Count; i++)
        {
            if (i < vidasCompletas)
            {
                corazones[i].sprite = corazonEntero;
            }
            else if (i == vidasCompletas && tieneCorazonMedio)
            {
                corazones[i].sprite = corazonMedio;
            }
            else
            {
                corazones[i].sprite = corazonVacio;
            }
        }
    }
}
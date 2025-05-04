using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Para acceder a componentes de UI

public class PanelEliminadoController : MonoBehaviour
{
    [Header("Configuración")]
    public float tiempoEspera = 5f;
    public int indiceEscenaMenu = 0; // Cambia si tu menú no es la escena 0
    
    // GameObject que puede contener el texto "Eliminado"
    [SerializeField] GameObject panelContenido;

    private void Awake()
    {
        Debug.Log("[PanelEliminado] Inicializando panel de eliminado");
        
        // Si el panel de contenido no está asignado, intentamos buscar un hijo
        if (panelContenido == null && transform.childCount > 0)
        {
            panelContenido = transform.GetChild(0).gameObject;
            Debug.Log("[PanelEliminado] Usando primer hijo como panel de contenido: " + panelContenido.name);
        }
        
        // Verifica si el panel está en un Canvas
        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("[PanelEliminado] ¡ERROR! El panel no está dentro de un Canvas. Debes colocarlo dentro de un objeto Canvas para que sea visible.");
        }
    }
    
    private void Start()
    {
        // El PanelEliminado permanece activo, solo ocultamos su contenido
        Debug.Log("[PanelEliminado] Ocultando contenido del panel al inicio");
        
        if (panelContenido != null)
        {
            panelContenido.SetActive(false);
        }
        else
        {
            Debug.LogError("[PanelEliminado] No se encontró el panel de contenido para ocultar");
        }
    }

    public void MostrarPanel()
    {
        Debug.Log("[PanelEliminado] Mostrando panel de eliminado");
        
        
        if (panelContenido != null)
        {
            panelContenido.SetActive(true);
            Debug.Log("[PanelEliminado] Activando contenido interno: " + panelContenido.name);
            
            // Asegurarse de que cualquier imagen o texto sea visible
            Image[] imagenes = panelContenido.GetComponentsInChildren<Image>(true);
            foreach (Image img in imagenes)
            {
                Debug.Log("[PanelEliminado] Imagen encontrada: " + img.name);
                img.enabled = true;
                
                // Verificar color y opacidad
                Color color = img.color;
                if (color.a < 0.1f)
                {
                    Debug.LogWarning("[PanelEliminado] La imagen tiene baja opacidad: " + color.a);
                    color.a = 1.0f;
                    img.color = color;
                }
            }
            
            // Verificar textos
            TMPro.TextMeshProUGUI[] textos = panelContenido.GetComponentsInChildren<TMPro.TextMeshProUGUI>(true);
            foreach (TMPro.TextMeshProUGUI texto in textos)
            {
                Debug.Log("[PanelEliminado] Texto encontrado: " + texto.name + " - Contenido: " + texto.text);
                texto.enabled = true;
            }
        }
        else
        {
            Debug.LogError("[PanelEliminado] No se encontró el panel de contenido");
        }
        
        // Verificar que este panel esté en un Canvas activo
        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            Debug.Log("[PanelEliminado] Canvas padre: " + canvas.name + " - Activo: " + canvas.gameObject.activeInHierarchy);
            
            // Asegurar que el canvas esté activo
            canvas.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("[PanelEliminado] No hay Canvas padre");
        }
        
        // Asegurarse de que el panel esté visible en la pantalla (RectTransform)
        RectTransform rectTransform = GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            Debug.Log("[PanelEliminado] Posición del panel: " + rectTransform.anchoredPosition);
            Debug.Log("[PanelEliminado] Tamaño del panel: " + rectTransform.sizeDelta);
            
            // Si el tamaño es muy pequeño, aumentarlo
            if (rectTransform.sizeDelta.x < 200 || rectTransform.sizeDelta.y < 100)
            {
                Debug.LogWarning("[PanelEliminado] El panel es muy pequeño, ajustando tamaño");
                rectTransform.sizeDelta = new Vector2(400, 200);
            }
        }
        
        // Pausar el juego y activar el temporizador para volver al menú
        Time.timeScale = 0f;
        StartCoroutine(VolverAlMenuTrasEspera());
    }

    private System.Collections.IEnumerator VolverAlMenuTrasEspera()
    {
        float tiempoPasado = 0f;
        Debug.Log("[PanelEliminado] Iniciando temporizador de " + tiempoEspera + " segundos");
        
        while (tiempoPasado < tiempoEspera)
        {
            yield return null;
            tiempoPasado += Time.unscaledDeltaTime;
            
            // Logs cada segundo para verificar que el temporizador está funcionando
            if (Mathf.FloorToInt(tiempoPasado) > Mathf.FloorToInt(tiempoPasado - Time.unscaledDeltaTime))
            {
                Debug.Log("[PanelEliminado] Tiempo restante: " + (tiempoEspera - tiempoPasado).ToString("F1") + " segundos");
            }
        }
        
        // No se oculta el panel ni nada, simplemente se carga la escena
        Debug.Log("[PanelEliminado] Temporizador finalizado, volviendo a escena " + indiceEscenaMenu);
        
        // Restaurar timeScale y cargar escena
        Time.timeScale = 1f;
        SceneManager.LoadScene(indiceEscenaMenu);
    }
} 
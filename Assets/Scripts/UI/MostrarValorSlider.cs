using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Muestra el valor numérico de un slider en un componente de texto.
/// Soporta tanto el componente Text estándar de Unity UI como TextMeshProUGUI.
/// Actualiza el texto automáticamente cuando el valor del slider cambia.
/// </summary>
public class MostrarValorSlider : MonoBehaviour
{
    /// <summary>
    /// Referencia al slider cuyo valor se mostrará.
    /// Si no se asigna, intentará encontrar uno automáticamente.
    /// </summary>
    [SerializeField] private Slider slider;
    
    /// <summary>
    /// Componente de texto estándar de Unity UI donde se mostrará el valor.
    /// </summary>
    [SerializeField] private Text textoValor; // Para UI estándar
    
    /// <summary>
    /// Componente TextMeshProUGUI donde se mostrará el valor.
    /// </summary>
    [SerializeField] private TextMeshProUGUI textoValorTMP; // Para TextMeshPro
    
    /// <summary>
    /// Formato de texto para mostrar el valor. Por defecto es "{0}%" mostrando el valor como porcentaje.
    /// </summary>
    [SerializeField] private string formatoTexto = "{0}%";
    
    /// <summary>
    /// Inicializa el componente buscando referencias necesarias y configurando listeners.
    /// </summary>
    private void Start()
    {
        // Si no se asignó un slider, intentar encontrarlo en este GameObject
        if (slider == null)
            slider = GetComponent<Slider>();
            
        // Si todavía no hay slider, buscar en objetos padres
        if (slider == null && transform.parent != null)
            slider = transform.parent.GetComponent<Slider>();
            
        if (slider != null)
        {
            // Asignar listener al evento de cambio de valor
            slider.onValueChanged.AddListener(ActualizarTexto);
            
            // Actualizar texto con el valor inicial
            ActualizarTexto(slider.value);
        }
        else
        {
            Debug.LogWarning("MostrarValorSlider: No se encontró ningún slider para mostrar su valor");
        }
    }
    
    /// <summary>
    /// Actualiza el texto con el valor del slider convertido a porcentaje.
    /// Funciona con ambos tipos de componentes de texto (Text estándar y TextMeshProUGUI).
    /// </summary>
    /// <param name="valor">Valor actual del slider (entre 0 y 1)</param>
    public void ActualizarTexto(float valor)
    {
        // Convertir a porcentaje (0-100)
        int porcentaje = Mathf.RoundToInt(valor * 100);
        string textoFormateado = string.Format(formatoTexto, porcentaje);
        
        // Actualizar el texto según el tipo que tengamos
        if (textoValor != null)
            textoValor.text = textoFormateado;
            
        if (textoValorTMP != null)
            textoValorTMP.text = textoFormateado;
    }
} 
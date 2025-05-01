using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Muestra el valor de un slider como porcentaje en un texto
/// </summary>
public class MostrarValorSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Text textoValor; // Para UI estándar
    [SerializeField] private TextMeshProUGUI textoValorTMP; // Para TextMeshPro
    [SerializeField] private string formatoTexto = "{0}%";
    
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
    /// Actualiza el texto con el valor del slider
    /// </summary>
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
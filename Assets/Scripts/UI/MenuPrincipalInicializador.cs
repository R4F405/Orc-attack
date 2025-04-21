using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Este script debe añadirse a un objeto en la escena del Menú Principal.
/// Se encarga de añadir sonidos a los botones del menú principal.
/// </summary>
public class MenuPrincipalInicializador : MonoBehaviour
{
    // Lista de botones que deben tener sonido
    public Button[] botonesConSonido;
    
    // Si está marcado, añadirá sonido a todos los botones encontrados en la escena
    public bool aplicarATodosLosBotones = true;
    
    void Start()
    {
        Debug.Log("MenuPrincipalInicializador: Inicializando...");
        
        // Si se debe aplicar a todos los botones, buscarlos
        if (aplicarATodosLosBotones)
        {
            botonesConSonido = FindObjectsOfType<Button>();
        }
        
        // Verificar y asignar sonidos a los botones especificados
        if (botonesConSonido != null && botonesConSonido.Length > 0)
        {
            foreach (Button boton in botonesConSonido)
            {
                if (boton != null)
                {
                    // Añadir listener para el sonido
                    boton.onClick.AddListener(() => SonidosUI.ReproducirSonidoClic());
                    Debug.Log($"Configurado botón: {boton.name} con sonido de clic");
                }
            }
            
            Debug.Log($"MenuPrincipalInicializador: Configurados {botonesConSonido.Length} botones con sonido");
        }
    }
    
    void OnDestroy()
    {
        // Limpiar los listeners al destruir el objeto
        if (botonesConSonido != null)
        {
            foreach (Button boton in botonesConSonido)
            {
                if (boton != null)
                {
                    boton.onClick.RemoveAllListeners();
                }
            }
        }
    }
} 
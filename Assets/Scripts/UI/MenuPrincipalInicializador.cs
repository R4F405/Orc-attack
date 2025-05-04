using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Este script debe añadirse a un objeto en la escena del Menú Principal.
/// Se encarga de añadir sonidos a los botones del menú principal para mejorar la experiencia de usuario.
/// Puede configurarse para aplicar automáticamente los sonidos a todos los botones de la escena o solo a los especificados.
/// </summary>
public class MenuPrincipalInicializador : MonoBehaviour
{
    /// <summary>
    /// Lista de botones a los que se les añadirá el sonido de clic.
    /// Esta lista se utiliza cuando aplicarATodosLosBotones es false.
    /// </summary>
    public Button[] botonesConSonido;
    
    /// <summary>
    /// Si está activado, buscará automáticamente todos los botones en la escena y les añadirá el sonido de clic.
    /// Si está desactivado, solo se aplicará a los botones específicos en el array botonesConSonido.
    /// </summary>
    public bool aplicarATodosLosBotones = true;
    
    /// <summary>
    /// Al iniciar, busca y configura los botones para reproducir sonidos al hacer clic.
    /// </summary>
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
    
    /// <summary>
    /// Al destruir este componente, elimina los listeners de los botones para evitar referencias nulas.
    /// Esto es importante para prevenir errores cuando se cambia de escena.
    /// </summary>
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
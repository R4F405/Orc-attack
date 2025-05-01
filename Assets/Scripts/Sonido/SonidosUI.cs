using UnityEngine;

/// <summary>
/// Clase estática para proporcionar métodos globales para reproducir sonidos de UI.
/// Esta clase actúa como una fachada para GestorSonidosUI.
/// </summary>
public static class SonidosUI
{
    private static bool VerificarGestor()
    {
        if (GestorSonidosUI.instancia == null)
        {
            Debug.LogWarning("SonidosUI: GestorSonidosUI no encontrado, buscando en escena...");
            
            // Intentar encontrar en la escena
            GestorSonidosUI gestor = Object.FindObjectOfType<GestorSonidosUI>();
            
            if (gestor == null)
            {
                Debug.LogError("SonidosUI: No se pudo encontrar GestorSonidosUI en la escena");
                return false;
            }
        }
        
        return true;
    }
    
    /// <summary>
    /// Reproduce el sonido estándar de clic
    /// </summary>
    public static void ReproducirSonidoClic()
    {
        if (VerificarGestor())
            GestorSonidosUI.instancia.ReproducirSonidoClic();
    }
    
    /// <summary>
    /// Reproduce el sonido de error
    /// </summary>
    public static void ReproducirSonidoError()
    {
        if (VerificarGestor())
            GestorSonidosUI.instancia.ReproducirSonidoError();
    }
    
    /// <summary>
    /// Reproduce el sonido de compra
    /// </summary>
    public static void ReproducirSonidoCompra()
    {
        if (VerificarGestor())
            GestorSonidosUI.instancia.ReproducirSonidoCompra();
    }
} 
using UnityEngine;

/// <summary>
/// Clase estática para proporcionar métodos globales para reproducir sonidos de UI.
/// </summary>
/// <remarks>
/// Esta clase actúa como una fachada para GestorSonidosUI, proporcionando una
/// interfaz simplificada para reproducir sonidos de interfaz sin necesidad de acceder
/// directamente a la instancia del gestor. Facilita la integración de sonidos de UI
/// en cualquier parte del código sin referencias directas a objetos.
/// </remarks>
public static class SonidosUI
{
    /// <summary>
    /// Verifica que exista una instancia del GestorSonidosUI y la busca si no está disponible.
    /// </summary>
    /// <returns>True si se encuentra o existe una instancia del gestor, False en caso contrario.</returns>
    /// <remarks>
    /// Método de utilidad interno para asegurar que el sistema tiene acceso al GestorSonidosUI
    /// antes de intentar reproducir cualquier sonido.
    /// </remarks>
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
    /// Reproduce el sonido estándar de clic para elementos de interfaz.
    /// </summary>
    /// <remarks>
    /// Ideal para botones, toggles y otros elementos interactivos de la interfaz
    /// que deberían proporcionar retroalimentación auditiva al ser presionados.
    /// </remarks>
    public static void ReproducirSonidoClic()
    {
        if (VerificarGestor())
            GestorSonidosUI.instancia.ReproducirSonidoClic();
    }
    
    /// <summary>
    /// Reproduce el sonido de error para acciones no permitidas.
    /// </summary>
    /// <remarks>
    /// Útil para notificar al jugador cuando una acción no se puede realizar,
    /// como intentar comprar algo sin recursos suficientes o seleccionar una
    /// opción bloqueada.
    /// </remarks>
    public static void ReproducirSonidoError()
    {
        if (VerificarGestor())
            GestorSonidosUI.instancia.ReproducirSonidoError();
    }
    
    /// <summary>
    /// Reproduce el sonido de compra o transacción exitosa.
    /// </summary>
    /// <remarks>
    /// Adecuado para confirmar transacciones, compras o adquisiciones en el juego,
    /// proporcionando una retroalimentación positiva al jugador.
    /// </remarks>
    public static void ReproducirSonidoCompra()
    {
        if (VerificarGestor())
            GestorSonidosUI.instancia.ReproducirSonidoCompra();
    }
} 
using UnityEngine;

/// <summary>
/// Controla el comportamiento avanzado de la cámara con límites de mapa.
/// </summary>
/// <remarks>
/// Esta clase permite que la cámara siga al jugador respetando límites
/// definidos del mapa. Incluye opciones para activar/desactivar los límites
/// y herramientas visuales en el editor para facilitar su configuración.
/// </remarks>
public class ControladorCamaraBrotato : MonoBehaviour
{
    /// <summary>
    /// Referencia al objeto del jugador que la cámara debe seguir.
    /// </summary>
    private GameObject jugador;
    
    /// <summary>
    /// Referencia a la cuadrícula de fondo para posicionar correctamente la cámara.
    /// </summary>
    public Grid background;
    
    /// <summary>
    /// Configura los límites del área de juego que restringen el movimiento de la cámara.
    /// </summary>
    [Header("Límites del mapa")]
    
    /// <summary>
    /// Determina si se aplican los límites al movimiento de la cámara.
    /// </summary>
    public bool usarLimites = true;
    
    /// <summary>
    /// Límite horizontal izquierdo del área de juego.
    /// </summary>
    public float limiteIzquierdo = -20f;
    
    /// <summary>
    /// Límite horizontal derecho del área de juego.
    /// </summary>
    public float limiteDerecho = 20f;
    
    /// <summary>
    /// Límite vertical superior del área de juego.
    /// </summary>
    public float limiteSuperior = 15f;
    
    /// <summary>
    /// Límite vertical inferior del área de juego.
    /// </summary>
    public float limiteInferior = -15f;

    /// <summary>
    /// Inicializa la cámara buscando al jugador y posicionándola correctamente dentro de los límites.
    /// </summary>
    private void Start()
    {
        BuscarJugador();
        if (jugador != null)
        {
            // Inicializar la posición de la cámara
            Vector3 posicionInicial = new Vector3(
                Mathf.Clamp(jugador.transform.position.x, limiteIzquierdo, limiteDerecho),
                Mathf.Clamp(jugador.transform.position.y, limiteInferior, limiteSuperior),
                -3f
            );
            
            transform.position = posicionInicial;
        }
    }

    /// <summary>
    /// Actualiza la posición de la cámara en cada frame para seguir al jugador.
    /// </summary>
    /// <remarks>
    /// Dependiendo de la configuración, puede aplicar límites al movimiento de la cámara.
    /// </remarks>
    void Update()
    {
        if (jugador == null)
        {
            BuscarJugador(); // Intenta encontrar al jugador si aún no ha sido asignado
            return;
        }

        if (usarLimites)
        {
            SeguirJugadorConLimites();
        }
        else
        {
            // Comportamiento estándar sin límites
            transform.position = new Vector3(jugador.transform.position.x, jugador.transform.position.y, -3f);
        }
    }

    /// <summary>
    /// Actualiza la posición de la cámara respetando los límites definidos del mapa.
    /// </summary>
    void SeguirJugadorConLimites()
    {
        // Obtener posición actual del jugador
        float targetX = jugador.transform.position.x;
        float targetY = jugador.transform.position.y;
        
        // Aplicar límites
        float nuevaX = Mathf.Clamp(targetX, limiteIzquierdo, limiteDerecho);
        float nuevaY = Mathf.Clamp(targetY, limiteInferior, limiteSuperior);
        
        // Actualizar posición de la cámara inmediatamente
        transform.position = new Vector3(nuevaX, nuevaY, -3f);
    }

    /// <summary>
    /// Busca al jugador en la escena mediante su etiqueta "Jugador".
    /// </summary>
    /// <remarks>
    /// La advertencia de depuración se muestra después de 1 segundo para evitar falsos
    /// positivos, ya que la cámara puede inicializarse antes que el jugador.
    /// </remarks>
    void BuscarJugador()
    {
        jugador = GameObject.FindGameObjectWithTag("Jugador");

        if (jugador == null && Time.timeSinceLevelLoad > 1f) // Solo muestra el mensaje después de 1 segundo
        {
            Debug.LogWarning("No se encontró un GameObject con la etiqueta 'Jugador'. Intentando de nuevo...");
        }
    }
    
    /// <summary>
    /// Dibuja los límites del mapa como líneas en el editor para facilitar su visualización y configuración.
    /// </summary>
    /// <remarks>
    /// Este método solo se ejecuta en el editor de Unity y no afecta al juego en tiempo de ejecución.
    /// </remarks>
    private void OnDrawGizmos()
    {
        if (!usarLimites) return;
        
        Gizmos.color = Color.red;
        Vector3 topLeft = new Vector3(limiteIzquierdo, limiteSuperior, 0);
        Vector3 topRight = new Vector3(limiteDerecho, limiteSuperior, 0);
        Vector3 bottomLeft = new Vector3(limiteIzquierdo, limiteInferior, 0);
        Vector3 bottomRight = new Vector3(limiteDerecho, limiteInferior, 0);
        
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
    }
} 
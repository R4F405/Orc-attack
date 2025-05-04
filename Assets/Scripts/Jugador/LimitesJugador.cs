using UnityEngine;

/// <summary>
/// Restringe el movimiento del jugador dentro de un área definida.
/// </summary>
/// <remarks>
/// Esta clase se encarga de impedir que el jugador salga de los límites establecidos
/// del campo de juego, asegurando que siempre se mantenga dentro del área visible.
/// </remarks>
public class LimitesJugador : MonoBehaviour
{
    [Header("Límites del Jugador")]
    /// <summary>
    /// Coordenada X mínima hasta donde el jugador puede moverse.
    /// </summary>
    public float limiteIzquierdo = -22f;
    
    /// <summary>
    /// Coordenada X máxima hasta donde el jugador puede moverse.
    /// </summary>
    public float limiteDerecho = 22f;
    
    /// <summary>
    /// Coordenada Y máxima hasta donde el jugador puede moverse.
    /// </summary>
    public float limiteSuperior = 17f;
    
    /// <summary>
    /// Coordenada Y mínima hasta donde el jugador puede moverse.
    /// </summary>
    public float limiteInferior = -17f;
    
    /// <summary>
    /// Referencia al componente Rigidbody2D del jugador.
    /// </summary>
    private Rigidbody2D rb;
    
    /// <summary>
    /// Referencia al componente que controla el movimiento del jugador.
    /// </summary>
    private MovimientoJugador movimientoJugador;
    
    /// <summary>
    /// Inicializa los componentes necesarios.
    /// </summary>
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        movimientoJugador = GetComponent<MovimientoJugador>();
    }
    
    /// <summary>
    /// Restringe la posición del jugador dentro de los límites después de cada movimiento.
    /// </summary>
    /// <remarks>
    /// LateUpdate asegura que la restricción se aplique después del movimiento normal del jugador.
    /// </remarks>
    private void LateUpdate()
    {
        // Esta función se ejecuta después del movimiento del jugador
        RestringirPosicion();
    }
    
    /// <summary>
    /// Ajusta la posición del jugador para mantenerlo dentro de los límites.
    /// </summary>
    private void RestringirPosicion()
    {
        // Obtener la posición actual
        Vector3 posicionActual = transform.position;
        
        // Aplicar límites
        posicionActual.x = Mathf.Clamp(posicionActual.x, limiteIzquierdo, limiteDerecho);
        posicionActual.y = Mathf.Clamp(posicionActual.y, limiteInferior, limiteSuperior);
        
        // Actualizar posición
        transform.position = posicionActual;
        
        // Si tiene Rigidbody2D, actualizar también su posición
        if (rb != null)
        {
            rb.position = new Vector2(posicionActual.x, posicionActual.y);
        }
    }
    
    /// <summary>
    /// Dibuja los límites del jugador en el editor para facilitar la visualización.
    /// </summary>
    /// <remarks>
    /// Solo visible en la vista de escena del editor de Unity, no afecta al juego.
    /// </remarks>
    private void OnDrawGizmos()
    {
        // Dibujar un rectángulo morado para los límites del jugador
        Gizmos.color = new Color(0.5f, 0f, 0.5f, 0.5f); // Color morado semi-transparente
        
        // Crear los vértices del rectángulo
        Vector3 topLeft = new Vector3(limiteIzquierdo, limiteSuperior, 0);
        Vector3 topRight = new Vector3(limiteDerecho, limiteSuperior, 0);
        Vector3 bottomLeft = new Vector3(limiteIzquierdo, limiteInferior, 0);
        Vector3 bottomRight = new Vector3(limiteDerecho, limiteInferior, 0);
        
        // Dibujar las líneas
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
        
        // Rellenar el área con color semi-transparente
        Vector3 center = new Vector3((limiteIzquierdo + limiteDerecho) / 2, (limiteInferior + limiteSuperior) / 2, 0);
        Vector3 size = new Vector3(limiteDerecho - limiteIzquierdo, limiteSuperior - limiteInferior, 0.1f);
        Gizmos.DrawCube(center, size);
    }
} 
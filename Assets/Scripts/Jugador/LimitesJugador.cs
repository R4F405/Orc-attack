using UnityEngine;

public class LimitesJugador : MonoBehaviour
{
    [Header("Límites del Jugador")]
    public float limiteIzquierdo = -22f;
    public float limiteDerecho = 22f;
    public float limiteSuperior = 17f;
    public float limiteInferior = -17f;
    
    private Rigidbody2D rb;
    private MovimientoJugador movimientoJugador;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        movimientoJugador = GetComponent<MovimientoJugador>();
    }
    
    private void LateUpdate()
    {
        // Esta función se ejecuta después del movimiento del jugador
        RestringirPosicion();
    }
    
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
    
    // Visualizar los límites en el editor
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
using UnityEngine;

public class ControladorCamaraBrotato : MonoBehaviour
{
    private GameObject jugador;
    public Grid background;
    
    [Header("Límites del mapa")]
    public bool usarLimites = true;
    public float limiteIzquierdo = -20f;
    public float limiteDerecho = 20f;
    public float limiteSuperior = 15f;
    public float limiteInferior = -15f;

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

    void BuscarJugador()
    {
        jugador = GameObject.FindGameObjectWithTag("Jugador");

        if (jugador == null && Time.timeSinceLevelLoad > 1f) // Solo muestra el mensaje después de 1 segundo
        {
            Debug.LogWarning("No se encontró un GameObject con la etiqueta 'Jugador'. Intentando de nuevo...");
        }
    }
    
    // Para depuración: dibujar los límites del mapa en el editor
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
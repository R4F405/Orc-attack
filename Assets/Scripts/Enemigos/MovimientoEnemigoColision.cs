using UnityEngine;

public class MovimientoEnemigoColision : MonoBehaviour
{
    private GameObject jugador;
    public float velocidad = 2f;

    private bool estaMirandoDerecha = true;

    private void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Jugador"); // Buscar al jugador automáticamente por su tag

        // Verificar si se encontró el jugador para evitar errores
        if (jugador == null)
        {
            Debug.LogError("No se encontró un GameObject con la etiqueta 'Player'.");
        }
    }

    void FixedUpdate()
    {
        if (jugador != null)
        {
            IASeguimiento();
        }

        // Verificar si se encontró el jugador para evitar errores
        if (jugador == null)
        {
        jugador = GameObject.FindGameObjectWithTag("Jugador"); // Buscar al jugador automáticamente por su tag        
        }
    }

    void Update()
    {
        if (jugador != null)
        {
            bool estaJugadorDerecha = transform.position.x < jugador.transform.position.x;
            Girar(estaJugadorDerecha);
        }
    }

    private void IASeguimiento()
    {
        transform.position = Vector2.MoveTowards(transform.position, jugador.transform.position, velocidad * Time.deltaTime);
    }

    private void Girar(bool estaJugadorDerecha)
    {
        if ((estaMirandoDerecha && !estaJugadorDerecha) || (!estaMirandoDerecha && estaJugadorDerecha))
        {
            estaMirandoDerecha = !estaMirandoDerecha;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }
}

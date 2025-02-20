using UnityEngine;

public class MovimientoEnemigoDistancia : MonoBehaviour
{
   private GameObject jugador;
    public float velocidad = 2f;
    public float distanciaMinima = 3f; // Distancia a la que se detiene antes de atacar

    private bool estaMirandoDerecha = true;

    private void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Jugador");

        if (jugador == null)
        {
            Debug.LogError("No se encontró un GameObject con la etiqueta 'Jugador'.");
        }
    }

    void FixedUpdate()
    {
        if (jugador != null)
        {
            IASeguimiento();
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
        float distancia = Vector2.Distance(transform.position, jugador.transform.position);

        if (distancia > distanciaMinima)
        {
            transform.position = Vector2.MoveTowards(transform.position, jugador.transform.position, velocidad * Time.deltaTime);
        }
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

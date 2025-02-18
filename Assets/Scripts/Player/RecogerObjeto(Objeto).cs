using UnityEngine;

public class RecogerObjeto : MonoBehaviour
{
    public float rangoRecoleccion = 1f; // Rango en el que el jugador puede recogerlo
    public int cantidad = 1; // Cantidad que suma este objeto

    private Transform jugador; // Referencia al jugador
    private bool jugadorCerca = false; // Verifica si el jugador está dentro del rango

    private void Update()
    {
        if (jugador != null)
        {
            float distancia = Vector2.Distance(transform.position, jugador.position);
            jugadorCerca = distancia <= rangoRecoleccion; // Detecta si está cerca
        }
    }

    public void IntentarRecoger()
    {
        if (jugadorCerca)
        {
            InventarioJugador inventario = jugador.GetComponent<InventarioJugador>();
            if (inventario != null)
            {
                inventario.AgregarObjeto(cantidad);
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Jugador"))
        {
            jugador = collision.transform; // Guarda la referencia del jugador
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Jugador"))
        {
            jugador = null;
            jugadorCerca = false;
        }
    }
}

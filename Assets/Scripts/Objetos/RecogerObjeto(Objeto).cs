using UnityEngine;

public class RecogerObjeto : MonoBehaviour
{
    public float rangoRecoleccion = 1f; // Rango en el que el jugador puede recogerlo
    public int cantidad = 1; // Cantidad que suma este objeto
    public AudioClip sonidoRecoleccion; // Sonido al recoger la calavera

    private Transform jugador; // Referencia al jugador
    private bool jugadorCerca = false; // Verifica si el jugador está dentro del rango
    private AudioSource audioSource; // Componente de audio

    private void Start()
    {
        // Obtener o crear componente AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

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
                // Reproducir sonido al recoger
                if (sonidoRecoleccion != null)
                {
                    // Usar AudioSource.PlayClipAtPoint para que el sonido se reproduzca aunque se destruya el objeto
                    AudioSource.PlayClipAtPoint(sonidoRecoleccion, transform.position);
                }

                inventario.AgregarCalavera(cantidad);
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

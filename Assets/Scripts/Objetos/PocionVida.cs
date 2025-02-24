using UnityEngine;

public class PocionVida : MonoBehaviour
{
    public float porcentajeCuracion = 20f; // Porcentaje de vida que se restaurará

    private void OnTriggerEnter2D(Collider2D otro)
    {
        if (otro.CompareTag("Jugador"))
        {
            VidaJugador vida = otro.GetComponent<VidaJugador>();
            if (vida != null)
            {
                int cantidadCuracion = Mathf.RoundToInt(vida.ObtenerSaludMaxima() * (porcentajeCuracion / 100f));
                vida.Curar(cantidadCuracion);
            }
            Destroy(gameObject); // Se destruye la poción al recogerla
        }
    }
}

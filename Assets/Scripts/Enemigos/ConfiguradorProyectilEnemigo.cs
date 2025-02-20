using UnityEngine;

public class ConfiguradorProyectilEnemigo : MonoBehaviour
{
    public float velocidad = 5f;
    public float tiempoVida = 3f;

    private int daño;
    private Vector2 direccion;

    public void Configurar(Vector2 nuevaDireccion, int nuevoDaño)
    {
        direccion = nuevaDireccion;
        daño = nuevoDaño;
        GetComponent<Rigidbody2D>().linearVelocity = direccion * velocidad;
    }

    private void Start()
    {
        Destroy(gameObject, tiempoVida);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Jugador"))
        {
            VidaJugador saludJugador = col.GetComponent<VidaJugador>();
            if (saludJugador != null)
            {
                saludJugador.RecibirDaño(daño);
            } 
            Destroy(gameObject);
        } 
    }
}

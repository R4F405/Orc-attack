using UnityEngine;

public class SistemaDanioColisionEnemigo : MonoBehaviour
{
    public float daño = 10f;
    public float tiempoEntreAtaques = 1.5f; 

    private float proximoAtaque = 0f; 

    // Funcion que se ejecuta mientras el jugador está dentro del collider del enemigo
    private void OnTriggerStay2D(Collider2D col) 
    {
        if (col.CompareTag("Jugador"))
        {
            Vida saludJugador = col.GetComponent<Vida>();
            if (saludJugador != null && Time.time >= proximoAtaque) // Verifica el cooldown
            {
                saludJugador.RecibirDaño(daño);
                proximoAtaque = Time.time + tiempoEntreAtaques; // Actualiza el tiempo del próximo ataque
            }
        }
    }
}

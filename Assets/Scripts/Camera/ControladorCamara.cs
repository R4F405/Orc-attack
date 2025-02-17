using UnityEngine;

public class ControladorCamara : MonoBehaviour
{
    private GameObject jugador;

    private void Start()
    {
        BuscarJugador();
    }

    void Update()
    {
        if (jugador == null)
        {
            BuscarJugador(); // Intenta encontrar al jugador si aún no ha sido asignado
            return;
        }

        transform.position = new Vector3(jugador.transform.position.x, jugador.transform.position.y, -3f);
    }

    void BuscarJugador()
    {
        jugador = GameObject.FindGameObjectWithTag("Jugador");

        if (jugador == null && Time.timeSinceLevelLoad > 1f) // Solo muestra el mensaje después de 1 segundo, ya que la camara se instancia mas rapido que el jugador y evitar errores
        {
            Debug.LogWarning("No se encontró un GameObject con la etiqueta 'Jugador'. Intentando de nuevo...");
        }
    }

}

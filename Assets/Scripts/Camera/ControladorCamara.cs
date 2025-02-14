using UnityEngine;

public class ControladorCamara : MonoBehaviour
{

    private GameObject jugador;

     private void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Jugador"); // Buscar al jugador automáticamente por su tag

        // Verificar si se encontró el jugador para evitar errores
        if (jugador == null)
        {
            Debug.LogError("No se encontró un GameObject con la etiqueta 'Player'.");
        }
    }

    //Se llama tras cada frame
    void Update()
    {
        transform.position = new Vector3(jugador.transform.position.x, jugador.transform.position.y,-3f); //Seguimiento de camara a personaje
    }
}

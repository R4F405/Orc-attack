using TMPro;
using UnityEngine;

public class UICalaveras : MonoBehaviour
{
    public TextMeshProUGUI textoCalaveras; // Texto donde se mostrará la cantidad de calaveras

    private InventarioJugador inventarioJugador;

    private void Start()
    {
        // Intentar obtener el InventarioJugador una vez al inicio
        ObtenerJugador();
    }

    private void Update()
    {
        // Si no se ha encontrado el jugador en Start(), intentar encontrarlo en Update()
        if (inventarioJugador == null)
        {
            ObtenerJugador();
        }

        // Si se encuentra el jugador, actualizar la UI
        if (inventarioJugador != null)
        {
            textoCalaveras.text = inventarioJugador.ObtenerCantidadCalaveras().ToString();
        }
    }

    private void ObtenerJugador()
    {
        // Intentar obtener el jugador con el tag 'Jugador'
        GameObject jugador = GameObject.FindWithTag("Jugador");
        if (jugador != null)
        {
            inventarioJugador = jugador.GetComponent<InventarioJugador>();
        }
    
    }
}

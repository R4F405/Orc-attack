using UnityEngine;

public class VidaJugador : MonoBehaviour
{
    public int saludMaxima = 6;

    private MovimientoJugador movimientoJugador; 
    private int saludActual;
    private Animator animator;

    public void AumentarSaludMaxima(int cantidad)
    {
        saludMaxima += cantidad;
        if (saludActual > saludMaxima)
        {
            saludActual = saludMaxima;  // Ajusta la salud actual si supera la nueva máxima
        }
    }

    private void Start()
    {
        saludActual = saludMaxima; // Inicia con la salud máxima

        movimientoJugador = GetComponent<MovimientoJugador>();
        animator = GetComponent<Animator>(); //Se obtiene el componente 
    }

    public void RecibirDaño(int cantidad)
    {
        saludActual -= cantidad;
        if (saludActual <= 0f)
        {
            Muerte();
        }
    }

     private void Muerte()
    {

        animator.SetBool("muerto", true);

        if (movimientoJugador != null)
        {
            movimientoJugador.enabled = false; // Desactiva el script de movimiento
        }

        // Aquí puedes agregar animaciones o efectos al morir si lo deseas
    }

    public int ObtenerSalud()
    {
        return saludActual;
    }

     public int ObtenerSaludMaxima()
    {
        return saludMaxima;
    }
}

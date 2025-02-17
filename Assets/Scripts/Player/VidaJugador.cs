using UnityEngine;

public class VidaJugador : MonoBehaviour
{
    public float saludMaxima = 6;

    private new Rigidbody2D rigidbody2D; 
    private MovimientoJugador movimientoJugador; 
    private float saludActual;
    private Animator animator;


    private void Start()
    {
        saludActual = saludMaxima; // Inicia con la salud máxima

        rigidbody2D = GetComponent<Rigidbody2D>(); 
        movimientoJugador = GetComponent<MovimientoJugador>();
        animator = GetComponent<Animator>(); //Se obtiene el componente 
    }

    public void RecibirDaño(float cantidad)
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

        // Deshabilita el Rigidbody2D y el script de movimiento para detener el movimiento
        if (rigidbody2D != null)
        {
            rigidbody2D.linearVelocity = Vector2.zero; // Detiene cualquier movimiento actual
        }

        if (movimientoJugador != null)
        {
            movimientoJugador.enabled = false; // Desactiva el script de movimiento
        }

        // Aquí puedes agregar animaciones o efectos al morir si lo deseas
    }

    public float ObtenerSalud()
    {
        return saludActual;
    }

     public float ObtenerSaludMaxima()
    {
        return saludMaxima;
    }
}

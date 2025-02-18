using UnityEngine;

public class VidaEnemigo : MonoBehaviour
{
    public float saludMaxima = 100f;
    private float saludActual;
    private DropObjetoEnemigo dropObjeto; // Referencia al script DropObjeto

    private void Start()
    {
        saludActual = saludMaxima;
        dropObjeto = GetComponent<DropObjetoEnemigo>(); // Obtener el script DropObjeto si está presente
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
        // Llamar a SoltarObjeto si el enemigo tiene el script DropObjeto
        if (dropObjeto != null)
        {
            dropObjeto.SoltarObjeto();
        }

        Destroy(gameObject);
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

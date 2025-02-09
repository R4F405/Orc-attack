using UnityEngine;

public class Health : MonoBehaviour
{
   public float saludMaxima = 100f;
    private float saludActual;

    private void Start()
    {
        saludActual = saludMaxima; // Inicia con la salud máxima
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
        // Aquí puedes agregar animaciones o efectos al morir
        Destroy(gameObject); // Destruye el objeto al morir
    }

    public float ObtenerSalud()
    {
        return saludActual;
    }
}

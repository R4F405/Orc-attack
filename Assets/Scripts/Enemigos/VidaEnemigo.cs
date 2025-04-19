using UnityEngine;

public class VidaEnemigo : MonoBehaviour
{
    public int saludMaxima = 100;

    private int saludActual;
    private DropCalaveras dropObjeto; // Referencia al script DropObjeto
    private BarraExperiencia barraExp;

    private void Start()
    {
        saludActual = saludMaxima;
        dropObjeto = GetComponent<DropCalaveras>(); // Obtener el script DropObjeto si está presente
        barraExp = FindAnyObjectByType<BarraExperiencia>(); // Busca la barra en la escena

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
        // Llamar a SoltarObjeto si el enemigo tiene el script DropObjeto
        if (dropObjeto != null)
        {
            dropObjeto.SoltarObjeto();
        }
        //Aumenta la experiencia si esta bien configurado
        if (barraExp != null)
        {
            barraExp.GanarExperiencia(); // Sumar experiencia al morir
        }

        Destroy(gameObject);
    }

    public int ObtenerSalud()
    {
        return saludActual;
    }

    public int ObtenerSaludMaxima()
    {
        return saludMaxima;
    }
    
    // Método para reiniciar la salud actual al valor máximo
    public void ReiniciarSalud()
    {
        saludActual = saludMaxima;
    }
}

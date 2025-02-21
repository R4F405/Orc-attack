using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BarraExperiencia : MonoBehaviour
{
    public Slider barraExp;
    public TextMeshProUGUI textoNivel; // Texto donde se mostrará el nivel
    public int experienciaActual = 0;
    public int experienciaMaxima = 100;
    public int experienciaPorEnemigo = 10;

    private int nivel = 1;

    void Start()
    {
        // Asegura que la barra comience en la posición correcta
        ActualizarBarra();
    }

    public void GanarExperiencia()
    {
        experienciaActual += experienciaPorEnemigo;

        if (experienciaActual >= experienciaMaxima)
        {
            experienciaActual = 0; // Reinicia la experiencia al subir de nivel
            nivel++;
            textoNivel.text = nivel.ToString();
        }

        ActualizarBarra();
    }

    void ActualizarBarra()
    {
        barraExp.value = (float)experienciaActual / experienciaMaxima;
    }
}

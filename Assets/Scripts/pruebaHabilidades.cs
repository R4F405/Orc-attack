using UnityEngine;

public class pruebaHabilidades : MonoBehaviour
{
    private GestorHabilidades gestorHabilidades;
    
    void Start()
    {
        gestorHabilidades = GetComponent<GestorHabilidades>();
    }
     void Update()
    {
        // Detectar si se presiona la tecla espacio
        if (Input.GetKeyDown(KeyCode.Space))
        {
            gestorHabilidades.aumentarDanioPorPorcentaje = 10; // Aumentar el daño en un 10%
            gestorHabilidades.AplicarHabilidades();
        }
    }

}

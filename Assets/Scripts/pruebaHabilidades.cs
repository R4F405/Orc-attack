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
        if (Input.GetKeyDown(KeyCode.Space)) {
            gestorHabilidades.aumentarProbabilidadCritico = 20;
            gestorHabilidades.AplicarHabilidades();
        }
        
        if (Input.GetKeyDown(KeyCode.V)) {
            gestorHabilidades.restarSegundosGenerarCajas = 2f;
            gestorHabilidades.AplicarHabilidades();
        }
   }
}

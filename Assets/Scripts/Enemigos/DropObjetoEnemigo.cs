using UnityEngine;

public class DropObjetoEnemigo : MonoBehaviour
{
    public GameObject objetoSoltar; // Prefab del objeto que se soltará
    public string capaObjeto = "Default"; // Nombre de la capa para el objeto dropeado

    public void SoltarObjeto()
    {
        if (objetoSoltar != null)
        {
            // Instanciar el objeto en la posición del enemigo
            GameObject objetoCreado = Instantiate(objetoSoltar, transform.position, Quaternion.identity);
            
            // Asignar la capa correcta
            objetoCreado.layer = LayerMask.NameToLayer(capaObjeto);
        }
    }
}

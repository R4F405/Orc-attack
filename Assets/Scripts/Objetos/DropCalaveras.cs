using UnityEngine;

/// <summary>
/// Controla la generación de calaveras al eliminar enemigos.
/// </summary>
/// <remarks>
/// Esta clase se encarga de instanciar objetos recolectables como calaveras 
/// en la posición donde muere un enemigo u otro objeto que los suelte.
/// </remarks>
public class DropCalaveras : MonoBehaviour
{
    /// <summary>
    /// Prefab del objeto que se soltará al morir el enemigo.
    /// </summary>
    public GameObject objetoSoltar; // Prefab del objeto que se soltará
    
    /// <summary>
    /// Nombre de la capa a la que se asignará el objeto generado.
    /// </summary>
    public string capaObjeto = "Calavera"; // Nombre de la capa para el objeto dropeado

    /// <summary>
    /// Instancia el objeto recolectable en la posición del enemigo.
    /// </summary>
    /// <remarks>
    /// Este método se llama cuando un enemigo es eliminado para generar
    /// el recurso en su ubicación y asignarle la capa correcta.
    /// </remarks>
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

using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform target;

    //Se llama tras cada frame
    void Update()
    {
        transform.position = new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z); //Seguimiento de camara a personaje
    }
}

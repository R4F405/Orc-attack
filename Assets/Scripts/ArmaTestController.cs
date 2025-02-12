using UnityEngine;

public class ArmaTestController : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetTrigger("atacar"); // Activar la animación al inicio
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Tecla espacio presionada");
            animator.SetTrigger("atacar");
        }
    }
}
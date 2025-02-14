using UnityEngine;

public class MovimientoJugador : MonoBehaviour 
{

    public float velocity = 5f;
    public Vector2 direction { get; private set; } // Es publico, pero solo se puede modificar dentro de PlayerMovement
    
    private Rigidbody2D rigidBody;

    //Se llama al empezar
    private void Start() 
    {   
        rigidBody = GetComponent<Rigidbody2D>(); //Se obtiene el componente 
    }

    //Se llama 50/s
    private void FixedUpdate() 
    {
        //Crea el movimiento segun direccion y velocidad
        Vector2 targetPosition = rigidBody.position + direction * velocity * Time.fixedDeltaTime; 
        rigidBody.MovePosition(targetPosition);
    }

    //Se llama tras cada frame
    private void Update() 
    {
        Movimiento();
    }

    //Funcion de movimiento
    private void Movimiento() 
    {
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized; //Obtiene la direccion del movimiento atraves de WASD o flechas
    }

    

}

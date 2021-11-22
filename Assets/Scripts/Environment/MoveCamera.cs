using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    // Rotation autour de l'axe x
    private float yaw = 0f;
    // Rotation autour de l'axe y
    private float pitch = 0f;

    // Vitesse linéaire de la caméra
    public float speed = 2f;
    // Vitesse angulaire horizontale de la souris 
    public float horizontalMouseSpeed = 1.5f;
    // Vitesse angulaire verticale de la souris 
    public float verticalMouseSpeed = 1.5f;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Up")) 
            transform.Translate(Vector3.forward * speed);
        if (Input.GetButton("Down")) 
            transform.Translate(Vector3.back * speed);
        if (Input.GetButton("Right")) 
            transform.Translate(Vector3.right * speed);
        if (Input.GetButton("Left")) 
            transform.Translate(Vector3.left * speed);




        // On calcule l'angle de rotation en fonction de l'angle de déplacement de la souris et de la vitesse angulaire, puis on donne tout ça à la transformation de l'objet courant
        yaw += horizontalMouseSpeed * Input.GetAxis("Mouse X");
        pitch -= verticalMouseSpeed * Input.GetAxis("Mouse Y");
        transform.eulerAngles = new Vector3(pitch, yaw, 0);
    }
}

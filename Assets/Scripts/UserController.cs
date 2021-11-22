using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     <author>Cyril Dubos</author>
///     Class <c>UserController</c> allows users to control the camera with a mouse and a keyboard.
/// </summary>
public class UserController : MonoBehaviour
{
    /// <summary>
    ///     <c>speed</c> models the speed of the user in the scene.
    /// </summary>
    public float speed = 2f;

    /// <summary>
    ///     <c>mouseSpeed</c> models the sensibility of the user mouse.
    /// </summary>
    public float mouseSpeed = 1.5f;

    /// <summary>
    ///     <c>camera</c> is the <c>Camera</c> object that will be moved by the controller.
    /// </summary>
#if UNITY_EDITOR
    public new GameObject camera;
#else
    public GameObject camera;
#endif

    public void Update()
    {
        if (Menu.PauseMenu.Paused)
            return;
        UpdatePosition();
        UpdateRotation();
    }

    /// <summary>
    ///    This method changes the position of the user in the scene with keyboard controls.
    /// </summary>
    private void UpdatePosition()
    {
        transform.Translate(Vector3.forward * Input.GetAxis("Vertical") * speed * Time.deltaTime);
        transform.Translate(Vector3.right * Input.GetAxis("Horizontal") * speed * Time.deltaTime);

        if (Input.GetKey(KeyCode.Space))
            transform.Translate(Vector3.up * speed * Time.deltaTime);
        if (Input.GetKey(KeyCode.LeftControl) && !(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
            transform.Translate(Vector3.down * speed * Time.deltaTime);
    }

    /// <summary>
    ///    This method rotates the camera with mouse controls.
    /// </summary>
    private void UpdateRotation()
    {
        float yaw = transform.eulerAngles.y + mouseSpeed * Input.GetAxis("Mouse X");
        float pitch = camera.transform.eulerAngles.x - mouseSpeed * Input.GetAxis("Mouse Y");

        transform.eulerAngles = new Vector3(0, yaw, 0);
        camera.transform.eulerAngles = new Vector3(pitch, yaw, 0);
    }
}

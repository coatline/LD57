using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{

    //public float lookSensitivity = 2f;
    //public float maxLookAngle = 80f;

    [SerializeField] float cameraPitchLimit = 80;
    [SerializeField] float lookSensitivity = 1;
    [SerializeField] float smoothing = 1;
    [SerializeField] Transform playerMesh;
    [SerializeField] Camera cam;

    Vector2 currentLookingPosition;
    Vector2 smoothedVelocity;
    Vector2 inputValues;


    public void SetCurrentLookingPosition(Vector2 lookingPosition)
    {
        currentLookingPosition = lookingPosition;
    }

    public void SetInputValues(Vector2 inputValues)
    {
        //print($"in values {inputValues} currentLookingPos {currentLookingPosition}");
        inputValues = Vector2.Scale(inputValues, new Vector2(smoothing * lookSensitivity, smoothing * lookSensitivity));
        //print($"after scaling {inputValues}");

        smoothedVelocity.x = Mathf.Lerp(smoothedVelocity.x, inputValues.x, 1 / smoothing);
        smoothedVelocity.y = Mathf.Lerp(smoothedVelocity.y, inputValues.y, 1 / smoothing);

        currentLookingPosition += smoothedVelocity;
        //print($"Now currentLookingPos {currentLookingPosition}");

        // Clamp the vertical rotation (pitch)
        currentLookingPosition.y = Mathf.Clamp(currentLookingPosition.y, -cameraPitchLimit, cameraPitchLimit);

        cam.transform.localRotation = Quaternion.AngleAxis(-currentLookingPosition.y, Vector3.right);
        playerMesh.localRotation = Quaternion.AngleAxis(currentLookingPosition.x, transform.up);
    }

    private void FixedUpdate()
    {

    }
}

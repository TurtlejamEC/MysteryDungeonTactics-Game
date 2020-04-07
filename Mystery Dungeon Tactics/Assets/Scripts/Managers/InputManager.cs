using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 1) {
            Touch touch = Input.GetTouch(0);
            float finalAngleOfElevation = Mathf.Clamp(CameraManager.angleOfElevation - touch.deltaPosition.y * Time.deltaTime, 0, 90);
            float finalYRotation = (((CameraManager.yRotation - touch.deltaPosition.x * 3 * Time.deltaTime) % 360) + 360) % 360;
            CameraManager.SetCamera(CameraManager.targetPos, finalAngleOfElevation, CameraManager.radius, finalYRotation);
            
        } else if (Input.touchCount == 2) {
            // Store both touches.
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            if (touch0.phase == TouchPhase.Moved && touch1.phase == TouchPhase.Moved) {
                if (Mathf.Sign(touch0.deltaPosition.x) == Mathf.Sign(touch1.deltaPosition.x) &&
                Mathf.Sign(touch0.deltaPosition.y) == Mathf.Sign(touch1.deltaPosition.y)) { // Pan
                Vector2 mean = new Vector2((touch0.deltaPosition.x + touch1.deltaPosition.x / 2f),
                    (touch0.deltaPosition.y + touch1.deltaPosition.y / 2f));

                float factor = 0.03f * CameraManager.radius * Time.deltaTime;
                float rotatedY = mean.x * Mathf.Cos(CameraManager.yRotation * Mathf.Deg2Rad) -
                                 mean.y * Mathf.Sin(CameraManager.yRotation * Mathf.Deg2Rad);
                float rotatedX = mean.x * Mathf.Sin(CameraManager.yRotation * Mathf.Deg2Rad) +
                                 mean.y * Mathf.Cos(CameraManager.yRotation * Mathf.Deg2Rad);
                Vector3 finalTargetPos = CameraManager.targetPos - new Vector3(-rotatedX * factor, 0f, rotatedY * factor);
                finalTargetPos.x = Mathf.Clamp(finalTargetPos.x, 0, 30);
                finalTargetPos.z = Mathf.Clamp(finalTargetPos.z, 0, 30);
                CameraManager.SetCamera(finalTargetPos, CameraManager.angleOfElevation, CameraManager.radius, CameraManager.yRotation);
                } else { // Zoom
                    // Find the position in the previous frame of each touch.
                    Vector2 touch0PrevPos = touch0.position - touch0.deltaPosition;
                    Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;

                    // Find the magnitude of the vector (the distance) between the touches in each frame.
                    float prevTouchDeltaMag = (touch0PrevPos - touch1PrevPos).magnitude;
                    float touchDeltaMag = (touch0.position - touch1.position).magnitude;

                    // Find the difference in the distances between each frame.
                    float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

                    float finalRadius = Mathf.Clamp(CameraManager.radius + deltaMagnitudeDiff * 0.2f * Time.deltaTime, 1, 10);
                    CameraManager.SetCamera(CameraManager.targetPos, CameraManager.angleOfElevation, finalRadius, CameraManager.yRotation);
                }
            }
        }
    }
}

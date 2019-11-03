using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

public class CameraManager : MonoBehaviour {
    public static Camera mainCamera;

    public static Vector3 targetPos;
    public static float angleOfElevation = 30;
    public static float radius = 3.5f;
    public static float yRotation = 270;

    public void Awake() {
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
    }

    public static void SetCamera(Vector3 newTargetPos, float newAngleOfElevation, float newRadius, float newYRotation) {
        targetPos = newTargetPos;
        angleOfElevation = newAngleOfElevation;
        radius = newRadius;
        yRotation = newYRotation;
        
        float yPos = radius * Mathf.Sin(angleOfElevation * Mathf.Deg2Rad);
        float baseMagnitude = radius * Mathf.Cos(angleOfElevation * Mathf.Deg2Rad);
        
        float xPos = baseMagnitude * Mathf.Cos(yRotation * Mathf.Deg2Rad);
        float zPos = baseMagnitude * Mathf.Sin(yRotation * Mathf.Deg2Rad);
        
        mainCamera.transform.position = new Vector3(xPos, yPos, zPos) + targetPos;
        mainCamera.transform.LookAt(targetPos);
    }

    public void FollowNewCharacter(int characterId) {
/*        Character characterInfo = CharacterManager.ActiveCharacters[characterId];
        mainCamera.transform.rotation = Quaternion.Euler(angleOfDepression, 0, 0);
        mainCamera.transform.position = new Vector3(characterInfo.Position.X, cameraHeight, characterInfo.Position.Z - ((cameraHeight / Mathf.Tan(angleOfDepression * Mathf.Deg2Rad))));*/
        Character characterInfo = CharacterManager.ActiveCharacters[characterId];
        targetPos = new Vector3(characterInfo.Position.X, 0, characterInfo.Position.Z);
        SetCamera(targetPos, 30, 3.5f, 270);
    }
}
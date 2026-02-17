using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.XR.ARFoundation;

public class ARGPSManager : MonoBehaviour {

    [Header("GPS Objetivo")]
    public double targetLatitude = 37.19221451704046;
    public double targetLongitude = -3.6166955963868173;
    public float detectionDistance = 10f;
    private bool instanciado = false;
    public GameObject modelo;
    public GameObject spawnedObject;
    public ARRaycastManager raycastManager;

    [Header("UI")]
    public Text gpsText;

    [Header("Objeto Pokémon")]
    public GameObject targetObject;

    // Referencia a tu clase de mensajes
    [Header("Mensajes")]
    public UIManager messageManager;

    private bool objectDetected = false;

    void Start() {

        if (!Input.location.isEnabledByUser) return;
        gpsText.text = "Ejecuta Start";
        Input.location.Start();
        Input.compass.enabled = true;
        /*
        if (GPSTracker.Instance != null) {

            GPSTracker.Instance.OnGPSUpdated += UpdateGPSData;
            messageManager?.MostrarMensaje("GPS iniciado. Buscando Pokémon...");
        }
        else {
            messageManager?.MostrarMensaje("GPSTracker no encontrado");
        }
        */
    }
    void UpdateGPSData(LocationInfo gpsData) {

        if (gpsText != null) {

            gpsText.text = $"GPS:\nLat: {gpsData.latitude:F6}\nLon: {gpsData.longitude:F6}\nAlt: {gpsData.altitude:F1}m";
        }

        float distance = HaversineDistance(gpsData.latitude, gpsData.longitude, targetLatitude, targetLongitude);

        if (gpsText != null) {

            gpsText.text += $"\nDistancia: {distance:F1}m";
        }
        CheckTargetProximity(distance);
    }
    void CheckTargetProximity(float distance) {

        if (distance < detectionDistance) {

            if (!objectDetected && targetObject != null) {

                targetObject.SetActive(true);
                targetObject.transform.position = transform.position + transform.forward * 2f;
                objectDetected = true;
                messageManager?.MostrarMensaje("¡Pokémon encontrado cerca!");
            }
        }
        else if (objectDetected) {

            objectDetected = false;
            if (targetObject != null) targetObject.SetActive(false);
            messageManager?.MostrarMensaje("Sigue buscando...");
        }
    }
    float HaversineDistance(double lat1, double lon1, double lat2, double lon2) {

        const float R = 6371000f; // Radio Tierra en metros

        float dLat = (float)(lat2 - lat1) * Mathf.Deg2Rad;
        float dLon = (float)(lon2 - lon1) * Mathf.Deg2Rad;
        float lat1Rad = (float)lat1 * Mathf.Deg2Rad;
        float lat2Rad = (float)lat2 * Mathf.Deg2Rad;

        float a = Mathf.Sin(dLat / 2) * Mathf.Sin(dLat / 2) +
                  Mathf.Cos(lat1Rad) * Mathf.Cos(lat2Rad) *
                  Mathf.Sin(dLon / 2) * Mathf.Sin(dLon / 2);

        float c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));
        return R * c;
    }
    void OnDestroy() {

        if (GPSTracker.Instance != null)
            GPSTracker.Instance.OnGPSUpdated -= UpdateGPSData;
    }
}
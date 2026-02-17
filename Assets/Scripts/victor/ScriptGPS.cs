using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ScriptGPS : MonoBehaviour {

    private double targetLat = 37.19220614091147;
    private double targetLon = -3.616657461975105;

    public float currentLat;

    public float currentLon;


    public float detectionRadius = 60f;

    private bool isSpawned = false;
    public GameObject prefab;

    private GameObject spawnedObject;
    public ARRaycastManager raycastManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {

        if (!Input.location.isEnabledByUser) return;
        UIManager.Instance.MostrarMensaje("Ejecuta Start");
        Input.location.Start();
        Input.compass.enabled = true;
    }

    // Update is called once per frame
    void Update() {

        UIManager.Instance.MostrarMensaje(Input.location.status.ToString());
        if (Input.location.status == LocationServiceStatus.Running) {

            currentLat = Input.location.lastData.latitude;
            currentLon = Input.location.lastData.longitude;

            double distance = CalculateDistance(currentLat, currentLon, targetLat, targetLon);
            if (distance <= detectionRadius && !isSpawned) {

                UIManager.Instance.MostrarMensaje("¡Has encontrado un objeto! Iniciando RA...");
                //Aqui se activa el modelo o se cambia de escena
                SpawnObjectInAR_Plane();
            }
            else {
                UIManager.Instance.MostrarMensaje($"GPS Móvil: {currentLat}, {currentLon}, Target (Google): {targetLat}, {targetLon} Distancia: {distance}");
            }
        }
    }
    //Fórmula de Haversine para calcular distancia en metros
    double CalculateDistance(double lat1, double lon1, double lat2, double lon2) {

        double R = 6371000;

        double dLat = ToRadians(lat2 - lat1);
        double dLon = ToRadians(lon2 - lon1);

        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        return R * c;
    }
    double ToRadians(double degrees) => degrees * Math.PI / 180;

    void SpawnObjectInAR() {
        Vector3 spawnPosition = Camera.main.transform.position + Camera.main.transform.forward * 2f;

        spawnPosition.y = -1.5f;

        isSpawned = true;
    }
    void SpawnObjectInAR_Plane() {

        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        if (raycastManager.Raycast(
           new Vector2(Screen.width / 2, Screen.height / 2),
           hits,
           UnityEngine.XR.ARSubsystems.TrackableType.Planes)) {

            Instantiate(prefab, hits[0].pose.position, Quaternion.identity);
        }
    }
}
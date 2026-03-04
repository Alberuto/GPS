using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class MHGPS : MonoBehaviour {

    private double targetLat = 37.19220614091147;
    private double targetLon = -3.616657461975105;
    public float currentLat, currentLon;
    public float detectionRadius = 60f;
    private bool isSpawned = false;
    public GameObject prefab;
    public ARRaycastManager raycastManager;

    void Start() {
#if UNITY_ANDROID
        Permission.RequestUserPermission(Permission.FineLocation);
#endif
        StartCoroutine(IniciarGPS());
    }
    IEnumerator IniciarGPS() {

        if (!Input.location.isEnabledByUser) {
            MHUIManager.Instance.MostrarMensaje("GPS desactivado");
            yield break;
        }
        Input.location.Start();
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) {
            yield return new WaitForSeconds(1);
            maxWait--;
        }
        Input.compass.enabled = true;
    }
    void Update() {
        MHUIManager.Instance.MostrarMensaje(Input.location.status.ToString());

        if (Input.location.status == LocationServiceStatus.Running)  {

            currentLat = Input.location.lastData.latitude;
            currentLon = Input.location.lastData.longitude;

            var target = MHMonsterManager.Instance?.GetNextMonster();
            if (target != null)
            {
                double distance = CalculateDistance(currentLat, currentLon, target.lat, target.lon);
                MHUIManager.Instance.MostrarMensaje(
                    $"Siguiente: {target.name}\nDist: {distance:F0}m");

                if (distance <= detectionRadius && !isSpawned)
                {
                    SpawnObjectInAR_Plane();
                }
            }
        }
    }
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
    void SpawnObjectInAR_Plane() {
        if (isSpawned) return;

        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        if (raycastManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), hits, TrackableType.Planes))
        {
            Instantiate(prefab, hits[0].pose.position, Quaternion.identity);
            isSpawned = true;
        }
    }
}
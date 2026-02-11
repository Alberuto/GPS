/*using UnityEngine;
using System;
using System.Collections;

public class GPSTracker : MonoBehaviour {

    public static GPSTracker Instance;

    public event Action<LocationInfo> OnGPSUpdated;

    [Header("Configuración GPS")]
    public float desiredAccuracy = 1f;
    public float updateDistance = 1f;

    private bool gpsStarted = false;

    void Awake() {

        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    void Start() {
        StartCoroutine(StartGPS());
    }
    public IEnumerator StartGPS() {
        // Permisos Android
#if UNITY_ANDROID && !UNITY_EDITOR
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            yield return new WaitUntil(() => Permission.HasUserAuthorizedPermission(Permission.FineLocation));
        }
#endif
        if (!Input.location.isEnabledByUser)
            yield break;

        Input.location.Start(desiredAccuracy, updateDistance);

        int maxWait = 20;

        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) {
            yield return new WaitForSeconds(1);
            maxWait--;
        }
        if (maxWait < 1 || Input.location.status == LocationServiceStatus.Failed)
            yield break;

        gpsStarted = true;
    }
    void Update() {

        if (gpsStarted && Input.location.status == LocationServiceStatus.Running) {
            OnGPSUpdated?.Invoke(Input.location.lastData);
        }
    }
    void OnApplicationPause(bool pause) {

        if (gpsStarted && pause) Input.location.Stop();
        else if (gpsStarted && !pause) Input.location.Start(desiredAccuracy, updateDistance);
    }
    void OnDestroy() {
        Input.location.Stop();
    }
    public bool IsGPSRunning() => gpsStarted && Input.location.status == LocationServiceStatus.Running;
}*/
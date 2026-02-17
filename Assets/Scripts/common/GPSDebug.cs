using System.Collections;
using UnityEngine;
using TMPro;

public class GPSDebug : MonoBehaviour {

    public TextMeshProUGUI statusText;
    void Start() {
        StartCoroutine(GPSRoutine());
    }
    IEnumerator GPSRoutine() {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.FineLocation))
        {
            UnityEngine.Android.Permission.RequestUserPermission(UnityEngine.Android.Permission.FineLocation);
            yield return new WaitForSeconds(1);
        }
#endif

        if (!Input.location.isEnabledByUser) {

            statusText.text = "Location desactivada en el móvil";
            yield break;
        }

        Input.location.Start();

        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)  {

            statusText.text = "Inicializando GPS...";
            yield return new WaitForSeconds(1);
            maxWait--;
        }
        if (maxWait < 1) {
            statusText.text = "Timeout inicializando GPS";
            yield break;
        }
        if (Input.location.status == LocationServiceStatus.Failed) {

            statusText.text = "No se pudo obtener la ubicación";
            yield break;
        }
        // Si llega aquí, está RUNNING
        statusText.text = "GPS RUNNING";

        while (true) {
            statusText.text =
                $"Estado: {Input.location.status}\n" +
                $"Lat: {Input.location.lastData.latitude}\n" +
                $"Lon: {Input.location.lastData.longitude}";
            yield return new WaitForSeconds(1);
        }
    }
}
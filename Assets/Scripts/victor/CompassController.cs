using UnityEngine;

public class CompassController : MonoBehaviour {

    public RectTransform arrowUI;

    public ScriptGPS gpsManager;

    public double prefabLat;

    public double prefabLon;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {

        Input.compass.enabled = true;
    }

    // Update is called once per frame
    void Update() {

        float heading = Input.compass.trueHeading;
        float bearing = CalculateBearing(
            gpsManager.GetComponent<ScriptGPS>().currentLat,
            gpsManager.GetComponent<ScriptGPS>().currentLon,
            prefabLat,
            prefabLon);
        float angle = heading - bearing;
        arrowUI.localRotation = Quaternion.Euler(0, 0, angle);
    }

    float CalculateBearing(double lat1, double lon1, double lat2, double lon2) {

        double dLon = (lon2 - lon1) * Mathf.Deg2Rad;
        lat1 *= Mathf.Deg2Rad;
        lat2 *= Mathf.Deg2Rad;

        double y = Mathf.Sin((float)dLon) * Mathf.Cos((float)lat2);
        double x = Mathf.Cos((float)lat1) * Mathf.Sin((float)lat2) -
                   Mathf.Sin((float)lat1) * Mathf.Cos((float)lat2) *
                   Mathf.Cos((float)dLon);
        double brng = Mathf.Atan2((float)y, (float)x);
        brng = brng * Mathf.Rad2Deg;
        return (float)((brng + 360) % 360);
    }
}
using UnityEngine;
using UnityEngine.UI;

public class MHCompassController : MonoBehaviour {

    public RectTransform arrowUI;
    public MHGPS gpsManager;
    public double prefabLat = 37.192206;
    public double prefabLon = -3.616657;

    void Start() {
        Input.compass.enabled = true;
    }

    void Update() {
        if (gpsManager == null) return;

        float heading = Input.compass.trueHeading;
        float bearing = CalculateBearing(gpsManager.currentLat, gpsManager.currentLon, prefabLat, prefabLon);
        float angle = heading - bearing;
        arrowUI.localRotation = Quaternion.Euler(0, 0, angle);
    }

    float CalculateBearing(double lat1, double lon1, double lat2, double lon2) {
        double dLon = (lon2 - lon1);
        lat1 *= Mathf.Deg2Rad; lat2 *= Mathf.Deg2Rad;

        double y = Mathf.Sin((float)(dLon * Mathf.Deg2Rad)) * Mathf.Cos((float)lat2);
        double x = Mathf.Cos((float)lat1) * Mathf.Sin((float)lat2) -
                   Mathf.Sin((float)lat1) * Mathf.Cos((float)lat2) * Mathf.Cos((float)(dLon * Mathf.Deg2Rad));
        double brng = Mathf.Atan2((float)y, (float)x);
        return (float)((brng * Mathf.Rad2Deg + 360) % 360);
    }
}
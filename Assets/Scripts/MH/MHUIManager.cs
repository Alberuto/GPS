using TMPro;
using UnityEngine;

public class MHUIManager : MonoBehaviour {

    public static MHUIManager Instance;
    public TextMeshProUGUI texto;

    void Awake() { Instance = this; }

    public void MostrarMensaje(string mensaje) {
        texto.text = mensaje;
    }
}
using TMPro;
using UnityEngine;


public class UIManager2 : MonoBehaviour {
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public UIManager2 Instance;
    public TMP_Text textoGameOver;

    private void Awake() {

        Instance = this;
        textoGameOver.text = "";
    }
    public void MostrarGameOver() {

        textoGameOver.text = "Choque";
    }
    public void MostrarYAW(float yaw) {

        textoGameOver.text = yaw.ToString();

    }
    public void MostrarMensaje(string msj) {

        textoGameOver.text = msj;
    }
}
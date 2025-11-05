using UnityEngine;

[System.Serializable]
public class Pregunta
{
    public string texto;
    public string[] opciones;
    public int indiceCorrecta;
    public Pregunta siguiente; // nodo siguiente

    public Pregunta(string texto, string[] opciones, int indiceCorrecta)
    {
        this.texto = texto;
        this.opciones = opciones;
        this.indiceCorrecta = indiceCorrecta;
    }
}

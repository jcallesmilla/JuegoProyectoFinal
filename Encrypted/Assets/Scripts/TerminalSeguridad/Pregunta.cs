using System;

[Serializable]
public class Pregunta
{
    public string textoPregunta;
    public string[] opciones;
    public int respuestaCorrecta; // índice de la opción correcta
    public Pregunta siguiente; // para lista enlazada circular

    public Pregunta(string texto, string[] opciones, int indiceCorrecto)
    {
        textoPregunta = texto;
        this.opciones = opciones;
        respuestaCorrecta = indiceCorrecto;
        siguiente = null;
    }
}

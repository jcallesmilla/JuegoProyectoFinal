using UnityEngine;

public class ListaPreguntas
{
    private Pregunta cabeza;

    public void Agregar(Pregunta nueva)
    {
        if (cabeza == null)
        {
            cabeza = nueva;
            cabeza.siguiente = cabeza;
            return;
        }

        Pregunta temp = cabeza;
        while (temp.siguiente != cabeza)
            temp = temp.siguiente;

        temp.siguiente = nueva;
        nueva.siguiente = cabeza;
    }

    public Pregunta ObtenerPreguntaAleatoria()
    {
        if (cabeza == null) return null;
        int cantidad = Contar();
        int indice = Random.Range(0, cantidad);
        Pregunta actual = cabeza;
        for (int i = 0; i < indice; i++)
            actual = actual.siguiente;
        return actual;
    }

    private int Contar()
    {
        if (cabeza == null) return 0;
        int count = 1;
        Pregunta temp = cabeza;
        while (temp.siguiente != cabeza)
        {
            count++;
            temp = temp.siguiente;
        }
        return count;
    }
}

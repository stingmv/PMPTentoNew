using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roulette : MonoBehaviour
{

    public RectTransform ruletaTransform; // RectTransform de la ruleta
    public float velocidadRotacion = 5f; // Ajusta la velocidad de rotación
    public float angulo;

    void Update()
    {
        // Obtén la posición del mouse en relación con el centro de la ruleta
        Vector2 posicionMouse = Input.mousePosition - ruletaTransform.position;

        // Calcula el ángulo de rotación en radianes
        angulo = Mathf.Atan2(posicionMouse.y, posicionMouse.x) * Mathf.Rad2Deg;

        // Crea la rotación objetivo basada en el ángulo calculado
        Quaternion rotacionObjetivo = Quaternion.Euler(0f, 0f, angulo);

        // Suavemente rota hacia la rotación objetivo
        ruletaTransform.rotation = Quaternion.Slerp(ruletaTransform.rotation, rotacionObjetivo, velocidadRotacion * Time.deltaTime);
    }
}

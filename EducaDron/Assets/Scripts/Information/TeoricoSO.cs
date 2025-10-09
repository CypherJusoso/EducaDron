using UnityEngine;

[CreateAssetMenu(fileName = "TeoricoSO", menuName = "Scriptable Objects/TeoricoSO")]
public class TeoricoSO : ScriptableObject
{
    [Header("Identificación")]
    public int nivelTeorico = 1;     // 1, 2 o 3

    [Header("Contenido")]
    public string titulo;            // Para Tittle_Info (TMP)
    [TextArea(6, 20)]
    public string contenido;         // Para I_ContentArea (TMP)
}
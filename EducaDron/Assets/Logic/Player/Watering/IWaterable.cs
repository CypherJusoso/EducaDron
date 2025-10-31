using UnityEngine;

public interface IWaterable
{
    /// <summary>
    /// Aplica una unidad de riego al objeto,
    /// avanzando su estado hacia "regado"
    /// </summary>
    void ProcessWatering();

    /// <summary>
    /// Indica si el objeto ya fue regado
    /// </summary>
    bool IsWatered {  get; }
}

using UnityEngine;

public interface IWaterable
{
    void ProcessWatering();
    bool IsWatered {  get; }
}

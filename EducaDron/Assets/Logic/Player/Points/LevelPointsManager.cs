using UnityEngine;

public class LevelPointsManager : MonoBehaviour
{

    [SerializeField] DroneStatusAndCollision droneLife;
    [SerializeField] Timer timer;
    [SerializeField] PhotoCapture photoCapture;
    [SerializeField] Riego riego;
    [SerializeField] Watering watering;

    int points = 70;

    private void Awake()
    {
        if (DataManager.instance != null)
        {
            DataManager.instance.ResetPoints();
        }
        else
        {
            Debug.LogWarning("DataManager no encontrado.");
        }
    }

    public void CalculatePointsLevel1()
    {
        int penalizacionHp = CalculateHp();
        int penalizacionTiempo = CalculateTimer();

        CalculatePhotos();

        if (points < 0) { points = 0; }

        DataManager.instance.levelPoints = points;
        Debug.Log($"Puntos finales nivel 1: {points} (Vida: -{penalizacionHp}, Tiempo: -{penalizacionTiempo})");
        DataManager.instance.currentLvl = 1;
    }

    public void CalculatePointsLevel2() 
    {
        int penalizacionHp = CalculateHp();
        int penalizacionTiempo = CalculateTimer();
        int penalizacionAgua = CalculateWaterLevel2();

        if (DataManager.instance != null)
        {
            DataManager.instance.currentLvl = 2;
            DataManager.instance.levelPoints = points;
            DataManager.instance.currentLvl = 3;
        }
        else
        {
            Debug.LogWarning("DataManager no instanciado.");
        }
        Debug.Log($"Puntos finales nivel 3: {points} (Vida: -{penalizacionHp}, Tiempo: -{penalizacionTiempo}, Riego: - {penalizacionAgua})");
    }

    public void CalculatePointsLevel3()
    {
        int penalizacionHp = CalculateHp();
        int penalizacionTiempo = CalculateTimer();
        int penalizacionAgua = CalculateWater();

        if (points < 0) { points = 0; }

        DataManager.instance.levelPoints = points;
        Debug.Log($"Puntos finales nivel 3: {points} (Vida: -{penalizacionHp}, Tiempo: -{penalizacionTiempo}, Riego: - {penalizacionAgua})");
        DataManager.instance.currentLvl = 3;
    }

    int CalculateHp()
    {
        int vidaPerdida = (int)(100 - droneLife.droneLife);
        int exceso = vidaPerdida - 40;
        int penalizacionHp = 0;

        if (exceso > 0)
        {
            penalizacionHp = 20;
            if (exceso > 10)
            {
                penalizacionHp += Mathf.FloorToInt((exceso - 10) / 5f);
                Debug.Log("Se activo la penalizacion chica de vida");

            }
            Debug.Log("Se activo la penalizacion grande de vida");

        }
        points -= penalizacionHp;
        return penalizacionHp;
    }
    int CalculateTimer()
    {
        float tiempoRestante = timer.timerDuration;
        float excesoTimer = 180 - tiempoRestante;
        int penalizacionTimer = 0;

        if (excesoTimer > 0)
        {
            penalizacionTimer = 25;

            if (excesoTimer > 10)
            {
                penalizacionTimer += Mathf.FloorToInt((excesoTimer - 10) / 10f);
                Debug.Log("Se activo la penalizacion chica de tiempo");

            }
            Debug.Log("Se activo la penalizacion grande de tiempo");
        }

        points -= penalizacionTimer;
        return penalizacionTimer;
    }

    int CalculateWater()
    {
        int aguaUsada = (int)(300 - riego.aguaActual);
        int excesoAgua = aguaUsada - 150;
        int penalizacionAgua = 0;
        if (excesoAgua > 0)
        {
            penalizacionAgua = 20;
            if (excesoAgua > 10)
            {
                penalizacionAgua += Mathf.FloorToInt((excesoAgua - 10) / 10f);
                Debug.Log("Se activo la penalizacion chica de agua");
            }
            Debug.Log("Se activo la penalizacion grande de agua");

        }

        return penalizacionAgua;
    }

    int CalculateWaterLevel2()
    {
        int aguaUsada = (int)(1000 - watering.aguaActual);
        int excesoAgua = aguaUsada - 700;
        int penalizacionAgua = 0;

        if (excesoAgua > 0)
        {
            penalizacionAgua = 20;
            if(excesoAgua > 10)
            {
                penalizacionAgua += Mathf.FloorToInt((excesoAgua - 10) / 10);
                Debug.Log("Se activo la penalizacion chica de agua");
            }
            Debug.Log("Se activo la penalizacion grande de agua");
        }
        return penalizacionAgua;
    }
    void CalculatePhotos()
    {
        if (photoCapture.actualPhotos > 5)
        {
            int fotosExtra = photoCapture.actualPhotos - 5;
            points -= fotosExtra;
        }
    }


}

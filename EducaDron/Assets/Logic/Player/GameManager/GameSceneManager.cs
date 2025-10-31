using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    string gameOverText = "GameOver";

    ///<summary>
    /// Inicia la rutina <see cref="ReloadLevelRoutine"/> para reiniciar el nivel
    ///</summary>
    public void ReloadLevel()
    {
        StartCoroutine(ReloadLevelRoutine());
    }

    ///<summary>
    ///Inicia la rutina <see cref="GameOverRoutine"/> cuando se perdio un desafio
    ///</summary>
    public void GameOver() 
    {
        StartCoroutine(GameOverRoutine());
    }

    ///<summary>
    ///Espera un segundo y carga la escena de Game Over
    ///</summary>
    IEnumerator GameOverRoutine()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(gameOverText);
    }

    ///<summary>
    ///Espera un segundo y reinicia el nivel actual
    ///</summary>
    IEnumerator ReloadLevelRoutine()
    {
        yield return new WaitForSeconds(1f);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    ///<summary>
    ///Carga la escena del menu principal
    ///</summary>
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    ///<summary>
    ///Carga la escena del cuestionario
    ///</summary>
    public void Quiz()
    {
        SceneManager.LoadScene("Quiz");
    }
}

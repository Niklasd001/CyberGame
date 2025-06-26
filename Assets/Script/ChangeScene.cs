using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CambiaScena : MonoBehaviour
{
    [SerializeField] private string nomeScena;

    public void CaricaScena()
    {
        if (!string.IsNullOrEmpty(nomeScena))
        {
            Debug.Log("Cambio scena in corso: " + nomeScena);
            
            StartCoroutine(CaricaSceneAsync());
        }
        else
        {
            Debug.LogWarning("Nome scena non impostato!");
        }
    }

    private IEnumerator CaricaSceneAsync()
    {
        AsyncOperation operazione = SceneManager.LoadSceneAsync(nomeScena);

        while (!operazione.isDone)
        {
            // Se vuoi, puoi stampare il progresso:
            Debug.Log("Progress: " + (operazione.progress * 100f) + "%");
            yield return null;
        }
    }
}

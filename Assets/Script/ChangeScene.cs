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
            SceneManager.LoadScene(nomeScena);
        }
        else
        {
            Debug.LogWarning("Nome scena non impostato!");
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class LevelRestarter:MonoBehaviour
    {
        public void RestartLevel()
        {         
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}

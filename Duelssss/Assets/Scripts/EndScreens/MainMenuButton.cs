using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
    
    public class BackToMenu : MonoBehaviour {
    
    	public void GoMainMenu() {
    		SceneManager.LoadScene("MainMenu"); // loads menu scene
    	}
    
    }
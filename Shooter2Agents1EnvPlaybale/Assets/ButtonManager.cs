using UnityEngine;
using UnityEngine.UI;
using Unity.MLAgents.Policies;
using Unity.MLAgents;

public class ButtonManager : MonoBehaviour
{
    public Button kiVsKiButton;
    public Button controlGreenButton;
    public Button controlRedButton;

    public GameObject greenAgent;
    public GameObject redAgent;

    void Start()
    {
        // Button Click Listeners hinzufügen
        kiVsKiButton.onClick.AddListener(OnKiVsKi);
        controlGreenButton.onClick.AddListener(OnControlGreen);
        controlRedButton.onClick.AddListener(OnControlRed);

        // Spiel wird Pausiert
        Time.timeScale = 0;
    }

    void OnKiVsKi()
    {
        Debug.Log("KI vs KI Button clicked!");
        // Füge hier die Logik für KI vs KI hinzu
        SetAgentToInference(redAgent);
        SetAgentToInference(greenAgent);
        DisableParentCanvas();
        Time.timeScale = 1;
    }

    void OnControlGreen()
    {
        Debug.Log("Control Green Button clicked!");
        // Füge hier die Logik für das Kontrollieren des grünen Charakters hinzu
        SetAgentToHeuristic(greenAgent);
        SetAgentToInference(redAgent);
        DisableParentCanvas();
        Time.timeScale = 1;
    }

    void OnControlRed()
    {
        Debug.Log("Control Red Button clicked!");
        // Füge hier die Logik für das Kontrollieren des roten Charakters hinzu
        SetAgentToHeuristic(redAgent);
        SetAgentToInference(greenAgent);
        DisableParentCanvas();
        Time.timeScale = 1;
    }

    void SetAgentToHeuristic(GameObject agent)
    {
        // Zugriff auf das BehaviourParameters-Skript des Agenten
        BehaviorParameters behaviourParams = agent.GetComponent<BehaviorParameters>();

        if (behaviourParams != null)
        {
            // Ändern des BehaviourType zu HeuristicOnly
            behaviourParams.BehaviorType = BehaviorType.HeuristicOnly;
            Debug.Log("Grüner Agent auf HeuristicOnly gesetzt!");
        }
        else
        {
            Debug.LogWarning("BehaviourParameters am grünen Agenten nicht gefunden!");
        }
    }

    void SetAgentToInference(GameObject agent)
    {
        BehaviorParameters behaviourParams = agent.GetComponent<BehaviorParameters>();

        if (behaviourParams != null)
        {
            behaviourParams.BehaviorType = BehaviorType.InferenceOnly;
            Debug.Log("Grüner Agent auf InferenceOnly gesetzt!");
        }
        else
        {
            Debug.LogWarning("BehaviourParameters am grünen Agenten nicht gefunden!");
        }
    }

    public void DisableParentCanvas()
    {
        // Access the parent GameObject
        GameObject parent = transform.parent.gameObject;

        // Disable the parent GameObject (Canvas)
        if (parent != null)
        {
            parent.SetActive(false);
            Debug.Log("Parent Canvas has been disabled.");
        }
        else
        {
            Debug.LogWarning("No parent GameObject found.");
        }
    }
}

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
        // Button Click Listeners hinzuf�gen
        kiVsKiButton.onClick.AddListener(OnKiVsKi);
        controlGreenButton.onClick.AddListener(OnControlGreen);
        controlRedButton.onClick.AddListener(OnControlRed);

        // Spiel wird Pausiert
        Time.timeScale = 0;
    }

    void OnKiVsKi()
    {
        Debug.Log("KI vs KI Button clicked!");
        // F�ge hier die Logik f�r KI vs KI hinzu
        SetAgentToInference(redAgent);
        SetAgentToInference(greenAgent);
        DisableParentCanvas();
        Time.timeScale = 1;
    }

    void OnControlGreen()
    {
        Debug.Log("Control Green Button clicked!");
        // F�ge hier die Logik f�r das Kontrollieren des gr�nen Charakters hinzu
        SetAgentToHeuristic(greenAgent);
        SetAgentToInference(redAgent);
        DisableParentCanvas();
        Time.timeScale = 1;
    }

    void OnControlRed()
    {
        Debug.Log("Control Red Button clicked!");
        // F�ge hier die Logik f�r das Kontrollieren des roten Charakters hinzu
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
            // �ndern des BehaviourType zu HeuristicOnly
            behaviourParams.BehaviorType = BehaviorType.HeuristicOnly;
            Debug.Log("Gr�ner Agent auf HeuristicOnly gesetzt!");
        }
        else
        {
            Debug.LogWarning("BehaviourParameters am gr�nen Agenten nicht gefunden!");
        }
    }

    void SetAgentToInference(GameObject agent)
    {
        BehaviorParameters behaviourParams = agent.GetComponent<BehaviorParameters>();

        if (behaviourParams != null)
        {
            behaviourParams.BehaviorType = BehaviorType.InferenceOnly;
            Debug.Log("Gr�ner Agent auf InferenceOnly gesetzt!");
        }
        else
        {
            Debug.LogWarning("BehaviourParameters am gr�nen Agenten nicht gefunden!");
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

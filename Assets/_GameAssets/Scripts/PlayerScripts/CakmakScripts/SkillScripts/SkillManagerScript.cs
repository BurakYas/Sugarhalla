using UnityEngine;

public class SkillManagerScript : MonoBehaviour
{
    private DashScript dashScript;
    private NewControlHandler controlHandler;

    void Awake()
    {
        dashScript = GetComponent<DashScript>();
        controlHandler = GetComponent<NewControlHandler>();
    }

    public void ActivateSkill(string skillName, Vector3 direction)
    {
        if (skillName == "Dash" && dashScript != null && dashScript.enabled)
        {
            Debug.Log("Dash skill çağrıldı!"); // Test için log
            dashScript.Dash(direction);
        }
        // Diğer skill'ler için else if ekleyebilirsin
    }

    public void EnableDoubleJump(bool enabled)
    {
        if (controlHandler != null)
            controlHandler.maxJumps = enabled ? 2 : 1;
    }
}

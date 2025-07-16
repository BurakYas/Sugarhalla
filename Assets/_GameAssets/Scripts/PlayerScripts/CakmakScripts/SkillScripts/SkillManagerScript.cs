using UnityEngine;

public class SkillManagerScript : MonoBehaviour
{
    private DashScript dashScript;
    private NewControlHandler controlHandler;
    private SpinScript spinScript;

    void Awake()
    {
        dashScript = GetComponent<DashScript>();
        controlHandler = GetComponent<NewControlHandler>();
        spinScript = GetComponent<SpinScript>();
    }

    public void ActivateSkill(string skillName, Vector3 direction)
    {
        if (skillName == "Dash" && dashScript != null && dashScript.enabled)
        {
            Debug.Log("Dash skill çağrıldı!"); // Test için log
            dashScript.Dash(direction);
        }
        else if (skillName == "Spin" && spinScript != null)
        {
            spinScript.Spin();
        }
        // Diğer skill'ler için else if ekleyebilirsin
    }

    public void EnableDoubleJump(bool enabled)
    {
        if (controlHandler != null)
            controlHandler.maxJumps = enabled ? 2 : 1;
    }
}

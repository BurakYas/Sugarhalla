using UnityEngine;

public class SkillManagerScript : MonoBehaviour
{
    private DashScript dashScript;

    void Awake()
    {
        dashScript = GetComponent<DashScript>();
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
}

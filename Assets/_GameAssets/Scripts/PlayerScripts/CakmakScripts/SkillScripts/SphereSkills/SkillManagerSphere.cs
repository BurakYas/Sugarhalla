using UnityEngine;

public class SkillManagerSphere : MonoBehaviour
{
    private DashSphere dashSphere;
    

    void Awake()
    {
         dashSphere = GetComponent<DashSphere>();
        
    }

    public void ActivateSkill(string skillName, Vector3 direction)
    {
        switch (skillName)
        {
            case "Dash":
                Debug.Log("SkillManagerSphere: Dash çağrıldı!");
                if (dashSphere != null)
                    dashSphere.Dash(direction);
                break;
            // Diğer skill'ler...
        }
    }
}

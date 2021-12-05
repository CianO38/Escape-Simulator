using UnityEngine;

public class LightSwitch : Interactable
{
    public Light _light;
    public bool isOn;

    void Start()
    {
        UpdateLight();
    }

    void UpdateLight()
    {
        _light.enabled = isOn;
    }

    public override string GetDescription()
    {
        if (isOn)
            return "Press [E] to turn <color=red>Off</color> the light.";

        return "Press [E] to turn <color=green>On</color> the light.";
    }

    public override void Interact()
    {
        isOn = !isOn;
        UpdateLight();
    }
}

using UnityEngine;

public class UIDirectionControl : MonoBehaviour
{
    public bool m_UseRelativeRotation = true;
	public static Transform Parent;

    private Quaternion m_RelativeRotation;     

    private void Start()
    {
        m_RelativeRotation = Parent.localRotation;
    }


    private void Update()
    {
        if (m_UseRelativeRotation)
            transform.rotation = m_RelativeRotation;

		transform.position = Parent.position;
    }
}

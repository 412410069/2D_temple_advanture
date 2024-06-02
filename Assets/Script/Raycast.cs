using UnityEngine;

public class RayData
{
    public Vector2 m_start;
    public float m_distance;
    public float m_angle;
    public Vector2 m_direction;
    public Vector2 m_end;
    public Collider m_hitCollider;
    public bool m_hit;

    public RayData(Vector2 start, float angle, float distance)
    {
        m_start = start;
        m_distance = distance;
        UpdateDirection(angle);
    }

    public void UpdateDirection(float angle)
    {
        m_angle = angle;
        m_direction = DirectionFromAngle(m_angle);
        m_end = m_start + m_direction * m_distance;
    }

    private Vector2 DirectionFromAngle(float angle)
    {
        float radians = angle * Mathf.Deg2Rad;
        return new Vector2(Mathf.Sin(radians), Mathf.Cos(radians));
    }
}

public class Raycast : MonoBehaviour
{
    public float _radius = 5.0f;
    public int _divide = 20;
    public GameObject player;

    private RaycastHit _hit;

    private RayData[] GetOriginalDatas()
    {
        RayData[] rayDatas = new RayData[_divide + 1];
        Vector2 center = transform.position;
        float startAngle = transform.eulerAngles.z - 360f / 2;
        float angleIncrement = 360f / _divide;

        for (int i = 0; i <= _divide; i++)
        {
            float currentAngle = startAngle + angleIncrement * i;
            rayDatas[i] = new RayData(center, currentAngle, _radius);
        }

        return rayDatas;
    }

    private RayData[] GetNormalDatas()
    {
        RayData[] rayDatas = GetOriginalDatas();

        for (int i = 0; i < rayDatas.Length; i++)
        {
            UpdateRaycast(rayDatas[i]);
        }

        return rayDatas;
    }

    private void UpdateRaycast(RayData rayData)
    {
        rayData.m_hit = Physics.Raycast(transform.position, rayData.m_direction, out _hit, _radius);

        if (rayData.m_hit)
        {
            rayData.m_hitCollider = _hit.collider;
            rayData.m_end = _hit.point;
        }
        else
        {
            rayData.m_hitCollider = null;
            rayData.m_end = rayData.m_start + rayData.m_direction * _radius;
        }
    }

    public bool IsPlayerInSight()
    {
        RayData[] rayDatas = GetNormalDatas();

        foreach (RayData rayData in rayDatas)
        {
            if (rayData.m_hit && rayData.m_hitCollider.gameObject == player)
            {
                return true;
            }
        }

        return false;
    }
}

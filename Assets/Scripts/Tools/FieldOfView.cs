using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FieldOfView : MonoBehaviour
{
    public float n_viewRadius;
    public float n_viewRadiusPursuit;
	[Range(0, 360)] public float n_viewAngle;
    public LayerMask n_targetMask;
	public LayerMask n_obstacleMask;
    public List<Transform> n_visibleTargetsCheck = new List<Transform>();
    public List<Transform> n_visibleTargetsPursuit = new List<Transform>();

    void Start()
    {
        StartCoroutine("FindTargetsWithDelay", .2f);
    }


    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargetsZ();
        }
    }

    public bool AnyCheckTargetVisible()
    {
        if(n_visibleTargetsCheck.Count >= 1)
        return true;
        else return false;
    }

    public bool AnyPursuitTargetVisible()
    {
        if(n_visibleTargetsPursuit.Count >= 1)
        return true;
        else return false;
    }


    void FindVisibleTargetsZ()
    {
        n_visibleTargetsCheck.Clear();
        n_visibleTargetsPursuit.Clear();
        Collider[] n_targets = Physics.OverlapSphere(transform.position, n_viewRadius, n_targetMask);
        for (int i = 0; i < n_targets.Length; i++)
        {
			Transform n_target = n_targets[i].transform;
            Vector3 n_dirToTarget = (n_target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, n_dirToTarget) < n_viewAngle /2)
			{
				float n_dstToTarget = Vector3.Distance(transform.position, n_target.position);
				
                if(!Physics.Raycast(transform.position, n_dirToTarget, n_dstToTarget, n_obstacleMask))
				{
                    if(n_dstToTarget > n_viewRadiusPursuit)
                    {
                        n_visibleTargetsCheck.Add(n_target);
                        }
                }

            }

        }
    }

    public Vector3 DirectionFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
		if(!angleIsGlobal) angleInDegrees += transform.eulerAngles.z + 90;
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), 0);
    }

}
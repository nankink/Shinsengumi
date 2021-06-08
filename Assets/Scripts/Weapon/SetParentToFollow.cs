using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetParentToFollow : MonoBehaviour
{
       public Transform toFollow;

        void Awake()
        {
            transform.rotation = toFollow.rotation;
            transform.SetParent(toFollow);
            
        }

}

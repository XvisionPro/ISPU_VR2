using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Models
{
    public class Location : MonoBehaviour
    {
        public Transform mainTarget;
        public GameObject objLocation;
        public float camDistanse;

        public Vector3 cameraPos;
        public Vector3 cameraRot;
    }
}

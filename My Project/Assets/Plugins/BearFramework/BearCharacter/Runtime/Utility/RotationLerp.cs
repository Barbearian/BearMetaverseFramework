using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Bear
{
    public struct CharacterRotationLerp
    {
        private Vector3 targetDir;
        public Transform rotationTarget;
        public float RotationSpeed;


        public Quaternion GetRotation() {
            if (targetDir.sqrMagnitude > 0) {
                return Quaternion.Lerp(rotationTarget.rotation,Quaternion.LookRotation(targetDir),RotationSpeed*Time.deltaTime);
            }
            else
            {
               return Quaternion.LookRotation( rotationTarget.forward,rotationTarget.up);

            }
        }

        public void SetTargetRotation(Vector3 targetDir) { 
            this.targetDir = targetDir;
        }




        
    }
}
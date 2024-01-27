using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace KinematicCharacterController.Examples
{
    public class AIClownController : MonoBehaviour
    {
        public float MovementPeriod = 1f;
        public List<CircusCharacterController> Characters = new ();

        private bool _stepHandling;
        private bool _ledgeHandling;
        private bool _intHandling;
        private bool _safeMove;

        private void Update()
        {
            var dest = transform.position;
            if (CircusPlayer.instance != null)
            {
                var target = CircusPlayer.instance.Character;
                if (target != null)
                {
                    NavMeshPath path = new NavMeshPath ();
                    if (NavMesh.CalculatePath (transform.position, target.transform.position, NavMesh.AllAreas, path))
                    {
                        for (int i = 0; i < path.corners.Length-1; i++)
                        {
                            int next_i = i+1;
                            Debug.DrawLine(path.corners[i], path.corners[next_i], Color.Lerp(Color.red, Color.green, (float)i / path.corners.Length));
                        }

                        if (path.corners.Length > 1)
                        {
                            dest = path.corners[1];
                        }
                    }
                    // else
                    // {
                    //     Debug.Log("no path!");
                    // }
                }
            }
            
            AICharacterInputs inputs = new AICharacterInputs();

            // Simulate an input on all controlled characters
            inputs.MoveVector = (dest - transform.position).normalized;
            inputs.LookVector = inputs.MoveVector; // Vector3.Slerp(-Vector3.forward, Vector3.forward, inputs.MoveVector.z).normalized;

            
            for (int i = 0; i < Characters.Count; i++)
            {
                Characters[i].SetInputs(ref inputs);
            }
        }
    }
}
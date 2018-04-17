using UnityEngine;

namespace Formation
{
    public class FormationManager : MonoBehaviour
    {
        public FormationType CurrentFormationType { get; set; }

        void Awake()
        {
            CurrentFormationType = FormationType.Auto;
        }
    }
}

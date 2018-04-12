using UnityEngine;

namespace AI
{
    public class EnemyStateController : StateController
    {
        public float abilityToUseDecisionInterval = 0.0f;

        private float abilityToUseDecisionTimer = 0.0f;

        public bool IsReadyToChooseAbility()
        {
            return abilityToUseDecisionTimer >= abilityToUseDecisionInterval;
        }

        public void ResetAbilityChoiceTimer()
        {
            abilityToUseDecisionTimer = 0.0f;
        }
        
        protected override void Update()
        {
            base.Update();

            abilityToUseDecisionTimer += Time.deltaTime;
        }
    }
}

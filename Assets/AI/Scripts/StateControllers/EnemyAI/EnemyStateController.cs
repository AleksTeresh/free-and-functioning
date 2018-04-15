using UnityEngine;

namespace AI
{
    public class EnemyStateController : UnitStateController
    {
        public float abilityToUseDecisionInterval = 4.0f;

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

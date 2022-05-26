using System;
using System.Collections;
using System.Collections.Generic;
using Ability.Type;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Ability.Manager
{
    public class AbilityManager : MonoBehaviour
    {
	    public event Action OnActivateAbility2;
	    public event Action OnActivateAbility3;
	    public event Action OnActivateAbility4;
	    
	    [TitleGroup("Abilities","Type Abilities", TitleAlignments.Centered)]
	    public AbilityType Ability2;
	    public AbilityType Ability3;
	    public AbilityType Ability4;

	    [TitleGroup("Uses")]
	    public bool CanUseAbility2;
	    public bool CanUseAbility3;
	    public bool CanUseAbility4;

	    internal float cooldownAbility2;
	    internal float cooldownAbility3;
	    internal float cooldownAbility4;


	    private void Awake()
	    { 
		    RestartAllCooldownsAbilities();
	    }



	    private void Update()
	    {
		    if (Ability2) CooldownAbility2();
		    if (Ability3) CooldownAbility3();
		    if (Ability4) CooldownAbility4();
	    }

	    private void CooldownAbility2()
	    {
		    if (CanUseAbility2 == false)
		    {
			    cooldownAbility2 += Time.deltaTime;
			    cooldownAbility2 = Mathf.Clamp(cooldownAbility2, 0, Ability2.Cooldown);

			    if (cooldownAbility2 >= Ability2.Cooldown)
			    {
				    OnActivateAbility2?.Invoke();
				    CanUseAbility2 = true;
			    }
		    }
	    }
	    private void CooldownAbility3()
	    {
		    if (CanUseAbility3 == false)
		    {
			    cooldownAbility3 += Time.deltaTime;
			    cooldownAbility3 = Mathf.Clamp(cooldownAbility3, 0, Ability3.Cooldown);

			    if (cooldownAbility3 >= Ability3.Cooldown)
			    {
				    OnActivateAbility3?.Invoke();
				    CanUseAbility3 = true;
			    }
		    }
	    }
	    
	    private void CooldownAbility4()
	    {
		    if (CanUseAbility4 == false)
		    {
			    cooldownAbility4 += Time.deltaTime;
			    cooldownAbility4 = Mathf.Clamp(cooldownAbility4, 0, Ability4.Cooldown);

			    if (cooldownAbility4 >= Ability4.Cooldown)
			    {
				    OnActivateAbility4?.Invoke();
				    CanUseAbility4 = true;
			    }
		    }
	    }

	    private void RestartAllCooldownsAbilities()
	    {
		    cooldownAbility2 = Ability2.Cooldown;
		    cooldownAbility3 = Ability3.Cooldown;
		    cooldownAbility4 = Ability4.Cooldown;

		    CanUseAbility2 = true;
		    CanUseAbility3 = true;
		    CanUseAbility4 = true;
	    }

	    public void RestartCooldownAbility2()
	    {
		    cooldownAbility2 = 0;
	    }
	    public void RestartCooldownAbility3()
	    {
		    cooldownAbility3 = 0;
	    }
	    public void RestartCooldownAbility4()
	    {
		    cooldownAbility4 = 0;
	    }


    }
}



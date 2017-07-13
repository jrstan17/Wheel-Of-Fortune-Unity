using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RevealCanvas : MonoBehaviour {
	VisabilityToggler MainPanelToggler;
	VisabilityToggler PlayerCreationToggler;
	VisabilityToggler OptionPanelToggler;

	void Start(){
		VisabilityToggler[] togglers = gameObject.GetComponents<VisabilityToggler>();
		MainPanelToggler = togglers[0];
		PlayerCreationToggler = togglers[1];
		OptionPanelToggler = togglers[2];
	}

	public void NewGame_Clicked() {
		MainPanelToggler.ToggleVisability(false);
		PlayerCreationToggler.ToggleVisability(true);
	}

	public void Exit_Clicked() {
		Application.Quit();
	}

	public void Options_Clicked() {
		MainPanelToggler.ToggleVisability(false);
		OptionPanelToggler.ToggleVisability(true);
		EventSystem.current.SetSelectedGameObject(null);
	}

	public void CloseOptions() {
		MainPanelToggler.ToggleVisability(true);
		OptionPanelToggler.ToggleVisability(false);
		EventSystem.current.SetSelectedGameObject(null);
	}

	public void PlayerCreationBack_Clicked(){
		MainPanelToggler.ToggleVisability(true);
		PlayerCreationToggler.ToggleVisability(false);
		}
	}
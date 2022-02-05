using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MutationAdjustmentController : MonoBehaviour
{
	public Slider MutationChanceSlider;
	public Slider MutationStrengthSlider;
	public Text ChanceText;
	public Text StrengthText;

	private bool _initialized = false;

	private void Start()
	{
		MutationChanceSlider.value = GameController.Instance.MutationChance;
		MutationStrengthSlider.value = GameController.Instance.MutationStrength;

		ChanceText.text = (GameController.Instance.MutationChance/100f).ToString("00.00%");
		StrengthText.text = GameController.Instance.MutationStrength.ToString("00.00");

		_initialized = true;
	}

	public void OnValueChanged()
	{
		if(!_initialized)
		{
			return;
		}

		GameController.Instance.MutationChance = (int)MutationChanceSlider.value;
		GameController.Instance.MutationStrength = MutationStrengthSlider.value;
		ChanceText.text = (GameController.Instance.MutationChance / 100f).ToString("00.00%");
		StrengthText.text = GameController.Instance.MutationStrength.ToString("00.00");
	}
}

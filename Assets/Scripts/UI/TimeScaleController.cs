using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeScaleController : MonoBehaviour
{
	public Text TimeScaleDisplayText;
	public Slider TimeScaleSlider;
	public Button PauseButton;
	public Button PlayButton;
	public Button FastButton;

	private float _previousTimeScale;

	public void OnPauseButtonPressed()
	{
		SetTimeScale(1);
		Time.timeScale = 0;
	}

	public void OnPlayButtonPressed()
	{
		SetTimeScale(_previousTimeScale);
	}

	public void OnFastButtonPressed()
	{
		SetTimeScale(TimeScaleSlider.maxValue);
	}

	public void OnValueChanged()
	{
		TimeScaleDisplayText.text = TimeScaleSlider.value.ToString();
		Time.timeScale = TimeScaleSlider.value;
		_previousTimeScale = TimeScaleSlider.value;
	}

	public void SetTimeScale(float value)
	{
		TimeScaleSlider.value = value;
		TimeScaleDisplayText.text = value.ToString();
		Time.timeScale = value;
		_previousTimeScale = TimeScaleSlider.value;
	}
}

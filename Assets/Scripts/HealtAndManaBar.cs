using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealtAndManaBar : MonoBehaviour {
	
	//максимальное значение
	public float maxValue = 100;
	//дефолтный цвет
	public Color color = Color.red;
	//ширина  (высчитывается по ширине экрана)
	public int width = 4;
	//слайдер, на котором будут отображаться значения
	public Slider slider;
	//находится ли слайдер справа от экрана
	public bool isRight;	
	//текущее значение
	public float current;
	
	//устанавливаем стартовые значения и цвета
	void Start()
	{
		//устанавливаем цвет
		slider.fillRect.GetComponent<Image>().color = color;
		//задаем слайдеру максимальное значение
		slider.maxValue = maxValue;
		//задаем слайдеру минимальное значение
		slider.minValue = 0;
		//устанавливаем текущее значение равное максимальному
		current = maxValue;
		//отрисовываем полоску на экране
		UpdateUI();
	}

	//возвращаем ткущее значение
	public float currentValue
	{
		get { return current; }  
	}
	
	//проверяем коректность значений
	void Update () 
	{
		if(current < 0) current = 0;
		if(current > maxValue) current = maxValue;
		slider.value = current;
	}
	
	//отрисовка слайдера
	void UpdateUI()
	{
		RectTransform rect = slider.GetComponent<RectTransform>();

		int rectDeltaX = 250;
		float rectPosX = 0;

		if(isRight) 
		{
			rectPosX = rect.position.x - (rectDeltaX - rect.sizeDelta.x)/2;
			//slider.direction = Slider.Direction.RightToLeft;
		}
		else 
		{
			rectPosX = rect.position.x + (rectDeltaX - rect.sizeDelta.x)/2;
			//slider.direction = Slider.Direction.LeftToRight;
		}

		rect.sizeDelta = new Vector2(rectDeltaX, rect.sizeDelta.y);
		rect.position = new Vector3(rectPosX, rect.position.y, rect.position.z);
	}
	
}
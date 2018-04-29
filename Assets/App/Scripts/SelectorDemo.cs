using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity.InputModule;
using HoloToolkitExtensions.Utilities;
using UnityEngine;

public class SelectorDemo : MonoBehaviour, IInputClickHandler, IInputHandler    
{
    public Color SelectColor = Color.blue;

    private DoubleClickPreventer _doubleClickPreventer = new DoubleClickPreventer(0.5f);


    private Dictionary<MeshRenderer, Color> _originalColors;
    // Use this for initialization
    void Start()
    {
        _originalColors = new Dictionary<MeshRenderer, Color>();
        SaveDefaultColors();
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        HandleClick();
    }

    public void OnInputDown(InputEventData eventData)
    {
        if (eventData.PressType == InteractionSourcePressInfo.Select)
        {
            HandleClick();
        }
    }

    public void OnInputUp(InputEventData eventData)
    {
    }

    private void HandleClick()
    {
        if (!_doubleClickPreventer.CanClick())
        {
            return;
        }
        Debug.Log("HandleClick");

        StartCoroutine(FlashColor());
        var audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    private IEnumerator FlashColor()
    {
        SetColor(SelectColor);
        yield return new WaitForSeconds(0.1f);
        Reset();
    }

    private void SaveDefaultColors()
    {
        _originalColors.Clear();
        foreach (var component in GetComponentsInChildren<MeshRenderer>())
        {
            _originalColors.Add(component, component.material.color);
        }
    }

    private void SetColor(Color c)
    {
        foreach (var component in GetComponentsInChildren<MeshRenderer>())
        {
            component.material.color = c;
        }
    }

    private void Reset()
    {
        foreach (var component in GetComponentsInChildren<MeshRenderer>())
        {
           component.material.color = _originalColors[component];
        }
    }
}

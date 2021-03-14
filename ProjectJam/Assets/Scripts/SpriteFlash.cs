using System;

using UnityEngine;

using BarthaSzabolcs.CommonUtility;

[Serializable]
public class SpriteFlash : MonoBehaviour
{
    #region Datamembers

    #region Const Fields

    private const string SHADER_COLOR_NAME = "_Color";

    #endregion
    #region Editor Settings

    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Material soildColorMaterial;
    
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private float time;

    #endregion
    #region Private Fields

    private Timer timer = new Timer();

    private Material flashInstance;
    private Material defaultInstance;
    
    private Color currentColor;

    #endregion

    #endregion


    #region Methods

    public void Start()
    {
        timer.Interval = time;
        timer.OnTimeElapsed += () =>
        {
            spriteRenderer.material = defaultInstance;
        };
        timer.Init(true);

        defaultInstance = new Material(spriteRenderer.material);
        flashInstance = new Material(soildColorMaterial);
    }

    public void StartFlash(Color color)
    {
        currentColor = color;
        timer.Reset();

        spriteRenderer.material = flashInstance;
    }

    public void Update()
    {
        timer.Tick(Time.deltaTime);

        if (timer.Elapsed == false && spriteRenderer != null)
        {
            RefreshColor();
        }
    }

    private void RefreshColor()
    {
        var multiplier = curve.Evaluate(timer.ElapsedPercentage);
        var color = new Color()
        {
            r = currentColor.r * multiplier,
            g = currentColor.g * multiplier,
            b = currentColor.b * multiplier,
            a = 1f
        };

        flashInstance.SetColor(SHADER_COLOR_NAME, color);
    }

    #endregion
}
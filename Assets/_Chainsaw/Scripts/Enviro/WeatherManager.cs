using UnityEngine;
using DG.Tweening;

public class WeatherManager : MonoBehaviour
{
    public Material skyboxSunny, skyboxOvercast;

    public GameObject windPrt, rainPrt, fogPrt;

    private Sequence weathersSequence;
    public float weatherSeqInterval = 1f;
    
    public enum WeatherType : int
    {
        SUNNY,
        RAINY,
        FOGGY,
        WINDY
    }

    private void Start()
    {
        SetEnvironment(WeatherType.SUNNY);
        
    }

    public void SetEnvironment(WeatherType _weather)
    {
        switch (_weather)
        {
            case WeatherType.SUNNY:
                RenderSettings.skybox = skyboxSunny;
                windPrt.SetActive(false);
                rainPrt.SetActive(false);
                fogPrt.SetActive(false);

                break;
                
            case WeatherType.RAINY:
                RenderSettings.skybox = skyboxOvercast;
                windPrt.SetActive(false);
                rainPrt.SetActive(true);
                fogPrt.SetActive(false);
                break;

            case WeatherType.FOGGY:
                RenderSettings.skybox = skyboxOvercast;
                windPrt.SetActive(false);
                rainPrt.SetActive(false);
                fogPrt.SetActive(true);
                break;

            case WeatherType.WINDY:
                RenderSettings.skybox = skyboxOvercast;
                windPrt.SetActive(true);
                rainPrt.SetActive(false);
                fogPrt.SetActive(false);
                break;
        }
    }

    public void RunWeathers()
    {
        weathersSequence = DOTween.Sequence()
            .AppendCallback(() => { SetEnvironment(WeatherType.RAINY); })
            .AppendInterval(weatherSeqInterval)
            .AppendCallback(() => { SetEnvironment(WeatherType.FOGGY); })
            .AppendInterval(weatherSeqInterval)
            .AppendCallback(() => { SetEnvironment(WeatherType.WINDY); })
            .AppendInterval(weatherSeqInterval)
            .OnComplete(() => { SetEnvironment(WeatherType.SUNNY); });

        weathersSequence.Play();
    }

    public void CancelRunWeathers()
    {
        if (weathersSequence != null && weathersSequence.IsPlaying())
        {
            weathersSequence.Complete();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public class TileGraphics : MonoBehaviour
{
    public GameObject GrassImage;
    public GameObject Leaves1Image;
    public GameObject Leaves2Image;
    public GameObject Leaves3Image;
    public GameObject SquirrelHoleImage;
    public GameObject SquirrelImage;

    public GameObject mowerImage;
    public GameObject blowerImage;
    public GameObject shovelImage;

    [SerializeField] private GameObject _minus;
    [SerializeField] private GameObject _plus;
    [SerializeField] private GameObject _dust;
    [SerializeField] private GameObject _grass;
    [SerializeField] private GameObject _leaves;
    private int _particleDuration = 1000;

    public async void ShowPlusPS()
    {
        _plus.SetActive(true);
        await Task.Delay(_particleDuration);
        _plus.SetActive(false);
    }

    public async void ShowMinusPS()
    {
        _minus.SetActive(true);
        await Task.Delay(_particleDuration);
        _minus.SetActive(false);
    }

    public async void ShowDustPS()
    {
        _dust.SetActive(true);
        await Task.Delay(_particleDuration);
        _dust.SetActive(false);
    }

    public async void ShowGrassPS()
    {
        print("Here");
        _grass.SetActive(true);
        await Task.Delay(_particleDuration);
        _grass.SetActive(false);
    }

    public async void ShowLeavesPS()
    {
        _leaves.SetActive(true);
        await Task.Delay(_particleDuration);
        _leaves.SetActive(false);
    }
}

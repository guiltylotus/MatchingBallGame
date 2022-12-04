using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControler : MonoBehaviour
{
    [SerializeField] private AudioSource hitButtonSound;
    [SerializeField] private AudioSource matchingSound;
    [SerializeField] private AudioSource failedMatchingSound;
    
    public Sprite backgroundImage;
    public Sprite[] pokemonImages;
    
    private readonly List<Button> _pikachuButtons = new List<Button>();
    private List<Sprite> _btnImages = new List<Sprite>();

    private readonly int _maxSelectedBtnSize = 2;
    private int _currentTop = 0;
    private Button[] _selectedBtn = new Button[2];

    private int _totalBtn = 0;
    private int _leftBtn = 0;

    private Button _imgShowPokemon;
    
    void Awake()
    {
        pokemonImages = Resources.LoadAll<Sprite>("Sprites/PokemonImages");
    }

    // Start is called before the first frame update
    void Start()
    {
        SetupButton();
        SetupPanel();
        GetShowPokemon();
        AddButtonListener();
    }

    private void SetupButton()
    {
        var objects = GameObject.FindGameObjectsWithTag("PikachuButton"); ;
        var btnImageIndex = 0;
        _leftBtn = objects.Length;
        _totalBtn = objects.Length;
        
        for (var i=0; i< objects.Length; i++)
        {
            // Set pokeball
            _pikachuButtons.Add(objects[i].GetComponent<Button>());
            _pikachuButtons[i].image.sprite = backgroundImage;
            
            _btnImages.Add(pokemonImages[btnImageIndex]);
            if (i % 2 == 1)
            {
                btnImageIndex += 1;
            }
        }
        
        // Shuffer _btnImages
        var rand = new System.Random();
        var shuffled = _btnImages.OrderBy(_ => rand.Next()).ToList();
        _btnImages = shuffled;
    }

    private void SetupPanel()
    {
        // Set panel constraintCount
        var panel = GameObject.Find("IngamePanel");
        var btnOnRow = Math.Sqrt(_totalBtn);
        
        var gridLayoutGroup = panel.GetComponent<GridLayoutGroup>();
        gridLayoutGroup.constraintCount = Convert.ToInt32(Math.Round(btnOnRow));

        // Dynamic size 
        RectTransform rt = (RectTransform)panel.transform;
        var panelWidth = rt.rect.width;
        var panelHeight = rt.rect.height;

        var btnWidth = Math.Round(panelWidth / btnOnRow) - 10;
        var btnHeight = Math.Round(panelHeight / btnOnRow) - 10 ;
        var size = (float)Math.Min(btnHeight, btnWidth);
        
        gridLayoutGroup.cellSize = new Vector2(size, size);
    }

    private void GetShowPokemon()
    {
        var imagePanel = GameObject.Find("ShowPokemon");
        _imgShowPokemon = imagePanel.GetComponent<Button>();
    }

    private void AddButtonListener()
    {
        foreach (var btn in _pikachuButtons)
        {
            btn.onClick.AddListener(() => SelectButton(btn));
        }
    }
    private void SelectButton(Button btn)
    {
        Debug.Log("name "+ btn.name + " " + _currentTop + " " + _maxSelectedBtnSize);
        if (_currentTop >= _maxSelectedBtnSize)
        {   
            Debug.Log("dont trick me");
            return;
        }
        _currentTop += 1;
        _selectedBtn[_currentTop-1] = btn;
        RevealPokemon(btn, _btnImages[Int32.Parse(btn.name)]);

        if (_currentTop == _maxSelectedBtnSize)
        {
            _selectedBtn[_currentTop-1] = btn;
            
            if (CheckSelectedResult())
            {
                // matchingSound.Play();
                StartCoroutine(RemovePokemons());
            } else
            { 
                // failedMatchingSound.Play();
                StartCoroutine(HideRevealedPokemons());
            }
        }
    }
    private bool CheckSelectedResult() 
    {
        Debug.Log("Selected done! " + _selectedBtn[0].name + " " + _selectedBtn[1].name);
        var img1 = _btnImages[Int32.Parse(_selectedBtn[0].name)];
        var img2 = _btnImages[Int32.Parse(_selectedBtn[1].name)];
        if (img1.name == img2.name)
        {
            return true;
        }

        return false;
    }

    private void RevealPokemon(Button btn, Sprite img)
    {
        hitButtonSound.Play();
        btn.enabled = false;
        btn.image.sprite = img;
        _imgShowPokemon.image.sprite = btn.image.sprite;
    }

    IEnumerator RemovePokemons()
    {
        yield return new WaitForSeconds(0.5f);
        matchingSound.Play();
        RemovePokemon(_selectedBtn[0]);
        RemovePokemon(_selectedBtn[1]);
        _currentTop = 0;
        StartCoroutine(CheckAndEndGame());
    }

    private void RemovePokemon(Button btn)
    {
        btn.image.color = Color.clear;
        btn.interactable = false;
        btn.enabled = false;
        
        _leftBtn -= 1;
    }

    IEnumerator CheckAndEndGame()
    {
        if (_leftBtn == 0)
        {
            yield return new WaitForSeconds(1);
            SceneManager.LoadScene("RestartGame");
        }
    }
    IEnumerator HideRevealedPokemons()
    {
        yield return new WaitForSeconds(0.5f);
        failedMatchingSound.Play();
        HidePokemon(_selectedBtn[0]);
        HidePokemon(_selectedBtn[1]);
        _currentTop = 0;
        _imgShowPokemon.image.sprite = backgroundImage;
    }
    
    private void HidePokemon(Button btn)
    {
        btn.image.sprite = backgroundImage;
        btn.enabled = true;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UniRx;
using UnityEngine;

using Random = System.Random; 

public class GameController : MonoBehaviour {
    private const string TIMER_FORMAT = "Jar Filling\n{0}";
    private readonly YieldInstruction _waitOneSecond = new WaitForSeconds(1);

    public Canvas mainCanvas;
    public int totalSeconds = 600;
    public Transform itemsCountainer;
    public Item[] itemPrefabs;
    public int totalItemsCount;
    public Vector2[] bluePath;
    public Vector2[] redPath;
    public float itemsMoveDelay = 1;
    public Vector2 itemStartingPosition = new Vector2(0, 149.66f);
    public TextMeshProUGUI _blueCountText;
    public TextMeshProUGUI _redCountText;
    public TextMeshProUGUI _timerText;
    public Transform _blueContainer;
    public Transform _redContainer;

    private List<Item> _items;
    private YieldInstruction _waitDuration;
    private int _blueCount;
    private int _redCount;
    private int _timer;
    private Random _random;

    private void GenerateItems() {
        _items = new List<Item>();
        for (int i = 0; i < totalItemsCount; i++) {
            var randomItemIndex = UnityEngine.Random.Range(0, itemPrefabs.Length - 1);
            var item = Instantiate(itemPrefabs[randomItemIndex], itemsCountainer);
            item.Transform.localScale = Vector3.one;
            _items.Add(item);
        }
    }

    private IEnumerator Play() {
        _random = new Random(DateTime.Now.Millisecond);
        var blueIsBigger = _random.NextDouble() > .5f;//just another random to make whole random more random)))
        while (_items.Count > 0 && _timer > 0) {
            var item = _items[0];
            var random = _random.NextDouble();
            item.Transform.SetParent(mainCanvas.transform);
            if (random >= .5f && blueIsBigger) {
                _blueCount++;
                item.Move(bluePath, itemStartingPosition,
                    () => { item.Transform.SetParent(_blueContainer); });
            }
            else {
                _redCount++;
                item.Move(redPath, itemStartingPosition, () => { item.Transform.SetParent(_redContainer); });
            }

            _blueCountText.text = _blueCount.ToString();
            _redCountText.text = _redCount.ToString();

            _items.Remove(item);

            yield return _waitDuration;
        }
    }

    private IEnumerator Timer() {
        while (_items.Count > 0 && _timer > 0) {
            yield return _waitOneSecond;
            _timer -= 1;
            _timerText.text = string.Format(TIMER_FORMAT, TimeSpan.FromSeconds(_timer).ToString(@"mm\:ss"));
        }
    }

    private void Awake() {
        _timer = totalSeconds;
        _waitDuration = new WaitForSeconds(itemsMoveDelay);
    }

    // Start is called before the first frame update
    private void Start() {
        GenerateItems();
    }

    // Update is called once per frame
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            StartCoroutine(Play());
            StartCoroutine(Timer());
        }
    }
}
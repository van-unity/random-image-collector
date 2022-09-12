using System;
using DG.Tweening;
using UnityEngine;

public class Item : MonoBehaviour {
    public float moveDuration;
    public GameObject GameObject { get; private set; }
    public Transform Transform { get; private set; }

    public RectTransform RectTransform { get; private set; }

    private void Awake() {
        GameObject = gameObject;
        Transform = GameObject.transform;
        RectTransform = GetComponent<RectTransform>();
    }

    public void Move(Vector2[] waypoints, Vector2 from, Action callback) {
        RectTransform.pivot = Vector2.one * .5f;
        RectTransform.anchorMin = RectTransform.pivot;
        RectTransform.anchorMax = RectTransform.pivot;
        RectTransform.anchoredPosition = from;

        var sequence = DOTween.Sequence();
        foreach (var waypoint in waypoints) {
            sequence.Append(RectTransform.DOAnchorPos(waypoint, moveDuration * .3333f));
        }

        sequence.OnComplete(() => callback());
    }
}
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MatchValue {
    blue,
    green,
    orange,
    purple,
    red,
    yellow,
    wild,
    none
};

[RequireComponent(typeof(SpriteRenderer))]
public class GamePiece : MonoBehaviour {
    public int xIndex;
    public int yIndex;

    Board m_board;

    bool m_isMoving = false;

    public InterpType interpolation = InterpType.SmootherStep;

    public enum InterpType {
        Linear,
        EaseOut,
        EaseIn,
        SmoothStep,
        SmootherStep
    };

    public MatchValue matchValue;
    
    public int breakableValue = 0;
    public Sprite[] breakableSprites = new Sprite[1];
    SpriteRenderer m_spriteRenderer;

    public int scoreValue = 20;

    public AudioClip clearSound;

    void AssignSprite() {
        if (breakableSprites[breakableValue] != null) {
            m_spriteRenderer.sprite = breakableSprites[breakableValue];
        }
    }

    // Start is called before the first frame update
    void Start() {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        AssignSprite();
    }

    // Update is called once per frame
    void Update() {

        /*
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            Move((int)(transform.position.x + 1), (int)transform.position.y, 0.5f);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            Move((int)(transform.position.x - 1), (int)transform.position.y, 0.5f);
        }*/
    }

    public void Init(Board board) {
        m_board = board;
    }

    public void SetCoord(int x, int y) {
        xIndex = x;
        yIndex = y;
    }

    public void Move(int destX, int destY, float timeToMove) {
        if(!m_isMoving)
            StartCoroutine(MoveRoutine(new Vector3(destX, destY, 0), timeToMove));
    }

    IEnumerator MoveRoutine(Vector3 destination, float timeToMove) {
        Vector3 startPosition = transform.position;
        bool reachedDestination = false;
        m_isMoving = true;
        float elapsedTime = 0f;

        while (!reachedDestination) {
            if(Vector3.Distance(transform.position, destination) < 0.01f) {
                reachedDestination = true;
                if(m_board != null) {
                    m_board.PlaceGamePiece(this, (int)destination.x, (int)destination.y);
                }
                break;
            }
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp(elapsedTime / timeToMove, 0f, 1f);

            switch (interpolation) {
                case InterpType.Linear:
                    break;
                case InterpType.EaseOut:
                    t = Mathf.Sin(t * Mathf.PI * 0.5f);
                    break;
                case InterpType.EaseIn:
                    t = 1 - Mathf.Cos(t * Mathf.PI * 0.5f);
                    break;
                case InterpType.SmoothStep:
                    t = t * t * (3 - 2 * t);
                    break;
                case InterpType.SmootherStep:
                    t = t * t * t * (t * (t * 6 - 15) + 10);
                    break;
            }
            transform.position = Vector3.Lerp(startPosition, destination, t);

            yield return null;
        }
        m_isMoving = false;
    }

    public void ChangeColor(GamePiece pieceToMatch) {
        SpriteRenderer rendererToChange = GetComponent<SpriteRenderer>();

        if(pieceToMatch != null) {
            SpriteRenderer renderToMatch = pieceToMatch.GetComponent<SpriteRenderer>();
            if(renderToMatch != null && rendererToChange != null) {
                rendererToChange.color = renderToMatch.color;
            }
            matchValue = pieceToMatch.matchValue;
        }
    }

    public bool Break() {
        breakableValue--;
        if (breakableValue >= 0) {
            AssignSprite();
        }
        return breakableValue < 0;
    }
}

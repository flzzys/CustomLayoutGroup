using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Test : MonoBehaviour {
    public GameObject itemPrefab;

    public UIMover uiMover;

    public RectTransform card;

    public Panel_CardSet prefab_CardSet;
    Panel_CardSet panel_CardSet;

    [Header("变化_显示")]
    public UXMovement movement_Show;

    [Header("变化_隐藏")]
    public UXMovement movement_Hide;

    public RectTransform[] poses;

    private void Awake() {
        card.sizeDelta = poses[0].sizeDelta;
        card.position = poses[0].position;

        panel_CardSet = Instantiate(prefab_CardSet, FindObjectOfType<Canvas>().transform);

        cardStartPos = new Rect {
            position = poses[0].position,
            size = poses[0].rect.size
        };
    }

    Rect cardStartPos;
    Rect cardEndPos;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Q)) {
            panel_CardSet.gameObject.SetActive(true);

            if(panel_CardSet.inventoryLayoutGroup.transform.childCount == 0) {
                CreateTestObject();
                panel_CardSet.InitPos(poses);

                cardEndPos = new Rect {
                    position = panel_CardSet.inventoryLayoutGroup.GetFirstItemPos(),
                    size = panel_CardSet.inventoryLayoutGroup.GetActualSize()
                };
            }

            //移动Card
            UIMover.Move(card, new MoverParams {
                startRect = cardStartPos,
                endRect = cardEndPos,
                changeScale = true
            }, () => {
                //隐藏Card
                //card.gameObject.SetActive(false);
            });

            card.GetComponent<CanvasGroup>().DOFade(0, .5f);
            panel_CardSet.GetComponent<CanvasGroup>().DOFade(1, .5f);

            //移动CardSet
            panel_CardSet.Move(true);
        }

        if (Input.GetKeyDown(KeyCode.W)) {
            panel_CardSet.Move(false);

            //移动Card
            card.gameObject.SetActive(true);

            UIMover.Move(card, new MoverParams {
                startRect = new Rect {
                    position = card.position,
                    size = card.rect.size * card.localScale
                },
                endRect = cardStartPos,
                changeScale = true
            }, () => {
                //panel_CardSet.gameObject.SetActive(false);
            });

            card.GetComponent<CanvasGroup>().DOFade(1, .5f);
            panel_CardSet.GetComponent<CanvasGroup>().DOFade(0, .5f);

        }

        if (Input.GetKeyDown("a")) {
            RectTransform target = panel_CardSet.inventoryLayoutGroup.transform.GetChild(2).GetComponent<RectTransform>();

            UIMover.Move(target, new MoverParams {
                endRect = new Rect {
                    position = poses[0].position,
                    size = poses[0].sizeDelta
                },
                changeScale = true
            });
        }
        if (Input.GetKeyDown("s")) {
            RectTransform target = panel_CardSet.inventoryLayoutGroup.transform.GetChild(2).GetComponent<RectTransform>();

            UIMover.Move(target, new MoverParams {
                endRect = new Rect {
                    position = poses[0].position,
                    size = poses[0].sizeDelta
                },
                changeScale = true
            });
        }
    }

    //测试物体
    public Sprite sprite;
    void CreateTestObject() {
        for (int i = 0; i < 8; i++) {
            GameObject go = new GameObject("qwe");
            go.transform.SetParent(panel_CardSet.inventoryLayoutGroup.transform);
            go.AddComponent<RectTransform>();
            go.AddComponent<Image>().sprite = sprite;
        }
    }
}

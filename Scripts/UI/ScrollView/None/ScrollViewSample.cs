using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewSample : MonoBehaviour
{

    [SerializeField] private RectTransform _content;
    [SerializeField] private GameObject _prefabListItem;

    [Space(10)]
    [Header("Scroll view Events")]
    [SerializeField] private ItemButtonEvent _eventItemClicked;
    [SerializeField] private ItemButtonEvent _eventItemOnSelect;
    [SerializeField] private ItemButtonEvent _eventItemOnSubmit;

    [Space(10)]
    [Header("Default Selected Index")]
    [SerializeField] private int _defaultSelectedIndex = 0;

    [Space(10)]
    [Header("For Testing Only")]
    [SerializeField] private int _testButtonCount = 1;

    // Start is called before the first frame update
    void Start()
    {
        if(_testButtonCount > 0)
        {
            TestCreateItems(_testButtonCount);
        }
    }

    private void TestCreateItems(int count)
    {
        for(int i = 0; i<count; i++)
        {
            CreateItems("Player" + i);
        }
    }

    private ItemButton CreateItems(string strName)
    {
        GameObject gObj;
        ItemButton item;

        gObj = Instantiate(_prefabListItem, Vector3.zero, Quaternion.identity);
        gObj.transform.SetParent(_content.transform);
        gObj.transform.localScale = new Vector3(1f, 1f, 1f);
        gObj.transform.localPosition = new Vector3();
        gObj.transform.localRotation = Quaternion.Euler(new Vector3());
        gObj.name = strName;

        item = gObj.GetComponent<ItemButton>();
        item.ItemNameValue = strName;

        item.OnSelectEvent.AddListener((ItemButton) => {HandleEventItemOnSelect(item); });
        item.OnclickEvent.AddListener((ItemButton) => { HandleEventItemOnClick(item); });
        item.OnSubmitEvent.AddListener((ItemButton) => { HandleEventItemOnSubmit(item); });

        return item;
    }

    private void HandleEventItemOnSubmit(ItemButton item)
    {
        _eventItemOnSubmit.Invoke(item);
    }

    private void HandleEventItemOnClick(ItemButton item)
    {
        _eventItemClicked.Invoke(item);
    }

    private void HandleEventItemOnSelect(ItemButton item)
    {
        _eventItemOnSelect.Invoke(item);
    }
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;

public class ScrollItem
{
    
    int mID;
    GameObject mObj;

    public void BindGameObject(GameObject obj)
    {
        mObj = obj;
    }

    public void SetGameObjectPosition(Vector3 pos)
    {
        mObj.transform.localPosition = pos;
    }

    public Vector3 GetGameObjectPosition()
    {
        return mObj.transform.localPosition;
    }

    public void SetData(int data)
    {
        mID = data;
    }

    public int GetData()
    {
        return mID;
    }

}

public class ScrollViewManager : MonoBehaviour
{
    public enum style {
        Horizontal,                         //横向分布
        Vertical,                           //纵向分布
        //HorizontalAndVertical               //横向和纵向分布
    };

    public Image scrollView;                //scroll view  
    public GameObject grid;                      //grid
    public Scrollbar HorizontalScrollBar;   //控制horizontal的scroll bar
    public Scrollbar VerticalScrollBar;     //控制vertical的scroll bar
    public int number;                      //物品总数量
    public float middle;                    //物品间距
    public style manage_style;              //管理物品方式
    public GameObject prefab;             //物品对象
    //public int rowCount;                    //行数
    //public int columnCount;                 //列数
    // public string prefab_path;              //物品资源路径

    private LinkedList<ScrollItem> itemList;            //存放物品的列表
    private float item_width;               //物品宽度
    private float item_height;              //物品高度
    private float sv_width;                 //scroll view宽度
    private float sv_height;                //scroll view高度   
    private float grid_width;               //grid宽度
    private float grid_height;              //grid高度
    private int row;                        //grid内可放物品行数
    private int column;                     //grid内可放物品列数
    private GameObject obj;                 //实例化物品对象
    private Vector3 gridOldPosition;        //grid更新前的坐标

    // Use this for initialization
    void Start()
    {
        // gameObj = Resources.Load(prefab_path) as GameObject;   ////加载prefab
        grid.transform.localPosition = new Vector3(0, 0, 0);   ////设置grid坐标
        item_width = prefab.GetComponent<RectTransform>().rect.width;  ////获取Item宽高
        item_height = prefab.GetComponent<RectTransform>().rect.height;
        sv_width = scrollView.GetComponent<RectTransform>().rect.width;  //获取scroll view 宽高
        sv_height = scrollView.GetComponent<RectTransform>().rect.height;

        if(HorizontalScrollBar != null)
        {
            HorizontalScrollBar.GetComponent<RectTransform>().sizeDelta = new Vector2(sv_width ,30);    //设置scroll bar坐标
            HorizontalScrollBar.transform.localPosition = new Vector3(0, -sv_height, 0) + scrollView.transform.localPosition;
        }
        if(VerticalScrollBar != null)
        {
            VerticalScrollBar.GetComponent<RectTransform>().sizeDelta = new Vector2(30, sv_height);
            VerticalScrollBar.transform.localPosition = new Vector3(sv_width, 0, 0) + scrollView.transform.localPosition;
        }
        itemList = new LinkedList<ScrollItem>();

        //根据所选排列方式初始化创建Item
        switch (manage_style)  
        {
            case style.Horizontal:
                grid_width = number * (middle + item_width);
                column = GetUpInt(sv_width, item_width + middle) + 1;
                if (grid_width <= sv_width)
                {
                    column = number;
                    grid_width = sv_width;
                }
                grid_height = sv_height;
                row = 1;
                grid.GetComponent<RectTransform>().sizeDelta = new Vector2(grid_width, grid_height);   
                HorizontalInitItem();
                break;

            case style.Vertical:
                grid_width = sv_width;
                column = 1;
                grid_height = number * (middle + item_height);
                row = GetUpInt(sv_height, item_height + middle) + 1;
                if (grid_height <= sv_height)
                {
                    row = number;
                    grid_height = sv_height;
                }
                grid.GetComponent<RectTransform>().sizeDelta = new Vector2(grid_width, grid_height);
                VerticalInitItem();
                break;

            //case style.HorizontalAndVertical:   //既上下滑动又左右滑动的有些问题先注释掉
            //    grid_width = columnCount * (middle + item_width);
            //    column = (int)(sv_width / (item_width + middle)) + 1;
            //    if (grid_width <= sv_width)
            //    {
            //        column = columnCount;
            //        grid_width = sv_width;                   
            //    }
            //    grid_height = rowCount * (middle + item_height);
            //    row = (int)(sv_height / (item_height + middle)) + 1;
            //    if (grid_height <= sv_height)
            //    {
            //        row = rowCount;
            //        grid_height = sv_height;
            //    }
            //    grid.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(grid_width, grid_height);
            //    HorizontalAndVerticalInitItem();
            //    break;
        }

    }

    void HorizontalInitItem() {

        for (int i = 0; i < column; i++) {
            obj = Instantiate(prefab);
            obj.transform.SetParent(grid.transform,false);
            obj.transform.localPosition = new Vector3(middle / 2 + item_width / 2 + (middle + item_width) * i, -(sv_height / 2), 0);
            ScrollItem item = new ScrollItem();
            item.BindGameObject(obj);
            item.SetData(i + 1);
            itemList.AddLast(item);
        }

    }

    void VerticalInitItem() {

        for (int i = 0; i < row; i++){
            obj = Instantiate(prefab);
            obj.transform.SetParent(grid.transform,false);
            obj.transform.localPosition = new Vector3(sv_width / 2, -(middle / 2 + item_height / 2 + (middle + item_height) * i), 0);
            ScrollItem item = new ScrollItem();
            item.BindGameObject(obj);
            item.SetData(i + 1);
            itemList.AddLast(item);
        }

    }

    //void HorizontalAndVerticalInitItem() {

    //    for (int i = 0; i < row; i++) {
    //        for (int j = 0; j < column; j++) {
    //            obj = Instantiate(gameObj);
    //            obj.transform.SetParent(grid.transform);
    //            obj.transform.localPosition = new Vector3(middle / 2 + item_width / 2 + (middle + item_width) * j, -(middle / 2 + item_height / 2 + (middle + item_height) * i), 0);
    //            Item item = new Item();
    //            item.BindGameObject(obj);
    //            item.SetData(i * columnCount + j + 1);
    //            itemList.Add(item);
    //        }
    //    }
    //}

    // Update is called once per frame
    //根据grid的移动调整Item的位置
    void Update()
    {

        Vector3 gridNewPosition = grid.transform.localPosition;

        float h = gridNewPosition.y - gridOldPosition.y;

        float w = gridNewPosition.x - gridOldPosition.x;

        gridOldPosition = grid.transform.localPosition;

        if (h > 0.05f)
        {   //当最后一个Item的值小于总量
            if (itemList.Last.Value.GetData() < number)
            {   //当第一个Item的位置已经超出了一gird上一个Item的距离
                while (itemList.First.Value.GetGameObjectPosition().y + gridNewPosition.y > (item_height + middle) / 2)
                {
                    Up();    //调整第一个Item位置
                }
            }
        }
        else if (h < -0.05f)
        {
            if (itemList.First.Value.GetData() > 1)
            {
                while (itemList.Last.Value.GetGameObjectPosition().y + gridNewPosition.y < -(sv_height + (item_height + middle) / 2))
                {
                    Down();
                }
            }
        }
        if (w > 0.05f)
        {
            if (itemList.First.Value.GetData() > 1)
            {
                while (itemList.Last.Value.GetGameObjectPosition().x + gridNewPosition.x > (sv_width + (item_width + middle) / 2))
                {
                    Right();
                }
            }
        }
        else if (w < -0.05f)
        {
            if (itemList.Last.Value.GetData() < number)
            {
                while (itemList.First.Value.GetGameObjectPosition().x + gridNewPosition.x < -((item_width + middle) / 2))
                {
                    Left();
                }
            }
        }

    }

    void Up(){
            //将第一个Item的位置放到最后一个Itemd的下方
        itemList.First.Value.SetGameObjectPosition(itemList.Last.Value.GetGameObjectPosition() + new Vector3(0, -(item_height + middle), 0));
        itemList.First.Value.SetData(itemList.Last.Value.GetData() + 1);
        itemList.AddLast(itemList.First.Value);
        itemList.RemoveFirst();

    }

    void Down() {

        itemList.Last.Value.SetGameObjectPosition(itemList.First.Value.GetGameObjectPosition() + new Vector3(0, item_height + middle, 0));
        itemList.Last.Value.SetData(itemList.First.Value.GetData() - 1);
        itemList.AddFirst(itemList.Last.Value);
        itemList.RemoveLast();

    }

    void Left(){

        itemList.First.Value.SetGameObjectPosition(itemList.Last.Value.GetGameObjectPosition() + new Vector3(item_width + middle, 0, 0));
        itemList.First.Value.SetData(itemList.Last.Value.GetData() + 1);
        itemList.AddLast(itemList.First.Value);
        itemList.RemoveFirst();

    }

    void Right() {

        itemList.Last.Value.SetGameObjectPosition(itemList.First.Value.GetGameObjectPosition() + new Vector3(-(item_width + middle), 0, 0));
        itemList.Last.Value.SetData(itemList.First.Value.GetData() - 1);
        itemList.AddFirst(itemList.Last.Value);
        itemList.RemoveLast();

    }

    int GetUpInt(float a, float b) {

        int i = 0;

        while (a > 0) {
            a -= b;
            i++;
        }

        return i;
    } 
}

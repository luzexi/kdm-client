
Read me before use ui framework


1，所有ui继承 ScreenBaseHandler，

		所有UI的prefab都必须与ui类的名字相同，否则无法获取prefab

		一个UI一个Canvas，每个Canvas上的设置为

		Model 为 Screen Space - Camera

		Canvas Scaler中

		Model 为 Scale With Screen Size

		Reference Resolution 为 x: 1334 y: 750

		Screen Match Model 为 Match Width Or Height

		Match 为 0.5

		Reference Pixels Per Unit 为 100

2, 所有ui有三个常用接口

		public override void Init()
		{
			base.Init();
		}

		public override void OpenScreen()
		{
			base.OpenScreen();
		}

		public override void CloseScreen()
		{
			base.CloseScreen();
		}

3，创建UI，CreateMenu示例：
		
		UIMainHandle main_handle = MenuManager.instance.CreateMenu<UIMainHandle>();
		main_handle.OpenScreen();

4，找UI，FindMenu示例：

		UILoadingHandle loading_handle = MenuManager.instance.FindMenu<UILoadingHandle>();
		if(loading_handle)
		{
			loading_handle.CloseScreen();
		}

5，输入事件接口，UI_Event。

		尽量在Button的元素上加入UI_Event，然后与UI脚本里的变量绑定，这样能减少一个AddComponent的调用。

		public UI_Event mBtnYes;

		public override void Init()
		{
			base.Init();

			mBtnYes.onClick = YesOnClick;
		}

		也可以使用下列方式接入事件。但会多调用一个AddComponent，问题不是很大，但也会多消耗点CPU。

		UI_Event ev = UI_Event.Get(mBtnInfo);
		ev.onClick = BtnInfoOnClick;

6，常用对话框，MessageBox：

		分几种对话框，

		第一种是Yes or No 对话框，有选择。

		示例
		MessageBox.instance.OpenMessage(MessageBox.TYPE.YesOrNo,"im title","im content", yesCallback, noCallback);

		一种是Ok 对话框，无选择。

		示例
		MessageBox.instance.OpenMessage(MessageBox.TYPE.Ok,"im title","im content", yesCallback);

		一种是Roll message，滚动文字。

		示例
		MessageBox.instance.OpenMessage(MessageBox.TYPE.RollMessage,"im title","im content");

7，由于静态变量会造成逻辑和架构的混乱，

		所以只有必要的时候才使用静态变量，否则只能使用架构中的方法来获取UI,以及变量

		UI之间传递参数，也使用public 函数传递，而非静态变量

8，由于协程控制难度高，容易导致逻辑顺序不一致，又会造成不必要的内存垃圾，

		所以不可以使用协程，除非在架构中加入某个系统时需要用到协程来架构。

		使用现有的Update帧更新做刷新控制。

9，由于GetComponent和transform.Find，效率不高，所以频繁调用会造成cpu不必要的消耗。

		不得频繁使用GetComponent和transform.Find。完全可以用变量存储形式替代，只做一次GetComponent和绑定，替换多次GetComponent和Find操作。

10，使用ScrollView 时，当点击按钮时无法响应ScrollView的滚动时，

		在按钮上加入onBeginDrag，onDrag，onEndDrag三个句柄，

		并且在这三个句柄中，调用ScrollRect的OnBeginDrag, OnDrag, 和 OnEndDrag三个接口，就可以实现ScrollView的滚动。

11，写UI时尽量，一个类一个UI，无其他需要绑定的脚本。

		因为小的脚本太多导致逻辑过于分散，Bug难以查找。

		一个文件，1千-2千行都不问题，到2千行以上，可以考虑拆分文件。

		如果1-2百行就拆分文件，那会导致逻辑过于分散，类与类之间的逻辑受到隔离，导致故障分散，难以查找。

		究其原因是人类的注意力有限。当逻辑集中在一个文件上时，你的注意力也集中在了一个文件上，而不是分散在多个文件上。调试，书写，整理都在一个文件上，脑袋思考更清晰。

12, UI_GameCanvas代替了 CanvasScale， GameCanvas 里做了matchWidthOrHeight不同分辨率做匹配处理，大于16:9的宽度匹配，小于16:9的高度匹配。

13, UIDefine里的 UI_SCREEN_WIDTH 和 UI_SCREEN_HEIGHT，为屏幕在UGUI里的宽和高最大坐标，正整数。

14，3D 和 UI的输入冲突，可以用UI_Event来解决，UI_Event在响应变量时会设置一个变量，3D根据这个变量来判断是否要响应3D输入逻辑


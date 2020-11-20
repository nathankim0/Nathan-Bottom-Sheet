<img src="https://user-images.githubusercontent.com/37360089/99781733-92cf2780-2b5b-11eb-8845-5c8d276f1530.png" Width="25%"/><img src="https://user-images.githubusercontent.com/37360089/99781745-95318180-2b5b-11eb-8797-5f192afac859.png" Width="25%"/>

# Nathan Bottom Sheet (Xamarin Forms)
---
Custom Bottom sheet that can be swept down and swept up
> ❌   회사 보안상,  전체 코드를 첨부하지 않았습니다.

## Options 
---
[Interface (INathanPicker)](https://github.com/Jinyeob/NathanBottomSheet/blob/master/INathanPicker.cs)
### Methods
- void VisiblePicker();
<br>NathanPicker를 띄웁니다.

- void InVisiblePicker();
<br>NathanPicker를 숨깁니다.

- void AddItem(string text); 
- void AddItem(string text, string imageSource); (overload)
- void AddItem(PickerItem pickerItem); (overload)
<br>NathanPicker 목록을 추가합니다.

- void DeleteItem(int index);
<br>index'에 위치한 목록을 삭제합니다.

- string GetItemText(int index);
<br>'index'에 위치한 목록의 이름을 가져옵니다.

### Properties
- DFDesignRectangleFromBottom375 PickerRectangle { get; set; }
<br>NathanPicker의 높이를 조절하기 위해 사용합니다.

- bool HasIcon { get; set; }
<br>NathanPicker 목록 아이콘 활성 상태를 가져오거나 설정합니다.

- IList ItemsSource { get; set; }
<br>표시할 항목의 목록을 가져오거나 설정합니다.

- string Title { get; set; }
<br>NathanPicker 제목을 가져오거나 설정합니다. 기본 값은 ""입니다.

- Color TitleColor { get; set; }
<br>NathanPicker 제목 생상을 가져오거나 설정합니다. 기본 값은 Color.Default 입니다.

- double TitleFontSize { get; set; }
<br>NathanPicker 제목 폰트 크기를 가져오거나 설정합니다.

- string TitleImageSource { get; set; }
<br>NathanPicker 제목 아이콘을 가져오거나 설정합니다. 기본 값은 "" 입니다.

- Color TextColor { get; set; }
<br>NathanPicker 목록 색상을 가져오거나 설정합니다. 기본 값은 Color.Default 입니다.

- FontAttributes FontAttributes { get; set; }
<br>NathanPicker 목록 FontAttributes를 가져오거나 설정합니다. 기본 값은 FontAttributes.None 입니다.

- double CharacterSpacing { get; set; }
<br>NathanPicker 목록 자간을 가져오거나 설정합니다. 기본 값은 0 입니다.

- double FontSize { get; set; }
<br>NathanPicker 목록 폰트 크기를 가져오거나 설정합니다.

- string FontFamily { get; set; }
<br>NathanPicker 목록 글꼴을 가져오거나 설정합니다. 기본 값은 null 입니다.

- bool SearchEnabled { get; set; }
<br>검색 기능 활성 상태를 가져오거나 설정합니다. 기본 값은 true 입니다.

- int SelectedIndex { get; set; }
<br>NathanPicker에서 선택한 항목의 인덱스를 가져오거나 설정합니다.

## Sample
---
``` csharp
_picker = new NathanPicker
            {
                PickerRectangle = DFDesignMainPage.bottomPanContainer08Rectangle,
                HasIcon=false,
                ItemsSource = new List<PickerItem>
                {
                    new PickerItem("Protect myself from Coronavirus Disease"),
                    new PickerItem("Destress/deload","iconListCategoryGeneral"),
                    new PickerItem("Live healthy without a diet"),
                    new PickerItem("Lose weight"),
                },
                TextColor = Color.Default,
                Title = "Related Dream",
                TitleColor = Color.Default,
                TitleFontSize = DFDesignMainPage.PickerTitleFontSize.Value,
                TitleImageSource = "iconListCategoryPCopy",
                SearchEnabled = true,
                FontSize = DFDesignMainPage.PickerTitleFontSize.Value,
                FontFamily = null,
                FontAttributes = FontAttributes.None,
                CharacterSpacing = -0.3,
                SortType=SortType.None
            };


_picker.SelectedIndexChanged += (sender, e) =>
{
     _button.ButtonLabel.Text = _picker.GetItemText(_picker.SelectedIndex);
};

...

button.Clicked+=(s,e)=>{ _picker.VisiblePicker(); };


```


## ScreenShot
---
### HasIcon == true
<img src="https://user-images.githubusercontent.com/37360089/99781733-92cf2780-2b5b-11eb-8845-5c8d276f1530.png" Width="30%"/>

### HasIcon == false
<img src="https://user-images.githubusercontent.com/37360089/99781745-95318180-2b5b-11eb-8797-5f192afac859.png" Width="30%"/>

### Sweep Down
<img src="https://user-images.githubusercontent.com/37360089/99780402-cc9f2e80-2b59-11eb-9a23-24d893c0ec38.gif" Width="30%"/>

### Background Touch
<img src="https://user-images.githubusercontent.com/37360089/99780424-d2950f80-2b59-11eb-82e6-651ce6b07d95.gif" Width="30%"/>

### Scroll
<img src="https://user-images.githubusercontent.com/37360089/99780430-d3c63c80-2b59-11eb-89c1-0d7d2d9a0d26.gif" Width="30%"/>

### Search
<img src="https://user-images.githubusercontent.com/37360089/99780437-d45ed300-2b59-11eb-92cc-cbd3ec589e53.gif" Width="30%"/>

### Item Select
<img src="https://user-images.githubusercontent.com/37360089/99780439-d5900000-2b59-11eb-9fa0-a19a2d289d70.gif" Width="30%"/>

### BottomSheet Default
NathanBottomSheet을 상속하여 활용할 수 있습니다.
<div>
<img src="https://user-images.githubusercontent.com/37360089/99780442-d6289680-2b59-11eb-853e-bdc1f1acdb75.gif" Width="30%"/>
</div>

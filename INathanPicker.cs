using System;
using System.Collections;
using Dreamfora;
using Xamarin.Forms;

namespace NathanPicker
{
    /// <summary>정렬 방식을 설명하는 값을 열거합니다.</summary>
    public enum SortType
    {
        /// <summary>정렬 방식은 수정되지 않습니다.</summary>
        None = 0x0,
        /// <summary>정렬 방식이 오름차순입니다.</summary>
        Ascending = 0x1,
        /// <summary>정렬 방식이 내림차순입니다.</summary>
        Descending = 0x2
    }


    public interface INathanPicker
    {
        #region Methods

        /// <summary>
        /// NathanPicker를 띄웁니다.
        /// </summary>
        void VisiblePicker();

        /// <summary>
        /// NathanPicker를 숨깁니다.
        /// </summary>
        void InVisiblePicker();

        #region AddItem Overload

        /// <summary>
        /// NathanPicker 목록을 추가합니다.
        /// </summary>
        /// <param name="text">목록 이름</param>
        void AddItem(string text);

        /// <summary>
        /// NathanPicker 목록을 추가합니다.
        /// </summary>
        /// <param name="text">목록 이름</param>
        /// <param name="imageSource">목록 왼쪽 아이콘</param>
        void AddItem(string text, string imageSource);

        /// <summary>
        /// NathanPicker 목록을 추가합니다.
        /// </summary>
        /// <param name="pickerItem">PickerItem 객체</param>
        void AddItem(PickerItem pickerItem);

        #endregion AddItem Overload

        /// <summary>
        /// 'index'에 위치한 목록을 삭제합니다.
        /// </summary>
        /// <param name="index">NathanPicker 목록 인덱스</param>
        void DeleteItem(int index);

        /// <summary>
        /// 'index'에 위치한 목록의 이름을 가져옵니다.
        /// </summary>
        /// <param name="index">NathanPicker 목록 인덱스</param>
        /// <returns></returns>
        string GetItemText(int index);

        #endregion Methods


        #region Properties

        /// <summary>
        /// DFDesignRectangleFromBottom375로 설정된 NathanPicker Rectangle을 가져오거나 설정합니다.
        /// 기본값은  DFDesignRectangleFromBottom375(0, (375 * (DFDevice.DeviceHeight / DFDevice.DeviceWidth) - DFDevice.StatusBarHeight)*0.6, 375, (375 * (DFDevice.DeviceHeight / DFDevice.DeviceWidth) - DFDevice.StatusBarHeight)*0.6); 로 선언된  DFDesignMainPage.bottomPanContainerRectangle 입니다.
        /// NathanPicker의 높이를 조절하기 위해 사용합니다.
        /// </summary>
        DFDesignRectangleFromBottom375 PickerRectangle { get; set; }

        /// <summary>
        /// NathanPicker 목록 아이콘 활성 상태를 가져오거나 설정합니다.
        /// 기본 값은 true 입니다.
        /// </summary>
        bool HasIcon { get; set; }

        /// <summary>
        /// 표시할 항목의 목록을 가져오거나 설정합니다.
        /// </summary>
        IList ItemsSource { get; set; }

        /// <summary>
        /// NathanPicker 제목을 가져오거나 설정합니다.
        /// 기본 값은 ""입니다.
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// NathanPicker 제목 생상을 가져오거나 설정합니다.
        /// 기본 값은 Color.Default 입니다.
        /// </summary>
        Color TitleColor { get; set; }

        /// <summary>
        /// NathanPicker 제목 폰트 크기를 가져오거나 설정합니다.
        /// 기본 값은 DFDesignDouble(16, 375) 로 선언된 DFDesignMainPage.PickerTitleFontSize.Value 입니다.
        /// </summary>
        double TitleFontSize { get; set; }

        /// <summary>
        /// NathanPicker 제목 아이콘을 가져오거나 설정합니다.
        /// 기본 값은 "" 입니다.
        /// </summary>
        string TitleImageSource { get; set; }

        /// <summary>
        /// NathanPicker 목록 색상을 가져오거나 설정합니다.
        /// 기본 값은 Color.Default 입니다.
        /// </summary>
        Color TextColor { get; set; }

        /// <summary>
        /// NathanPicker 목록 FontAttributes를 가져오거나 설정합니다.
        /// 기본 값은 FontAttributes.None 입니다.
        /// </summary>
        FontAttributes FontAttributes { get; set; }

        /// <summary>
        /// NathanPicker 목록 자간을 가져오거나 설정합니다.
        /// 기본 값은 0 입니다.
        /// </summary>
        double CharacterSpacing { get; set; }

        /// <summary>
        /// NathanPicker 목록 폰트 크기를 가져오거나 설정합니다.
        /// 기본 값은 DFDesignDouble(16, 375) 로 선언된 DFDesignMainPage.PickerFontSize.Value 입니다.
        /// </summary>
        double FontSize { get; set; }

        /// <summary>
        /// NathanPicker 목록 글꼴을 가져오거나 설정합니다.
        /// 기본 값은 null 입니다.
        /// </summary>
        string FontFamily { get; set; }

        /// <summary>
        /// 검색 기능 활성 상태를 가져오거나 설정합니다.
        /// 기본 값은 true 입니다.
        /// </summary>
        bool SearchEnabled { get; set; }

        /// <summary>
        /// NathanPicker에서 선택한 항목의 인덱스를 가져오거나 설정합니다.
        /// </summary>
        int SelectedIndex { get; set; }

        #endregion Properties
    }
}
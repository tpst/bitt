   M S B A M L   `   `   `  ����H  DBitTrader-1.0, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:BitTrader_1._0BitTrader_1._0  5          BitTrader_1._0.MarketView   5      b ^DevExpress.Xpf.Core.v16.1, Version=16.1.11.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      <dxb2http://schemas.devexpress.com/winfx/2008/xaml/bars  5      (localclr-namespace:BitTrader_1._0  5      .��5
      � $��0,10,0,0q�6     � RowDefinitions  5   
   �� 5      $��60 �6   �� 5      .+�5   
   � $��0��6   $��0,0,0,5q�6   
�������5      $  DevExpress.Xpf.Bars.BarManager  5   
   -   5          Name$ 
BarManager��5      $��1��6-      AllowCustomization .     AllowQuickCustomization .     CreateStandardLayout .     Bars 5        DevExpress.Xpf.Bars.Bar  5      -   5          Name$ mainMenuBar��5         Caption$ 	Main Menu��6/      
IsMainMenu . 	   DockInfo	 5      %  DevExpress.Xpf.Bars.BarDockInfo  5      
   ContainerType$	
 Top=�6*   /  )DevExpress.Xpf.Bars.BarItemLinkHolderBase   	ItemLinks 5      (  "DevExpress.Xpf.Bars.BarSubItemLink  5      %  DevExpress.Xpf.Bars.BarItemLink   BarItemName$ 	bsOptions��6-   5        5!      -   5        $ 	statusBar��5!      $ 
Status Bar��6-      IsStatusBar . 	 5"        5#      $
 Bottom=�6*   5%         Items 5'      $  DevExpress.Xpf.Bars.BarSubItem  5(      -   5         �Name$ 	bsOptions��5(   !   !  DevExpress.Xpf.Bars.BarItem   Content)	  #DevExpress.Xpf.Core.ObjectConverter$ Options	 62   ._�5+      � -   5         �Name$ mainViewGrid��5+       � ColumnDefinitions 5,      �� 5-      $
��0.3* �6'   �� 5.      .+�50      �� $��0��6   
�������52      � 54      -   5        $ DisplayGrid��54      $��1��6,     55      �� 56      $��7* �6(   �� 57      $��* �6(   .+�59      !
   BitTrader_1._0.ChartControl
  -   5         
 Name$ 	mainChart��59   )   $��0��6<   $��5,0,5,5q�6I   %   BitTrader_1._0.IndicatorControl  5:      -   5          Name$ 	indicator��5:   -   $��1��6@   5;      5<      5>      5?      5@      
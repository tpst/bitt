Imports DevExpress.Xpf.Charts
Imports BitTrader_1._0.TradeData
Imports Newtonsoft.Json
Imports BitTrader_1._0.DataRequest

Public Class ChartControl
    Private _da As DataAggregator

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        DataContext = Me
        _da = New DataAggregator
    End Sub


    Private Sub ChartControl_Loaded(sender As Object, e As RoutedEventArgs)
        chart.Diagram.Series(0).DataSource = _da.HistoricalDataRequest  'DataManager.Data

        'Dim argument As Date = Date.Now

        'axisX.WholeRange.SetMinMaxValues(argument.AddYears(-1), argument)

    End Sub



End Class

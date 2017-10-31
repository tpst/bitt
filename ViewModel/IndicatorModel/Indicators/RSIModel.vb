Imports BitTrader_1._0.TradeData
''' <summary>
''' Relative Strength Indicator
''' </summary>
Public Class RSIModel
    ' http://stockcharts.com/school/doku.php?id=chart_school:technical_indicators:relative_strength_index_rsi
    '              100
    'RSI = 100 - --------
    '             1 + RS
    'RS = Average Gain / Average Loss over e.g 14 day period.

    Public Property Period As TimeSpan

    Public Property Data As List(Of Double)


    Public Sub UpdateData(data As List(Of DataPoint))
        For Each pt In data
            Dim avgGain As Double
            Dim avgLoss As Double
        Next
    End Sub


End Class

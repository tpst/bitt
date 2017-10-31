Public Class TradeData

    Public Class DataPoint
        Public Property Name As String
        Public Property TimeStamp As String

        Public Property TS As TimeSpan
        Public Property Last As Double
        Public Property Open As Double
        Public Property Close As Double
        Public Property High As Double
        Public Property Low As Double
        Public Property Volume As Double

        Public Sub New(ByVal t As String, l As Double)
            TimeStamp = t
            Last = l
        End Sub

        Public Sub New()
        End Sub
    End Class

    Public Class JSON_result
        Public Name As String
        Public TimeStamp As String
        Public Symbols As Symbols
        ' Alternative? https://www.quandl.com/collections/markets/bitcoin-data
        ' Thanks http://json2csharp.com/

        Public Function ConvertToDataPt() As DataPoint
            Dim data As New DataPoint With {.TimeStamp = TimeStamp, .Name = Name, .Last = Symbols.btceur.last, .TS = TimeSpan.Parse(TimeStamp),
                                       .Volume = Symbols.btceur.volume}

            Return data
        End Function
    End Class

    Public Class BTCEUR
        Public last As Double
        Public volume As Double
        Public ask As Double
        Public bid As Double
    End Class

    Public Class Symbols
        Public btceur As BTCEUR
    End Class

End Class

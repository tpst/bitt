Imports Newtonsoft.Json

Public Class DataAggregator
    Private webClient As System.Net.WebClient

    Private _data As List(Of TradingData)

    Public Sub New()
        webClient = New Net.WebClient
    End Sub

    Public Function HistoricalDataRequest()
        Dim GetString As String = "https://apiv2.bitcoinaverage.com/indices/global/history/BTCEUR?period=alltime&format=json"
        Dim jsonResult As String = webClient.DownloadString(GetString)

        Dim resultdata As New List(Of HistoricalData)
        resultdata = JsonConvert.DeserializeObject(Of List(Of HistoricalData))(jsonResult)

        _data = resultdata.Select(Function(x) New TradingData With {.Price = x.average,
                                                                           .Volume = x.volume,
                                                                           .Date = x.time,
                                                                           .Low = If(x.low, 0),
                                                                            .High = If(x.high, 0)}).ToList
        Return _data
    End Function

    Public Const _pollingInterval As Integer = 10000

    Public Sub RequestTick()
        Dim getString As String = "https://apiv2.bitcoinaverage.com/exchanges/kraken?symbol=BTCEUR"
        Dim jsonResult As String = webClient.DownloadString(getString)

    End Sub


End Class

Public Class HistoricalData
    Public average As Double?
    Public low As Double?
    Public high As Double?
    Public time As Date
    Public volume As Double?
End Class



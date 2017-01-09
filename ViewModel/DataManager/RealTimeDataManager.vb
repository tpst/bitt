Imports DevExpress.Xpf.Charts
Imports BitTrader_1._0.DataRequest
Imports BitTrader_1._0.TradeData
Imports Newtonsoft.Json

Public Class RealTimeDataManager

    Private _pollingInterval As Integer = 2500
    Private Timer As New Windows.Threading.DispatcherTimer

    Dim webClient As System.Net.WebClient
    Dim jsonResult As String

    Private _data As List(Of Data)

    Public Api As CSVParser
    Public Sub New()
        Api = New CSVParser(Me)
        Dim dataRequest As New APIDataRequest(Me)
        Data = Api.CSVtoTradeData
    End Sub

    Public Property Data As List(Of Data)
        Get
            Return _data
        End Get
        Set(value As List(Of Data))
            _data = value
        End Set
    End Property

    Private inProcess As Boolean? = Nothing

    Public Sub DisableProcess()
        inProcess = Timer.IsEnabled
        Timer.Stop()
    End Sub

    Public Sub RestoreProcess()
        If inProcess IsNot Nothing Then
            Timer.IsEnabled = CBool(inProcess)
            inProcess = Nothing
        End If
    End Sub

End Class

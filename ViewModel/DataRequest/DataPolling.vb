Imports System.Windows.Threading
Imports BitTrader_1._0.TradeData
Imports Newtonsoft.Json
Imports DevExpress.Xpf.Charts
Imports Microsoft.VisualBasic.FileIO.TextFieldParser

Public Class DataRequest

    Public Const _pollingInterval As Integer = 20000

    Public Class APIDataRequest
        Dim jsonRequest As String = "https://apiv2.bitcoinaverage.com/exchanges/kraken?symbol=BTCEUR"

        Private _timer As DispatcherTimer
        Dim webClient As System.Net.WebClient
        Dim jsonResult As String
        Private _inProcess As Boolean? = Nothing

        Private _myDataManager As RealTimeDataManager

        Public Sub New(parent As RealTimeDataManager)
            _myDataManager = parent
            webClient = New System.Net.WebClient
            _timer = New DispatcherTimer
            _timer.Interval = TimeSpan.FromMilliseconds(_pollingInterval)
            AddHandler _timer.Tick, AddressOf RequestData
            _timer.Start()
        End Sub

        Private Sub RequestData()
            Dim argument As Date = Date.Now

            jsonResult = webClient.DownloadString(jsonRequest)
            Dim obj As JSON_result
            obj = JsonConvert.DeserializeObject(Of JSON_result)(jsonResult)

            Dim d = obj.ConvertToDataPt
            Dim seriesp As New SeriesPoint(obj.TimeStamp, obj.Symbols.btceur.last)
            _myDataManager.Data.Add(d)
        End Sub

        Public Sub ResumePolling()
            If _inProcess IsNot Nothing Then
                _timer.IsEnabled = CBool(_inProcess)
                _inProcess = Nothing
            End If
        End Sub

        Public Sub DisablePolling()
            _inProcess = _timer.IsEnabled
            _timer.Stop()
        End Sub

    End Class


    Public Class CSVParser
        Private jsonRequest As String = "https://apiv2.bitcoinaverage.com/exchanges/kraken?symbol=BTCEUR"

        Private _myDataManager As RealTimeDataManager

        Public Sub New(parent As RealTimeDataManager)
            _myDataManager = parent
        End Sub

        Public Function CSVtoTradeData() As List(Of Data)
            Dim list As New List(Of Data)

            Dim afile As FileIO.TextFieldParser = New FileIO.TextFieldParser("C:\dev\Personal\BitTrader-1.0\BCHARTS.csv")
            Dim currentLine As String() ' this array will hold each line of data
            afile.TextFieldType = FileIO.FieldType.Delimited
            afile.Delimiters = New String() {","}
            afile.HasFieldsEnclosedInQuotes = True

            ' parse the actual file
            Dim firstLine = True
            Do While Not afile.EndOfData
                Try
                    currentLine = afile.ReadFields
                    If firstLine = True Then
                        firstLine = False
                    Else
                        list.Add(New Data(currentLine(0), Double.Parse(currentLine(7))))
                    End If

                Catch ex As FileIO.MalformedLineException
                    Stop
                End Try
            Loop

            afile.Dispose()
            list.Reverse()

            Return list
        End Function

    End Class


End Class

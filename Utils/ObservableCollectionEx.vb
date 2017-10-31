Imports System.ComponentModel
Imports System.Collections.Specialized
Imports System.Collections.ObjectModel

Public Class MemberChangeEventArgs
    Inherits EventArgs

    Public Index As Integer
    Public Item As Object
    Public PropertyName As String

    Public Sub New(index As Integer, item As Object, propertyName As String)
        Me.Index = index
        Me.Item = item
        Me.PropertyName = propertyName
    End Sub
End Class


Public Interface IObservableCollectionEx
    Inherits INotifyCollectionChanged, INotifyPropertyChanged

    Event MemberChanged As EventHandler(Of MemberChangeEventArgs)
    Event MemberChanging As EventHandler(Of MemberChangeEventArgs)

End Interface


Public Interface IObservableCollectionEx(Of T)
    Inherits IObservableCollectionEx, IList(Of T)

End Interface


<Serializable()>
Public Class ObservableCollectionEx(Of T)
    Inherits Collection(Of T)
    Implements IObservableCollectionEx(Of T), IDisposable

    Private _monitor As New SimpleMonitor()
    Private _itemsChangingNotify As Boolean = GetType(T).GetInterface("INotifyPropertyChanging") IsNot Nothing
    Private _itemsChangedNotify As Boolean = GetType(T).GetInterface("INotifyPropertyChanged") IsNot Nothing
    Private _eventsDisabledCounter As Integer = 0

    Public Sub New()
    End Sub

    Public Sub New(ByVal items As IEnumerable(Of T))
        If (items Is Nothing) Then
            Throw New ArgumentNullException("items")
        End If
        AddRange(items)
    End Sub

    Public Sub New(capacity As Integer)
        MyBase.New(New List(Of T)(capacity))
    End Sub

    Public Sub BeginUpdate()
        _eventsDisabledCounter += 1
    End Sub

    Public Sub EndUpdate()
        EndUpdate(True, True)
    End Sub

#Region "Extended Operations"
    Public Sub AddRange(items As IEnumerable(Of T))
        If (items IsNot Nothing AndAlso items.Any()) Then
            BeginUpdate()
            Dim startIndex = Count
            Try
                For Each item As T In items
                    MyBase.Add(item)
                Next
            Finally
                EndUpdate(False, True)
                ' Fire events if any items were added.
                OnCollectionChanged(New NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, items.ToList(), startIndex))
            End Try
        End If
    End Sub

    Public Sub RemoveAll(items As IEnumerable(Of T))
        BeginUpdate()
        Try
            For Each item As T In items.ToList()
                Remove(item)
            Next
        Finally
            EndUpdate()
        End Try
    End Sub

    Public Sub RemoveAll(startIdx As Integer)
        Dim items As New List(Of T)()

        BeginUpdate()
        Try
            For idx As Integer = startIdx To Count - 1
                items.Add(Me(startIdx))
                RemoveAt(startIdx)
            Next
        Finally
            ' Fire remove event below
            EndUpdate(False, True)
        End Try

        OnCollectionChanged(NotifyCollectionChangedAction.Remove, items, startIdx)
    End Sub

    Public Sub ReplaceAll(items As IEnumerable(Of T))
        BeginUpdate()
        Dim countChanged As Boolean = False
        Try
            Dim idx As Integer = 0
            For Each item As T In items
                If (idx < Count) Then
                    ' Replace existing items
                    Me(idx) = item
                Else
                    ' Else append to the list
                    MyBase.Add(item)
                    countChanged = True
                End If
                idx += 1
            Next

            ' Remove items that are beyond the end of the new item list
            While (Count > idx)
                MyBase.RemoveAt(Count - 1)
                countChanged = True
            End While
        Finally
            ' Fire the list reset
            EndUpdate(True, countChanged)
        End Try
    End Sub

    Public Sub Move(ByVal oldIndex As Integer, ByVal newIndex As Integer)
        Me.MoveItem(oldIndex, newIndex)
    End Sub

    Public Function FindIndexOf(predicate As Func(Of T, Boolean)) As Integer
        For i As Integer = 0 To Count - 1
            If (predicate(Item(i))) Then
                Return i
            End If
        Next
        Return -1
    End Function

    Public Function FindLastIndexOf(predicate As Func(Of T, Boolean)) As Integer
        Dim idx As Integer = -1
        For i As Integer = 0 To Count - 1
            If (predicate(Item(i))) Then
                idx = i
            End If
        Next
        Return idx
    End Function
#End Region

#Region "Private Functions"
    Protected Sub EndUpdate(fireReset As Boolean, fireCount As Boolean)
        If (_eventsDisabledCounter > 0) Then
            _eventsDisabledCounter -= 1
            If (_eventsDisabledCounter = 0) Then
                If fireReset Then
                    OnCollectionChanged(New NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset))
                End If
                If fireCount Then
                    OnPropertyChanged("Count")
                End If
            End If
        End If
    End Sub

    Protected Function BlockReentrancy() As IDisposable
        Me._monitor.Enter()
        Return Me._monitor
    End Function

    Protected Sub CheckReentrancy()
        If (Me._monitor.Busy) Then
            Throw New InvalidOperationException("Observable Collection Reentrancy Not Allowed")
        End If
    End Sub
#End Region

#Region "Events"
    Public Event MemberChanged As EventHandler(Of MemberChangeEventArgs) Implements IObservableCollectionEx(Of T).MemberChanged
    Public Event MemberChanging As EventHandler(Of MemberChangeEventArgs) Implements IObservableCollectionEx(Of T).MemberChanging
    Public Event CollectionChanged As NotifyCollectionChangedEventHandler Implements INotifyCollectionChanged.CollectionChanged
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Private Sub Item_PropertyChanged(sender As Object, e As PropertyChangedEventArgs)
        OnMemberChanged(MyBase.Items.IndexOf(CType(sender, T)), CType(sender, T), e.PropertyName)
    End Sub

    Private Sub OnMemberChanged(index As Integer, item As T, propertyName As String)
        RaiseEvent MemberChanged(Me, New MemberChangeEventArgs(index, item, propertyName))
    End Sub

    Private Sub Item_PropertyChanging(sender As Object, e As PropertyChangingEventArgs)
        OnMemberChanging(MyBase.Items.IndexOf(CType(sender, T)), CType(sender, T), e.PropertyName)
    End Sub

    Private Sub OnMemberChanging(index As Integer, item As T, propertyName As String)
        RaiseEvent MemberChanging(Me, New MemberChangeEventArgs(index, item, propertyName))
    End Sub

    Protected Sub OnPropertyChanged(propertyName As String)
        If (_eventsDisabledCounter = 0) Then
            OnPropertyChanged(New PropertyChangedEventArgs(propertyName))
        End If
    End Sub

    Protected Sub OnPropertyChanged(e As PropertyChangedEventArgs)
        If (_eventsDisabledCounter = 0) Then
            RaiseEvent PropertyChanged(Me, e)
        End If
    End Sub

    Protected Sub OnCollectionChanged(ByVal action As NotifyCollectionChangedAction, ByVal item As T, ByVal index As Integer)
        If (_eventsDisabledCounter = 0) Then
            OnCollectionChanged(New NotifyCollectionChangedEventArgs(action, item, index))
        End If
    End Sub

    Protected Sub OnCollectionChanged(ByVal action As NotifyCollectionChangedAction, ByVal items As List(Of T), ByVal index As Integer)
        If (_eventsDisabledCounter = 0) Then
            OnCollectionChanged(New NotifyCollectionChangedEventArgs(action, items, index))
        End If
    End Sub

    Protected Sub OnCollectionChanged(ByVal action As NotifyCollectionChangedAction, ByVal item As T, ByVal index As Integer, ByVal oldIndex As Integer)
        If (_eventsDisabledCounter = 0) Then
            OnCollectionChanged(New NotifyCollectionChangedEventArgs(action, item, index, oldIndex))
        End If
    End Sub

    Protected Sub OnCollectionChanged(ByVal action As NotifyCollectionChangedAction, ByVal oldItem As T, ByVal newItem As T, ByVal index As Integer)
        If (_eventsDisabledCounter = 0) Then
            OnCollectionChanged(New NotifyCollectionChangedEventArgs(action, newItem, oldItem, index))
        End If
    End Sub

    Protected Sub OnCollectionChanged(ByVal action As NotifyCollectionChangedAction, ByVal oldItems As List(Of T), ByVal newItems As List(Of T), ByVal index As Integer)
        If (_eventsDisabledCounter = 0) Then
            OnCollectionChanged(New NotifyCollectionChangedEventArgs(action, newItems, oldItems, index))
        End If
    End Sub

    Protected Sub OnCollectionReset()
        If (_eventsDisabledCounter = 0) Then
            OnCollectionChanged(New NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset))
        End If
    End Sub

    Protected Sub OnCollectionChanged(e As NotifyCollectionChangedEventArgs)
        If (_eventsDisabledCounter = 0) Then
            RaiseEvent CollectionChanged(Me, e)
        End If
    End Sub

    Private Sub AddMemberHandlers(item As T)
        If (_itemsChangedNotify AndAlso item IsNot Nothing) Then
            AddHandler CType(item, INotifyPropertyChanged).PropertyChanged, AddressOf Item_PropertyChanged
        End If
        If (_itemsChangingNotify AndAlso item IsNot Nothing) Then
            AddHandler CType(item, INotifyPropertyChanging).PropertyChanging, AddressOf Item_PropertyChanging
        End If
    End Sub

    Private Sub RemoveMemberHandlers(item As T)
        If (_itemsChangedNotify AndAlso item IsNot Nothing) Then
            RemoveHandler CType(item, INotifyPropertyChanged).PropertyChanged, AddressOf Item_PropertyChanged
        End If
        If (_itemsChangingNotify AndAlso item IsNot Nothing) Then
            RemoveHandler CType(item, INotifyPropertyChanging).PropertyChanging, AddressOf Item_PropertyChanging
        End If
    End Sub
#End Region

#Region "Collection Overrides"
    Protected Overrides Sub ClearItems()
        Me.CheckReentrancy()

        For Each item As T In MyBase.Items
            RemoveMemberHandlers(item)
        Next
        MyBase.ClearItems()

        Me.OnPropertyChanged("Count")
        Me.OnPropertyChanged("Item[]")
        Me.OnCollectionReset()
    End Sub

    Protected Overrides Sub RemoveItem(ByVal index As Integer)
        Me.CheckReentrancy()
        Dim item As T = MyBase.Item(index)

        RemoveMemberHandlers(item)
        MyBase.RemoveItem(index)

        Me.OnPropertyChanged("Count")
        Me.OnPropertyChanged("Item[]")
        Me.OnCollectionChanged(NotifyCollectionChangedAction.Remove, item, index)
    End Sub

    Protected Overrides Sub SetItem(ByVal index As Integer, ByVal item As T)
        Me.CheckReentrancy()
        Dim oldItem As T = MyBase.Item(index)

        RemoveMemberHandlers(oldItem)
        AddMemberHandlers(item)
        MyBase.SetItem(index, item)

        Me.OnPropertyChanged("Item[]")
        Me.OnCollectionChanged(NotifyCollectionChangedAction.Replace, oldItem, item, index)
    End Sub

    Protected Overrides Sub InsertItem(ByVal index As Integer, ByVal item As T)
        Me.CheckReentrancy()

        AddMemberHandlers(item)
        MyBase.InsertItem(index, item)

        Me.OnPropertyChanged("Count")
        Me.OnPropertyChanged("Item[]")
        Me.OnCollectionChanged(NotifyCollectionChangedAction.Add, item, index)
    End Sub

    Protected Overridable Sub MoveItem(ByVal oldIndex As Integer, ByVal newIndex As Integer)
        Me.CheckReentrancy()
        Dim item As T = MyBase.Item(oldIndex)
        MyBase.RemoveItem(oldIndex)
        MyBase.InsertItem(newIndex, item)
        Me.OnPropertyChanged("Item[]")
        Me.OnCollectionChanged(NotifyCollectionChangedAction.Move, item, newIndex, oldIndex)
    End Sub
#End Region

#Region "Rentrancy Protection"
    <Serializable()>
    Private Class SimpleMonitor
        Implements IDisposable

        Public Sub Dispose() Implements IDisposable.Dispose
            Me._busyCount -= 1
        End Sub

        Public Sub Enter()
            Me._busyCount += 1
        End Sub

        Public ReadOnly Property Busy As Boolean
            Get
                Return (Me._busyCount > 0)
            End Get
        End Property

        Private _busyCount As Integer
    End Class
#End Region

#Region "IDisposable Support"
    Private _disposed As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me._disposed Then
            If disposing Then
                For Each item As T In Me
                    RemoveMemberHandlers(item)
                Next
            End If
        End If
        Me._disposed = True
    End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class


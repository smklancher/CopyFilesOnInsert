Imports System.IO
Imports Microsoft.WindowsAPICodePack.Taskbar


Public Class Form1

    Delegate Sub UpdateDelegate(sender As Object, e As Integer)

    Private Sub UpdateInvoke(sender As Object, e As Integer)
        ProgressBar.Value = e
        TryWin7Progress(e)
    End Sub

    Public Sub UpdatePercentage(Percentage As Integer)
        Me.BeginInvoke(New UpdateDelegate(AddressOf UpdateInvoke), New Object() {Me, Percentage})
    End Sub

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        If txtCopyTo.Text = "" Then
            txtCopyTo.Text = Environment.CurrentDirectory
        End If
        AddHandler CopyOnInsert.CopyProgress, AddressOf UpdatePercentage

        CopyOnInsert.Init()
    End Sub

    Private Sub txtSetCopyTo_Click(sender As System.Object, e As System.EventArgs) Handles txtSetCopyTo.Click
        FolderBrowserDialog1.SelectedPath = txtCopyTo.Text
        If FolderBrowserDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            txtCopyTo.Text = FolderBrowserDialog1.SelectedPath
        End If
    End Sub

    Private Sub txtCopyTo_TextChanged(sender As Object, e As System.EventArgs) Handles txtCopyTo.TextChanged
        CopyOnInsert.CopyTo = txtCopyTo.Text
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs)
        'Threading.Thread.Sleep(2000)





    End Sub


    Public Sub TryWin7Progress(Percentage As Integer)
        Try
            If TaskbarManager.IsPlatformSupported Then
                If Percentage = 100 Then
                    TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress)
                Else
                    TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Normal)
                    TaskbarManager.Instance.SetProgressValue(Percentage, 100)
                End If
            End If
        Catch ex As Exception
            Debug.Print(ex.ToString)
        End Try
    End Sub



#Region " Logging "

    Delegate Sub FormLogDelagate(msg As String)
    Public Sub FormLog(msg As String)

    End Sub

    Public Sub Log(msg As String)
        Debug.WriteLine(msg)

        'add the timestamp so the timestamp is the same for file and form
        msg = Now.ToString("o") & " - " & msg & Environment.NewLine

        'write to filesystem log immediately
        My.Computer.FileSystem.WriteAllText(Environment.CurrentDirectory & "\Log.txt", msg, True)

        'invoke to write to form log (which doesn't happen until the thread is ready)
        'Try
        '    Me.BeginInvoke(New FormLogDelagate(AddressOf FormLog), New Object() {msg})
        'Catch ex As Exception
        '    'Debug.WriteLine(ex.ToString)
        'End Try

        'TODO: don't know why this doesn't work
        ''http://www.codeproject.com/Articles/37642/Avoiding-InvokeRequired
        'Me.UIThread(Sub()
        '                Me.txtLog.Text = msg & Me.txtLog.Text
        '            End Sub)

    End Sub
#End Region


End Class

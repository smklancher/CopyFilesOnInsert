Imports System
Imports System.Management
Imports System.Threading
Imports System.IO
Imports Microsoft.WindowsAPICodePack.Shell
Imports Microsoft.WindowsAPICodePack.Taskbar

Class CopyOnInsert
    Public Shared CopyTo As String = ""

    Private Shared w As ManagementEventWatcher = Nothing
    Private Shared WithEvents DirCopier As New xDirectory
    Private Shared t As Thread

    Public Shared Sub Init()
        t = New Thread(AddressOf StartThread)

        t.Start()

        'Exit Sub

        'Dim we As New WMIEvent()

        'Dim q As WqlEventQuery
        'Dim observer As New ManagementOperationObserver()

        '' Bind to local machine
        'Dim opt As New ConnectionOptions()
        'opt.EnablePrivileges = True
        ''sets required privilege
        'Dim scope As New ManagementScope("root\CIMV2", opt)

        'Try
        '    q = New WqlEventQuery()
        '    q.EventClassName = "__InstanceModificationEvent"
        '    q.WithinInterval = New TimeSpan(0, 0, 1)

        '    ' DriveType - 5: CDROM
        '    q.Condition = "TargetInstance ISA 'Win32_LogicalDisk' and" & vbCr & vbLf & "            TargetInstance.DriveType = 5"
        '    w = New ManagementEventWatcher(scope, q)

        '    ' register async. event handler
        '    'AddHandler w.EventArrived, New EventArrivedEventHandler(AddressOf we.CDREventArrived)
        '    w.Start()

        'Catch e As Exception
        '    Form1.Log(e.Message)
        '    w.[Stop]()
        'End Try
    End Sub


    Public Shared Sub StartThread()
        Dim q As WqlEventQuery
        Dim observer As New ManagementOperationObserver()

        Dim scope As New ManagementScope("root\CIMV2")
        scope.Options.EnablePrivileges = True

        q = New WqlEventQuery()
        q.EventClassName = "__InstanceModificationEvent"
        q.WithinInterval = New TimeSpan(0, 0, 3)
        q.Condition = "TargetInstance ISA 'Win32_LogicalDisk' and TargetInstance.DriveType = " & DriveType.CDRom
        w = New ManagementEventWatcher(scope, q)

        AddHandler w.EventArrived, New EventArrivedEventHandler(AddressOf CDREventArrived)
        w.Start()
    End Sub


    'Private Sub w_EventArrived(sender As Object, e As EventArrivedEventArgs)
    '    'Get the Event object and display its properties (all)
    '    For Each pd As PropertyData In e.NewEvent.Properties
    '        Dim mbo As ManagementBaseObject = Nothing
    '        If (InlineAssignHelper(mbo, TryCast(pd.Value, ManagementBaseObject))) IsNot Nothing Then
    '            Me.listBox1.BeginInvoke(New Action(Function() listBox1.Items.Add("--------------Properties------------------")))
    '            For Each prop As PropertyData In mbo.Properties
    '                Me.listBox1.BeginInvoke(New Action(Of PropertyData)(Function(p) listBox1.Items.Add(Convert.ToString(p.Name) & " - " & Convert.ToString(p.Value))), prop)
    '            Next
    '        End If
    '    Next
    'End Sub


    ' Dump all properties
    Public Shared Sub CDREventArrived(sender As Object, e As EventArrivedEventArgs)

        Dim pd As PropertyData = e.NewEvent.Properties("TargetInstance")
        If pd Is Nothing Then Exit Sub

        Dim mbo As ManagementBaseObject = Nothing
        mbo = TryCast(pd.Value, ManagementBaseObject)
        If mbo Is Nothing Then Exit Sub


        Debug.WriteLine("=========Properties===========")
        For Each prop As PropertyData In mbo.Properties
            Debug.WriteLine(Convert.ToString(prop.Name) & " - " & Convert.ToString(prop.Value))
        Next

        'If we have access to the device, ready to copy files, etc
        If mbo.Properties("Access").Value IsNot Nothing Then
            'Disc was inserted
            If DirCopier.Status <> xDirectoryStatus.Stopped Then
                Form1.Log("Ignoring inserted disc because copying is in progress")
                Exit Sub
            End If

            Dim DriveToCopyFrom As String
            If mbo.Properties("Name").Value IsNot Nothing Then ' or "Caption" or "DeviceID"
                DriveToCopyFrom = mbo.Properties("Name").Value & "\"
            Else
                Form1.Log("Name property didn't have the drive letter")
                Exit Sub
            End If

            Dim Serial As String = ""
            If mbo.Properties("VolumeSerialNumber").Value IsNot Nothing Then
                Serial = mbo.Properties("VolumeSerialNumber").Value
            End If

            Dim VolumeName As String = ""
            If mbo.Properties("VolumeName").Value IsNot Nothing Then
                VolumeName = mbo.Properties("VolumeName").Value
            End If

            Dim CopyThisDiscTo As String = Path.Combine({CopyTo, Serial & " - " & VolumeName})

            Form1.Log("Copying " & DriveToCopyFrom & " to " & CopyThisDiscTo)

            TotalItemsToCopy = 0
            TotalItemsCopied = 0
            RaiseEvent CopyProgress(0)

            'Check that the source exists... somehow this triggered one of the times I ejected a CD, but only once... not sure why.
            If Directory.Exists(DriveToCopyFrom) Then
                DirCopier.StartCopy(DriveToCopyFrom, CopyThisDiscTo, True)
            Else
                Form1.Log("WARNING: Tried to copy a disc but folder does not exist")
            End If
        Else
            'Disc was removed

        End If


            '' Get the Event object and display it
            'Dim pd As PropertyData = e.NewEvent.Properties("TargetInstance")

            'If pd IsNot Nothing Then
            '    Dim mbo As ManagementBaseObject = TryCast(pd.Value, ManagementBaseObject)

            '    ' if CD removed VolumeName == null
            '    If mbo.Properties("VolumeName").Value IsNot Nothing Then
            '        Form1.Log("CD has been inserted")
            '    Else
            '        Form1.Log("CD has been ejected")
            '    End If
            'End If
    End Sub

    Private Shared Sub DirCopier_CopyComplete(sender As Object, e As System.IO.CopyCompleteEventArgs) Handles DirCopier.CopyComplete
        Dim Copier As xDirectory
        Copier = DirectCast(sender, xDirectory)
        Form1.Log("Finished copying " & Copier.Source.FullName & " to " & Copier.Destination.FullName)

        'System sounds might be set to nothing, so try to play a sound directly
        If File.Exists(Environ("systemroot") & "\Media\Windows Notify.wav") Then
            My.Computer.Audio.Play(Environ("systemroot") & "\Media\Windows Notify.wav")
        Else
            'If the sound wasn't found, fall back to system sounds
            System.Media.SystemSounds.Exclamation.Play()
        End If

        TryEjectCD()

    End Sub

    Private Shared Sub DirCopier_CopyError(sender As Object, e As System.IO.CopyErrorEventArgs) Handles DirCopier.CopyError
        Form1.Log("Copy Error: " & e.Exception.ToString)
        'Throw e.Exception
    End Sub


    Public Shared Event CopyProgress(Percentage As Integer)


    Private Shared TotalItemsToCopy As Long = 0
    Private Shared TotalItemsCopied As Long = 0

    Private Shared Sub DirCopier_IndexComplete(sender As Object, e As System.IO.IndexCompleteEventArgs) Handles DirCopier.IndexComplete
        TotalItemsToCopy = e.FileCount + e.FolderCount
        Form1.Log("Total items to copy: " & TotalItemsToCopy)
    End Sub


    Private Shared Sub DirCopier_ItemCopied(sender As Object, e As System.IO.ItemCopiedEventArgs) Handles DirCopier.ItemCopied
        TotalItemsCopied = TotalItemsCopied + 1

        Dim percentage As Integer = 0
        If TotalItemsToCopy > 0 Then
            percentage = (TotalItemsCopied / TotalItemsToCopy) * 100
        End If

        If percentage > 100 Then percentage = 100

        Form1.Log(percentage & "% done with file copy.  Copied " & TotalItemsCopied & " items.")
        RaiseEvent CopyProgress(percentage)
    End Sub




    Declare Function s Lib "winmm.dll" Alias "mciSendStringA" (ByVal ab As String, ByVal ass As String, ByVal s As Integer, ByVal aas As Integer) As Integer
    'This doesn't specify which drive and supposedly mci is a very old method
    Public Shared Sub TryEjectCD()
        Try
            Dim ret As String
            ret = New String(CChar(" "), 255)
            Dim Open As Integer = s("Set CDAudio Door Open Wait", ret, 0&, 0&)
        Catch ex As Exception
            Form1.Log(ex.ToString)
        End Try
    End Sub
End Class
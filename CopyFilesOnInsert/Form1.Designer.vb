<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.txtSetCopyTo = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.ProgressBar = New System.Windows.Forms.ProgressBar()
        Me.txtCopyTo = New System.Windows.Forms.TextBox()
        Me.txtLog = New System.Windows.Forms.TextBox()
        Me.SuspendLayout
        '
        'txtSetCopyTo
        '
        Me.txtSetCopyTo.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.txtSetCopyTo.Location = New System.Drawing.Point(244, 77)
        Me.txtSetCopyTo.Name = "txtSetCopyTo"
        Me.txtSetCopyTo.Size = New System.Drawing.Size(75, 23)
        Me.txtSetCopyTo.TabIndex = 1
        Me.txtSetCopyTo.Text = "Browse"
        Me.txtSetCopyTo.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 35)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(207, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "When a disc is inserted it will be copied to:"
        '
        'ProgressBar
        '
        Me.ProgressBar.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ProgressBar.Location = New System.Drawing.Point(12, 9)
        Me.ProgressBar.Name = "ProgressBar"
        Me.ProgressBar.Size = New System.Drawing.Size(575, 23)
        Me.ProgressBar.TabIndex = 3
        '
        'txtCopyTo
        '
        Me.txtCopyTo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCopyTo.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.CopyFilesOnInsert.My.MySettings.Default, "CopyToLocation", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.txtCopyTo.Location = New System.Drawing.Point(12, 51)
        Me.txtCopyTo.Name = "txtCopyTo"
        Me.txtCopyTo.Size = New System.Drawing.Size(575, 20)
        Me.txtCopyTo.TabIndex = 0
        Me.txtCopyTo.Text = Global.CopyFilesOnInsert.My.MySettings.Default.CopyToLocation
        '
        'txtLog
        '
        Me.txtLog.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLog.Location = New System.Drawing.Point(15, 106)
        Me.txtLog.Multiline = True
        Me.txtLog.Name = "txtLog"
        Me.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtLog.Size = New System.Drawing.Size(572, 0)
        Me.txtLog.TabIndex = 4
        Me.txtLog.Visible = False
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(599, 107)
        Me.Controls.Add(Me.txtLog)
        Me.Controls.Add(Me.ProgressBar)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtSetCopyTo)
        Me.Controls.Add(Me.txtCopyTo)
        Me.Name = "Form1"
        Me.Text = "Copy Files On Insert"
        Me.ResumeLayout(false)
        Me.PerformLayout

End Sub
    Friend WithEvents txtCopyTo As System.Windows.Forms.TextBox
    Friend WithEvents txtSetCopyTo As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents FolderBrowserDialog1 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents ProgressBar As System.Windows.Forms.ProgressBar
    Friend WithEvents txtLog As System.Windows.Forms.TextBox

End Class

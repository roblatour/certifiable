' Certifiable v1.4

' Copyright Rob Latour, 2025
' License MIT

' github/roblatour/certifiable

Imports System.IO
Imports System.Net.Security
Imports System.Net.Sockets
Imports System.Reflection
Imports System.Security.Cryptography.X509Certificates
Imports System.Text
Imports System.Threading
Imports TextCopy

Module Program

    Dim iHost As String = String.Empty
    Dim iPort As Integer = 443
    Dim iGenerateLanguage As String = String.Empty
    Dim iUsePROGMEM As Boolean = False
    Dim iVariableName As String = String.Empty
    Dim iDisplay As Boolean = False
    Dim iCertNum As Integer = 1
    Dim iCopyToClipboard As Boolean = False
    Dim iWriteToFile As Boolean = False
    Dim iOutputFilename As String = String.Empty
    Dim iOverwriteOutputFile As Boolean = False
    Dim iFuture As Boolean = False
    Dim iHelp As Boolean = False
    Dim iCertNumSpecified As Boolean = False

    Dim gExpiredCertificates As Integer = 0
    Dim gCurrentCertificates As Integer = 0
    Dim gFutureCertificates As Integer = 0

    Dim gCurrentTimeUTC As DateTime

    Dim gStartingColour As ConsoleColor

    Dim gPrettyCommandLine As String

    Dim allCertifcates As New List(Of X509Certificate2)()

    Private Function ServerCertificateValidationCallback(sender As Object, certificate As X509Certificate2, chain As X509Chain, sslPolicyErrors As SslPolicyErrors) As Boolean

        ' Accept all certificates  
        Return True

    End Function

    Function GetAllCertifations() As Boolean

        gCurrentTimeUTC = DateTime.UtcNow
        Dim timeout As Integer = 5000 ' Timeout in milliseconds

        Try

            Using client As New TcpClient()

                Dim connectTask As Task = client.ConnectAsync(iHost, iPort)
                If Not connectTask.Wait(timeout) Then
                    Throw New TimeoutException("The connection attempt timed out.")
                End If

                client.ReceiveTimeout = timeout
                client.SendTimeout = timeout

                Using sslStream As New SslStream(client.GetStream(), False, AddressOf ServerCertificateValidationCallback)
                    Dim cts As New CancellationTokenSource(timeout)
                    Dim authTask As Task = sslStream.AuthenticateAsClientAsync(iHost)
                    If Not authTask.Wait(timeout) Then
                        Throw New TimeoutException("The SSL authentication timed out.")
                    End If

                    Dim chain As New X509Chain()
                    chain.Build(sslStream.RemoteCertificate)

                    Dim certificateInfo As New StringBuilder()

                    For Each cert In chain.ChainElements
                        If gCurrentTimeUTC > cert.Certificate.NotAfter Then
                            gExpiredCertificates += 1
                        ElseIf (gCurrentTimeUTC >= cert.Certificate.NotBefore) AndAlso (gCurrentTimeUTC <= cert.Certificate.NotAfter) Then
                            allCertifcates.Add(cert.Certificate)
                            gCurrentCertificates += 1
                        Else
                            gFutureCertificates += 1
                            If iFuture Then
                                allCertifcates.Add(cert.Certificate)
                            End If
                        End If
                    Next
                End Using
            End Using
        Catch ex As TimeoutException

            ConsoleWriteLineInColour("Error: the connection attempt to " & iHost & ":" & iPort & " timed out", ConsoleColor.Red)

            Return False

        Catch ex As Exception

            ConsoleWriteLineInColour(" ", ConsoleColor.Red)
            ConsoleWriteLineInColour("Error:", ConsoleColor.Red)

            ConsoleWriteLineInColour(ex.Message, ConsoleColor.Red)

            If ex.Message.Contains("Cannot determine the frame size or a corrupted frame was received") Then
                ConsoleWriteLineInColour("It may be you are looking for a certificate where there is none. For example with a server that only supports http and not https connections.", ConsoleColor.Red)
            End If

            Return False

        End Try

        Return True

    End Function

    Private Sub DisplayCertificates()

        If allCertifcates.Count > 0 Then

            Dim count As Integer = 0

            For Each cert As X509Certificate2 In allCertifcates

                count += 1

                If (iCertNumSpecified AndAlso (count = iCertNum)) OrElse (Not iCertNumSpecified) Then

                    ConsoleWriteLineInColour(" ")
                    ConsoleWriteLineInColour("Certificate Number:  " & count, ConsoleColor.Cyan)
                    ConsoleWriteLineInColour("Friendly Name:       " & cert.FriendlyName)
                    ConsoleWriteLineInColour("Subject:             " & cert.Subject)
                    ConsoleWriteLineInColour("Issuer:              " & cert.Issuer)
                    If (gCurrentTimeUTC >= cert.NotBefore) AndAlso (gCurrentTimeUTC <= cert.NotAfter) Then
                        ConsoleWriteLineInColour("Not Before:          " & cert.NotBefore, ConsoleColor.Green)
                        ConsoleWriteLineInColour("Not After:           " & cert.NotAfter, ConsoleColor.Green)
                    Else
                        ConsoleWriteLineInColour("Not Before:          " & cert.NotBefore, ConsoleColor.Yellow)
                        ConsoleWriteLineInColour("Not After:           " & cert.NotAfter, ConsoleColor.Yellow)
                    End If
                    ConsoleWriteLineInColour("Signature Algorithm: " & cert.SignatureAlgorithm.ToString)
                    ConsoleWriteLineInColour("Serial Number:       " & cert.SerialNumber)
                    ConsoleWriteLineInColour("Thumbprint:          " & cert.Thumbprint)
                    ConsoleWriteLineInColour("PEM:                 " & cert.ExportCertificatePem)

                End If

            Next

        End If

        If gExpiredCertificates = 1 Then
            ConsoleWriteLineInColour(" ")
            ConsoleWriteLineInColour("One expired certificate was not shown", ConsoleColor.Yellow)
        ElseIf gExpiredCertificates = 1 Then
            ConsoleWriteLineInColour(" ")
            ConsoleWriteLineInColour(gExpiredCertificates & " expired certificate were not shown", ConsoleColor.Yellow)
        End If

        If iFuture AndAlso (gFutureCertificates = 0) Then
            ConsoleWriteLineInColour(" ")
            ConsoleWriteLineInColour("There are no future dated certificates", ConsoleColor.Yellow)
        End If

    End Sub

    Private Function CreateGenericOpeningComments(ByVal startCommentString As String) As String

        Dim returnValue As String

        returnValue = startCommentString & "The following code is for use with " & iHost.ToLower & ":" & iPort & vbCrLf
        returnValue &= startCommentString & vbCrLf
        returnValue &= startCommentString & "It has been generated by Certifiable v" & GetVersionNumber() & " ( https://github.com/roblatour/certifiable ) using the following command:" & vbCrLf
        returnValue &= startCommentString & gPrettyCommandLine & vbCrLf
        returnValue &= startCommentString & vbCrLf

        Return returnValue

    End Function

    Private Function CreateOpeningComments(ByVal startCommentString As String) As String

        Const dateAndTimeFormat As String = "yyyy-MM-dd HH:mm:ss"

        Dim returnValue As String = CreateGenericOpeningComments(startCommentString)

        If iCertNumSpecified Then

            Dim cert As X509Certificate2 = allCertifcates(iCertNum - 1)

            returnValue &= startCommentString & "The certificate below is valid between " & cert.NotBefore.ToString(dateAndTimeFormat) & " (UTC) and " & cert.NotAfter.ToString(dateAndTimeFormat) & " (UTC) inclusive" & vbCrLf

        Else

            Dim count As Integer = 0

            For Each cert As X509Certificate2 In allCertifcates
                count += 1
                returnValue &= startCommentString & "Certificate " & count & " below is valid between " & cert.NotBefore.ToString(dateAndTimeFormat) & " (UTC) and " & cert.NotAfter.ToString(dateAndTimeFormat) & " (UTC) inclusive" & vbCrLf
            Next

        End If

        returnValue &= startCommentString & vbCrLf

        Return returnValue

    End Function

    Private Function CreateOutputFileForCPlusPlusProgMem() As String

        Dim returnValue As String = String.Empty

        Try

            Dim outputString As String = String.Empty
            Dim startCommentString As String
            Dim endString As String
            Dim firstLine As String
            Dim indent As Integer
            Dim variableName As String = String.Empty
            Dim startCert As Integer
            Dim endCert As Integer

            startCommentString = "// "
            endString = ")EOF"";"

            If iVariableName = String.Empty Then
                variableName = iHost.ToUpper.Replace(".", "_")
                If IsNumeric(variableName.Substring(0, 1)) Then
                    variableName = "HOST_" & variableName
                End If
                variableName &= "_CERTIFICATE"
            Else
                variableName = iVariableName
            End If

            firstLine = "static const char *" & variableName & " PROGMEM = R""EOF(" & vbCrLf
            indent = 0

            outputString = CreateOpeningComments(startCommentString)
            outputString &= firstLine

            If iCertNumSpecified Then
                startCert = iCertNum - 1
                endCert = iCertNum - 1
            Else
                startCert = 0
                endCert = allCertifcates.Count - 1
            End If

            For i = startCert To endCert

                Dim cert As X509Certificate2 = allCertifcates(i)

                Using reader As New StringReader(cert.ExportCertificatePem)
                    Dim line As String = reader.ReadLine()
                    While line IsNot Nothing
                        outputString &= line & vbCrLf
                        line = reader.ReadLine()
                    End While
                End Using

            Next

            outputString &= endString & vbCrLf

            returnValue = outputString

        Catch ex As Exception

            ConsoleWriteLineInColour(" ", ConsoleColor.Red)
            ConsoleWriteLineInColour("Error:", ConsoleColor.Red)
            ConsoleWriteLineInColour(ex.Message, ConsoleColor.Red)

        End Try

        Return returnValue

    End Function

    Private Function CreateOutputFileForCPlusPlus() As String

        Dim returnValue As String = String.Empty

        Try

            Dim outputString As String
            Dim startCommentString As String
            Dim endString As String
            Dim firstLine As String
            Dim indent As Integer
            Dim variableName As String = String.Empty
            Dim startCert As Integer
            Dim endCert As Integer

            startCommentString = "// "
            endString = "\n"";"

            If iVariableName = String.Empty Then
                variableName = iHost.ToUpper.Replace(".", "_")
                If IsNumeric(variableName.Substring(0, 1)) Then
                    variableName = "HOST_" & variableName
                End If
                variableName &= "_CERTIFICATE"
            Else
                variableName = iVariableName
            End If

            firstLine = "static const char *" & variableName & " = "
            indent = firstLine.Length

            outputString = CreateOpeningComments(startCommentString)
            outputString &= firstLine

            If iCertNumSpecified Then
                startCert = iCertNum - 1
                endCert = iCertNum - 1
            Else
                startCert = 0
                endCert = allCertifcates.Count - 1
            End If

            Dim skipFirstLineIndent As Boolean = True

            For i = startCert To endCert

                Dim cert As X509Certificate2 = allCertifcates(i)

                Using reader As New StringReader(cert.ExportCertificatePem)
                    Dim line As String = reader.ReadLine()
                    While line IsNot Nothing

                        If skipFirstLineIndent Then
                            skipFirstLineIndent = False
                        Else
                            outputString &= StrDup(indent, " ")
                        End If

                        outputString &= """" & line

                        If line.Contains("-----END CERTIFICATE-----") AndAlso (i = endCert) Then
                            outputString &= endString
                        Else
                            outputString &= "\n"" \"
                        End If

                        outputString &= vbCrLf

                        line = reader.ReadLine()
                    End While
                End Using

            Next

            returnValue = outputString

        Catch ex As Exception

            ConsoleWriteLineInColour(" ", ConsoleColor.Red)
            ConsoleWriteLineInColour("Error:", ConsoleColor.Red)
            ConsoleWriteLineInColour(ex.Message, ConsoleColor.Red)

        End Try

        Return returnValue

    End Function
    Private Function CreateOutputFileForPython() As String

        Dim returnValue As String = String.Empty

        Try

            Dim outputString As String
            Dim startCommentString As String
            Dim endString As String
            Dim firstLine As String
            Dim variableName As String
            Dim indent As Integer
            Dim startCert As Integer
            Dim endCert As Integer

            startCommentString = "# "
            endString = """"""""

            If iVariableName = String.Empty Then
                variableName = iHost.ToUpper.Replace(".", "_")
                If IsNumeric(variableName.Substring(0, 1)) Then
                    variableName = "HOST_" & variableName
                End If
                variableName &= "_CERTIFICATE"
            Else
                variableName = iVariableName
            End If

            firstLine = variableName & " = "
            indent = 0

            outputString = CreateOpeningComments(startCommentString)
            outputString &= firstLine

            If iCertNumSpecified Then
                startCert = iCertNum - 1
                endCert = iCertNum - 1
            Else
                startCert = 0
                endCert = allCertifcates.Count - 1
            End If

            For i = startCert To endCert

                Dim cert As X509Certificate2 = allCertifcates(i)

                Using reader As New StringReader(cert.ExportCertificatePem)
                    Dim line As String = reader.ReadLine()
                    While line IsNot Nothing

                        If line.Contains("-----BEGIN CERTIFICATE-----") Then
                            outputString &= """"""""
                        End If

                        outputString &= line

                        If line.Contains("-----END CERTIFICATE-----") AndAlso (i = endCert) Then
                            outputString &= endString
                        End If

                        outputString &= vbCrLf

                        line = reader.ReadLine()
                    End While
                End Using

            Next

            returnValue = outputString

        Catch ex As Exception

            ConsoleWriteLineInColour(" ", ConsoleColor.Red)
            ConsoleWriteLineInColour("Error:", ConsoleColor.Red)
            ConsoleWriteLineInColour(ex.Message, ConsoleColor.Red)

        End Try


        Return returnValue

    End Function

    Private Function CreateOutputFileForVBNet() As String

        Dim returnValue As String = String.Empty

        Try

            Dim outputString As String
            Dim startCommentString As String
            Dim endString As String
            Dim firstLine As String
            Dim variableName As String
            Dim indent As Integer
            Dim startCert As Integer
            Dim endCert As Integer

            startCommentString = "' "
            endString = """"

            If iVariableName = String.Empty Then
                variableName = iHost.ToUpper.Replace(".", "_")
                If IsNumeric(variableName.Substring(0, 1)) Then
                    variableName = "HOST_" & variableName
                End If
                variableName &= "_CERTIFICATE"
            Else
                variableName = iVariableName
            End If

            firstLine = "Dim " & variableName & " as String = "
            indent = firstLine.Length

            outputString = CreateOpeningComments(startCommentString)
            outputString &= firstLine

            If iCertNumSpecified Then
                startCert = iCertNum - 1
                endCert = iCertNum - 1
            Else
                startCert = 0
                endCert = allCertifcates.Count - 1
            End If

            Dim skipFirstLineIndent As Boolean = True

            For i = startCert To endCert

                Dim cert As X509Certificate2 = allCertifcates(i)

                Using reader As New StringReader(cert.ExportCertificatePem)

                    Dim line As String = reader.ReadLine()
                    While line IsNot Nothing

                        If skipFirstLineIndent Then
                            skipFirstLineIndent = False
                        Else
                            outputString &= StrDup(indent, " ")
                        End If

                        outputString &= """" & line

                        If line.Contains("-----END CERTIFICATE-----") AndAlso (i = endCert) Then
                            outputString &= endString
                        Else
                            outputString &= "\n"" \"
                        End If

                        outputString &= vbCrLf

                        line = reader.ReadLine()
                    End While
                End Using

            Next

            returnValue = outputString

        Catch ex As Exception

            ConsoleWriteLineInColour(" ", ConsoleColor.Red)
            ConsoleWriteLineInColour("Error:", ConsoleColor.Red)
            ConsoleWriteLineInColour(ex.Message, ConsoleColor.Red)

        End Try


        Return returnValue

    End Function

    Private Sub Generate()

        Dim outputString As String = String.Empty
        Dim codeType As String = String.Empty

        Select Case iGenerateLanguage

            Case "c++"
                codeType = "C++"
                If iUsePROGMEM Then
                    outputString = CreateOutputFileForCPlusPlusProgMem()
                Else
                    outputString = CreateOutputFileForCPlusPlus()
                End If

            Case "python"
                codeType = "Python"
                outputString = CreateOutputFileForPython()

            Case "vb.net"
                codeType = "VB.Net"
                outputString = CreateOutputFileForVBNet()

            Case Else
                ConsoleWriteLineInColour("Invalid code format " & iGenerateLanguage, ConsoleColor.Yellow)

        End Select

        ' ConsoleWriteLineInColour(outputString) 

        Try

            If iCopyToClipboard Then

                ClipboardService.SetText(outputString)
                ConsoleWriteLineInColour(" ")
                ConsoleWriteLineInColour(codeType & " certificate code for " & iHost & ":" & iPort & " copied to the clipboard", ConsoleColor.Green)

            End If

        Catch ex As Exception

            ConsoleWriteLineInColour("Error: Copy to clipboard failed", ConsoleColor.Red)
            ConsoleWriteLineInColour(ex.Message, ConsoleColor.Red)

        End Try

        Try

            If iWriteToFile Then

                If outputString.Length > 0 Then

                    Dim oKToWrite As Boolean = True

                    ' Check if the file exists

                    If iOverwriteOutputFile Then
                    Else

                        If File.Exists(iOutputFilename) Then

                            ConsoleWriteLineInColour(iOutputFilename & " already exists. Do you want to overwrite it? (y/n)", ConsoleColor.Yellow)

                            Dim response As String = Console.ReadLine().ToLower()

                            If response = "y" OrElse response = "yes" Then
                            Else
                                ConsoleWriteLineInColour(iOutputFilename & " will not be overwritten")
                                ConsoleWriteLineInColour(codeType & " certificate code for " & iHost & ":" & iPort & " was not exported to " & iOutputFilename, ConsoleColor.Yellow)
                                oKToWrite = False
                            End If

                        End If

                    End If

                    If oKToWrite Then

                        ' Write the output to the output file

                        Dim path As String = IO.Path.GetDirectoryName(iOutputFilename)

                        If (path IsNot Nothing) AndAlso (path.Length > 0) Then
                            If Not Directory.Exists(path) Then
                                Directory.CreateDirectory(path)
                            End If
                        End If

                        Using file As New StreamWriter(iOutputFilename, False)
                            file.WriteLine(outputString)
                        End Using

                        ConsoleWriteLineInColour(" ")
                        ConsoleWriteLineInColour(codeType & " certificate code for " & iHost & ":" & iPort & " exported to " & iOutputFilename, ConsoleColor.Green)

                    End If

                End If

            End If


        Catch ex As Exception

            ConsoleWriteLineInColour("Error: Failed to write to " & iOutputFilename, ConsoleColor.Red)
            ConsoleWriteLineInColour(ex.Message, ConsoleColor.Red)

        End Try

    End Sub

    Private Function ValidateCertNum() As Boolean

        Dim returnCode As Boolean

        If iFuture Then
            returnCode = (iCertNum <= gCurrentCertificates + gFutureCertificates)
        Else
            returnCode = (iCertNum <= gCurrentCertificates)
        End If

        If returnCode Then
        Else

            ConsoleWriteLineInColour("The -n option value was set to " & iCertNum, ConsoleColor.Red)
            ConsoleWriteLineInColour("However, for " & iHost & ":" & iPort & " the -n option value must be between 1 and " & (gCurrentCertificates).ToString & " (inclusive)", ConsoleColor.Red)

            If gFutureCertificates > 0 Then
                ConsoleWriteLineInColour("unless the -f option is used, in which case the -n option value must be between 1 and " & (gCurrentCertificates + gFutureCertificates).ToString & " (inclusive)", ConsoleColor.Red)
            End If
            ConsoleWriteLineInColour(" ")
            ConsoleWriteLineInColour("For more information, here is the information on all current and future dated certifications:")

            iCertNumSpecified = False
            iFuture = True
            DisplayCertificates()

        End If

        Return returnCode

    End Function
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub ConsoleWriteLineInColour(ByVal Message As String, Optional ByVal Colour As ConsoleColor = ConsoleColor.Gray, Optional PrintSpecialCharacters As Boolean = False)

        Dim originalTextEncoding As System.Text.Encoding = System.Console.OutputEncoding

        Try

            If PrintSpecialCharacters Then
                System.Console.OutputEncoding = System.Text.Encoding.Unicode
            End If

            If (Console.BackgroundColor = Colour) Then
                Colour = ConsoleColor.Black
            End If

            Console.ForegroundColor = Colour

            If Message.Length > 0 Then
                Console.WriteLine(Message)
            End If

            If PrintSpecialCharacters Then
                System.Console.OutputEncoding = originalTextEncoding
            End If

        Catch ex As Exception

            ConsoleWriteLineInColour(" ", ConsoleColor.Red)
            ConsoleWriteLineInColour("Error:", ConsoleColor.Red)
            ConsoleWriteLineInColour(ex.Message, ConsoleColor.Red)

        End Try

    End Sub

    Private Function ValidateArguments(args As String()) As Boolean

        If (args.Length = 0) OrElse (args(0) = "?") OrElse (args(0) = "-?") Then
            iHelp = True
            Return True
        End If

        ' List of valid arguments
        Dim validArguments As HashSet(Of String) = New HashSet(Of String) From {"-p", "-g", "-v", "-d", "-n", "-c", "-w", "-o", "-f"}

        Dim invalidArgumentFound As Boolean = False

        For Each arg As String In args
            If arg.StartsWith("-"c) AndAlso Not validArguments.Contains(arg) Then
                ConsoleWriteLineInColour("Invalid argument: " & arg, ConsoleColor.Red)
                invalidArgumentFound = True
            End If
        Next

        If invalidArgumentFound Then
            ConsoleWriteLineInColour("please correct", ConsoleColor.Red)
            Return False
        End If

        iHost = args(0)

        Try

            For i As Integer = 0 To args.Length - 1

                Select Case args(i).ToLower()

                    Case "-p"
                        If i + 1 < args.Length Then
                            If (Integer.TryParse(args(i + 1), iPort)) Then
                            Else
                                ConsoleWriteLineInColour("the -p option, if used, must be followed an integer value from 1 to 65535 inclusive", ConsoleColor.Red)
                                Return False
                            End If
                        Else
                            ConsoleWriteLineInColour("the -p option is missing the port number", ConsoleColor.Red)
                            Return False
                        End If

                    Case "-g"
                        If i + 1 < args.Length Then
                            iGenerateLanguage = args(i + 1)
                        Else
                            ConsoleWriteLineInColour("the -g option is missing the language", ConsoleColor.Red)
                            Return False
                        End If

                        If iGenerateLanguage = "c++" Then
                            If i + 2 < args.Length Then
                                Dim testForProgMem As String = args(i + 2).ToLower()
                                If testForProgMem = "progmem" Then
                                    iUsePROGMEM = True
                                Else
                                    If testForProgMem.StartsWith("-"c) Then
                                        iUsePROGMEM = False
                                    Else
                                        ConsoleWriteLineInColour("Invalid option following '-g c++'", ConsoleColor.Red)
                                        Return False
                                    End If
                                End If
                            Else
                                iUsePROGMEM = False
                            End If
                        End If

                    Case "-v"
                        If i + 1 < args.Length Then
                            iVariableName = args(i + 1)
                        Else
                            ConsoleWriteLineInColour("the -v option is missing the variable name", ConsoleColor.Red)
                            Return False
                        End If

                    Case "-d"
                        iDisplay = True

                    Case "-n"
                        If i + 1 < args.Length Then
                            If (Integer.TryParse(args(i + 1), iCertNum)) Then
                                iCertNumSpecified = True
                            Else
                                ConsoleWriteLineInColour("the -n option, if used, must be followed an integer value greater than 0", ConsoleColor.Red)
                                Return False
                            End If
                        Else
                            ConsoleWriteLineInColour("the -n option is missing the certificate number", ConsoleColor.Red)
                            Return False
                        End If

                    Case "-c"
                        iCopyToClipboard = True

                    Case "-w"
                        If i + 1 < args.Length Then
                            Try
                                iWriteToFile = True
                                iOutputFilename = args(i + 1)
                            Catch ex As Exception
                                ConsoleWriteLineInColour("the -w option, if used, must be followed by a filename which may include a path", ConsoleColor.Red)
                                Return False
                            End Try
                        Else
                            ConsoleWriteLineInColour("the -w option is missing the filename", ConsoleColor.Red)
                            Return False
                        End If

                    Case "-o"
                        iOverwriteOutputFile = True

                    Case "-f"
                        iFuture = True

                    Case "?"
                        iHelp = True

                End Select
            Next
        Catch ex As Exception
            ConsoleWriteLineInColour("expecting more information", ConsoleColor.Red)
            Return False
        End Try

        If Not ((Uri.CheckHostName(iHost) = UriHostNameType.Dns) OrElse (Uri.CheckHostName(iHost) = UriHostNameType.IPv4) OrElse (Uri.CheckHostName(iHost) = UriHostNameType.IPv6)) Then
            ConsoleWriteLineInColour("the host must be a valid domain name or IP address", ConsoleColor.Red)
            Return False
        End If

        If IsNumeric(iPort) AndAlso ((iPort > 0) AndAlso (iPort < 65536)) Then
            ' OK
        Else
            ConsoleWriteLineInColour("the -p option, if used, must be followed an integer value from 1 to 65535 inclusive", ConsoleColor.Red)
            Return False
        End If

        If IsNumeric(iCertNum) AndAlso ((iCertNum > 0)) Then
            ' OK
        Else
            ConsoleWriteLineInColour("the -c option, if used, must be followed by an integer value greater than 0", ConsoleColor.Red)
            Return False
        End If

        If (iDisplay) OrElse (iWriteToFile) OrElse (iCopyToClipboard) Then
        Else
            iDisplay = True
        End If

        If iGenerateLanguage.Length > 0 Then
            iGenerateLanguage = iGenerateLanguage.ToLower()
            If (iGenerateLanguage = "c++") OrElse (iGenerateLanguage = "vb.net") OrElse (iGenerateLanguage = "python") Then
            Else
                ConsoleWriteLineInColour("the -f option, if used, must be followed by 'c++', 'python', or 'vb.net' (without the quotes)", ConsoleColor.Red)
                Return False
            End If
        Else
            iGenerateLanguage = "c++"
        End If

        If iGenerateLanguage <> "c++" Then
            For Each arg As String In args
                If arg.ToLower() = "progmem" Then
                    ConsoleWriteLineInColour("The progmem option can only be used when generating c++", ConsoleColor.Red)
                    Return False
                    Exit For
                End If
            Next
        End If

        ' gPrettyCommandLine is set to a printable version of the command line that can be copied and pasted into a command prompt

        gPrettyCommandLine = Environment.CommandLine
        gPrettyCommandLine = gPrettyCommandLine.Replace("ertifiable.dll ", "ertifiable.exe ")

        If gPrettyCommandLine.StartsWith("""""") Then
            gPrettyCommandLine = gPrettyCommandLine.Remove(0, 1)
            Dim firstIndex As Integer = gPrettyCommandLine.IndexOf("""""")
            If firstIndex <> -1 Then
                gPrettyCommandLine = gPrettyCommandLine.Remove(firstIndex, 1)
            End If
        End If

        Return True

    End Function

    <System.Diagnostics.DebuggerStepThrough()>
    Function GetVersionNumber() As String
        Dim assembly As Assembly = Assembly.GetExecutingAssembly()
        Dim version As Version = assembly.GetName().Version
        Dim versionString As String = version.ToString()

        While versionString.EndsWith(".0")
            versionString = versionString.Substring(0, versionString.Length - 2)
        End While

        If versionString = String.Empty Then
            versionString = 0
        End If

        Return versionString
    End Function

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub DisplayHelp()

        Dim startingColour As ConsoleColor = Console.ForegroundColor

        Dim versionNumber As String = GetVersionNumber()

        ConsoleWriteLineInColour(" ")
        ConsoleWriteLineInColour("Certifiable v" & GetVersionNumber() & " Help", ConsoleColor.White)
        ConsoleWriteLineInColour(" ")
        ConsoleWriteLineInColour("Given a host name or IP address, and some additional information as outlined below,")
        ConsoleWriteLineInColour("Certifiable will generate the code for assigning a variable a SSL certificate PEM")
        ConsoleWriteLineInColour(" ")
        ConsoleWriteLineInColour("Usage:")
        ConsoleWriteLineInColour("certifiable [ host (-p n) (-d) (-n n) (-g x) (-v x) (-c) (-w) (-o) (-f) ] | [ ] | [ ? ]")
        ConsoleWriteLineInColour(" ")
        ConsoleWriteLineInColour("host   the host name or IP address from which to get the certificate(s)")
        ConsoleWriteLineInColour("       for example: google.com, www.google.com, 142.251.41.14")
        ConsoleWriteLineInColour("       a host value is required in all cases other than displaying this help information")
        ConsoleWriteLineInColour(" ")
        ConsoleWriteLineInColour("Options:", ConsoleColor.White)
        ConsoleWriteLineInColour(" ")
        ConsoleWriteLineInColour(" -p n  the host's port number")
        ConsoleWriteLineInColour("       for example: -p 8096")
        ConsoleWriteLineInColour("       if not used a port number of 443 will be assumed")
        ConsoleWriteLineInColour(" ")
        ConsoleWriteLineInColour(" -d    display the host's certificate(s) with unique number for each")
        ConsoleWriteLineInColour("       if neither -c or -o options are used, the -d option will be assumed")
        ConsoleWriteLineInColour(" ")
        ConsoleWriteLineInColour(" -f    by default future dated certificates are excluded")
        ConsoleWriteLineInColour("       if -f is used, then future dated certificates will be included")
        ConsoleWriteLineInColour("       note: expired certificates are always excluded")
        ConsoleWriteLineInColour(" ")
        ConsoleWriteLineInColour(" -n n  in many cases a host will have multiple certificates")
        ConsoleWriteLineInColour("       -n indicates a specific certificate should be used for code generation, the second n specifies which one")
        ConsoleWriteLineInColour("       for example, -n 3")
        ConsoleWriteLineInColour("       indicates only the 3rd certificate should be used for code generation")
        ConsoleWriteLineInColour("       if not used, code for all certificates will be generation")
        ConsoleWriteLineInColour(" ")
        ConsoleWriteLineInColour(" -g x  used to specifying the language in which the code will be generated")
        ConsoleWriteLineInColour("       supported languages are: c++, python, and vb.net")
        ConsoleWriteLineInColour("       for example: -g vb.net")
        ConsoleWriteLineInColour("       Additionally, c++ may be followed by the option progmem")
        ConsoleWriteLineInColour("       if c++ is followed by progmem the generated code will store the certificate in program memory")
        ConsoleWriteLineInColour("       (flash memory) rather than in RAM (SRAM)")
        ConsoleWriteLineInColour("       for example: -g c++ progmem")
        ConsoleWriteLineInColour("       if not used, the c++ will be assumed")
        ConsoleWriteLineInColour(" ")
        ConsoleWriteLineInColour(" -v x  the variable name to be used in the output file")
        ConsoleWriteLineInColour("       for example: -v server_root_cert")
        ConsoleWriteLineInColour("       if not used, a variable name based on the host name / IP address will be generated")
        ConsoleWriteLineInColour(" ")
        ConsoleWriteLineInColour(" -c    the generated code will be copied to the clipboard")
        ConsoleWriteLineInColour(" ")
        ConsoleWriteLineInColour(" -w x  the generated code will be written to the specified (path and) file name")
        ConsoleWriteLineInColour("       for example, -w c:\temp\certificate.h")
        ConsoleWriteLineInColour("       the output path name is optional, if omitted the output file will be written to the current working directory")
        ConsoleWriteLineInColour("       for example, -w certificate.h")
        ConsoleWriteLineInColour("       if the path is specified but does not exist, it will be created")
        ConsoleWriteLineInColour(" ")
        ConsoleWriteLineInColour(" -o    if the -w option is used and the specified file already exists")
        ConsoleWriteLineInColour("       the user will be prompted to overwrite the file unless the -o options is used")
        ConsoleWriteLineInColour("       in which case the existing file will be automatically overwritten")
        ConsoleWriteLineInColour("       for example, -w c:\temp\certificate.h -o")
        ConsoleWriteLineInColour(" ")
        ConsoleWriteLineInColour(" Certifiable used with no arguments, or with '?' displays this help")
        ConsoleWriteLineInColour(" ")
        ConsoleWriteLineInColour("Final notes on usage:")
        ConsoleWriteLineInColour(" the -d option does not generate code, rather simply displays available information")
        ConsoleWriteLineInColour(" code will only be generated if the -c or -w options are used")
        ConsoleWriteLineInColour(" ")
        ConsoleWriteLineInColour("Some common examples:")
        ConsoleWriteLineInColour(" certifiable ?")
        ConsoleWriteLineInColour(" certifiable www.google.com")
        ConsoleWriteLineInColour(" certifiable 142.251.41.14 -d")
        ConsoleWriteLineInColour(" certifiable github.com -d")
        ConsoleWriteLineInColour(" certifiable github.com -c")
        ConsoleWriteLineInColour(" certifiable github.com -n 2 -g c++ progmem -c")
        ConsoleWriteLineInColour(" certifiable github.com -n 2 -g c++ -v github_cert -c")
        ConsoleWriteLineInColour(" certifiable github.com -n 2 -g python -c")
        ConsoleWriteLineInColour(" certifiable github.com -n 2 -g vb.net -w certificate.h")
        ConsoleWriteLineInColour(" certifiable github.com -w certificate.h -o")
        ConsoleWriteLineInColour(" ")
        ConsoleWriteLineInColour("Certifiable v" & versionNumber, ConsoleColor.Yellow)
        ConsoleWriteLineInColour("Copyright © 2025, Rob Latour", ConsoleColor.Yellow, True)
        ConsoleWriteLineInColour(" ")
        ConsoleWriteLineInColour("Certifiable is open source", ConsoleColor.Cyan)
        ConsoleWriteLineInColour("https://github.com/roblatour/certifiable", ConsoleColor.Cyan)
        ConsoleWriteLineInColour(" ")
        ConsoleWriteLineInColour("Certifiable is licensed under the MIT License", ConsoleColor.Cyan)
        ConsoleWriteLineInColour("https://github.com/roblatour/setvol/blob/main/LICENSE", ConsoleColor.Cyan)
        ConsoleWriteLineInColour(" ")
        ConsoleWriteLineInColour("Certifiable makes use of TextCopy which is licensed under the MIT License", ConsoleColor.Cyan)
        ConsoleWriteLineInColour("https://github.com/CopyText/TextCopy", ConsoleColor.Cyan)
        ConsoleWriteLineInColour(" ")

        Console.ForegroundColor = startingColour

    End Sub

    Sub Main(args As String())

        gStartingColour = Console.ForegroundColor

        If ValidateArguments(args) Then

            If -iHelp Then

                DisplayHelp()

            ElseIf GetAllCertifations() Then

                If (ValidateCertNum()) Then

                    Dim oKToGenerate As Boolean = (iCopyToClipboard OrElse iWriteToFile)

                    If iDisplay Then DisplayCertificates()

                    If iFuture Then

                        If (gCurrentCertificates = 0) AndAlso (gFutureCertificates = 0) Then

                            ConsoleWriteLineInColour("No current or future certificates were found for " & iHost & "on port " & iPort)
                            oKToGenerate = False

                        End If

                    Else

                        If (gCurrentCertificates = 0) Then

                            ConsoleWriteLineInColour("No current certificates were found for " & iHost & "on port " & iPort)
                            oKToGenerate = False

                        End If

                    End If

                    If iFuture Then
                    Else
                        If gFutureCertificates = 1 Then
                            ConsoleWriteLineInColour(" ")
                            ConsoleWriteLineInColour("Note: there is one future dated certificate, to see it use the -future option")
                        ElseIf gFutureCertificates > 1 Then
                            ConsoleWriteLineInColour(" ")
                            ConsoleWriteLineInColour("Note: there are " & gFutureCertificates & " future dated certificates, to see them use the -future option")
                        End If
                    End If

                    If oKToGenerate Then
                        Generate()
                    End If

                End If

            End If

        End If

        Console.ForegroundColor = gStartingColour

        Environment.Exit(0)

    End Sub

End Module

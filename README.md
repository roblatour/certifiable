# Certifiable (v1.0 January 2, 2025)

Certifiable is a free open source command line program for Windows which generates code to assign a SSL Certificate PEM to a variable

The following languages are supported for code generation: C++, Python, VB.net

## Download 

You are welcome to download and use Certifiable on as many computers as you would like.

A downloadable signed executable portable copy of the program for Windows (x64) is available from [here](https://github.com/roblatour/certifiable/releases/download/v1.0.0.0/certifiable.exe).

## License
Certifiable is licensed under a [MIT license](https://github.com/roblatour/certifiable/blob/main/LICENSE)

## Setup

Certifiable does not need to be installed, rather download file (above) and run it.


## Example output

From the command:

```cpp
certifiable github.com -c
```
the following response is returned 
```cpp
C++ certificate code for github.com:443 copied to the clipboard
```
and here is what was copied to the clipboard
```cpp
// The following certificate is for use with github.com:443
// It is valid between 2024-03-06 7:00:00 PM (UTC) and 2024-03-06 7:00:00 PM (UTC) inclusive.
const char* GITHUB_COM_CERTIFICATE = "-----BEGIN CERTIFICATE-----\n" \
                                     "MIIEozCCBEmgAwIBAgIQTij3hrZsGjuULNLEDrdCpTAKBggqhkjOPQQDAjCBjzEL\n" \
                                     "MAkGA1UEBhMCR0IxGzAZBgNVBAgTEkdyZWF0ZXIgTWFuY2hlc3RlcjEQMA4GA1UE\n" \
                                     "BxMHU2FsZm9yZDEYMBYGA1UEChMPU2VjdGlnbyBMaW1pdGVkMTcwNQYDVQQDEy5T\n" \
                                     "ZWN0aWdvIEVDQyBEb21haW4gVmFsaWRhdGlvbiBTZWN1cmUgU2VydmVyIENBMB4X\n" \
                                     "DTI0MDMwNzAwMDAwMFoXDTI1MDMwNzIzNTk1OVowFTETMBEGA1UEAxMKZ2l0aHVi\n" \
                                     "LmNvbTBZMBMGByqGSM49AgEGCCqGSM49AwEHA0IABARO/Ho9XdkY1qh9mAgjOUkW\n" \
                                     "mXTb05jgRulKciMVBuKB3ZHexvCdyoiCRHEMBfFXoZhWkQVMogNLo/lW215X3pGj\n" \
                                     "ggL+MIIC+jAfBgNVHSMEGDAWgBT2hQo7EYbhBH0Oqgss0u7MZHt7rjAdBgNVHQ4E\n" \
                                     "FgQUO2g/NDr1RzTK76ZOPZq9Xm56zJ8wDgYDVR0PAQH/BAQDAgeAMAwGA1UdEwEB\n" \
                                     "/wQCMAAwHQYDVR0lBBYwFAYIKwYBBQUHAwEGCCsGAQUFBwMCMEkGA1UdIARCMEAw\n" \
                                     "NAYLKwYBBAGyMQECAgcwJTAjBggrBgEFBQcCARYXaHR0cHM6Ly9zZWN0aWdvLmNv\n" \
                                     "bS9DUFMwCAYGZ4EMAQIBMIGEBggrBgEFBQcBAQR4MHYwTwYIKwYBBQUHMAKGQ2h0\n" \
                                     "dHA6Ly9jcnQuc2VjdGlnby5jb20vU2VjdGlnb0VDQ0RvbWFpblZhbGlkYXRpb25T\n" \
                                     "ZWN1cmVTZXJ2ZXJDQS5jcnQwIwYIKwYBBQUHMAGGF2h0dHA6Ly9vY3NwLnNlY3Rp\n" \
                                     "Z28uY29tMIIBgAYKKwYBBAHWeQIEAgSCAXAEggFsAWoAdwDPEVbu1S58r/OHW9lp\n" \
                                     "LpvpGnFnSrAX7KwB0lt3zsw7CAAAAY4WOvAZAAAEAwBIMEYCIQD7oNz/2oO8VGaW\n" \
                                     "WrqrsBQBzQH0hRhMLm11oeMpg1fNawIhAKWc0q7Z+mxDVYV/6ov7f/i0H/aAcHSC\n" \
                                     "Ii/QJcECraOpAHYAouMK5EXvva2bfjjtR2d3U9eCW4SU1yteGyzEuVCkR+cAAAGO\n" \
                                     "Fjrv+AAABAMARzBFAiEAyupEIVAMk0c8BVVpF0QbisfoEwy5xJQKQOe8EvMU4W8C\n" \
                                     "IGAIIuzjxBFlHpkqcsa7UZy24y/B6xZnktUw/Ne5q5hCAHcATnWjJ1yaEMM4W2zU\n" \
                                     "3z9S6x3w4I4bjWnAsfpksWKaOd8AAAGOFjrv9wAABAMASDBGAiEA+8OvQzpgRf31\n" \
                                     "uLBsCE8ktCUfvsiRT7zWSqeXliA09TUCIQDcB7Xn97aEDMBKXIbdm5KZ9GjvRyoF\n" \
                                     "9skD5/4GneoMWzAlBgNVHREEHjAcggpnaXRodWIuY29tgg53d3cuZ2l0aHViLmNv\n" \
                                     "bTAKBggqhkjOPQQDAgNIADBFAiEAru2McPr0eNwcWNuDEY0a/rGzXRfRrm+6XfZe\n" \
                                     "SzhYZewCIBq4TUEBCgapv7xvAtRKdVdi/b4m36Uyej1ggyJsiesA\n" \
                                     "-----END CERTIFICATE-----";
```


## Using Certifiable

From the command prompt just type "certifiable ?" (without the quotes) to see the help as mostly reproduced below:.

Certifiable v1.0 Help<br>
<br>
Given a host name or IP address, and some additional information as outlined below,<br>
Certifiable will generate the code for assigning a variable a SSL certificate PEM
<br><br>
Usage:<br>
certifiable [ host (-p n) (-d) (-n n) (-g x) (-v x) (-c) (-w) (-o) (-f) ] | [ ] | [ ? ]<br>
<br>

*how do i insert a tab between where it says 'host' and the 'the host name' below*

host&nbsp; &nbsp; &nbsp; &nbsp;the host name or IP address from which to get the certificate(s)<br>
 &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;for example: google.com, www.google.com, 142.251.41.14<br>
  &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;a host value is required in all cases other than displaying this help information<br>
<br>
Options:<br>
<br>
 -p n&nbsp; &nbsp; &nbsp; &nbsp;the host's port number<br>
&nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp;for example: -p 8096<br>
&nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp;if not used a port number of 443 will be assumed<br>
<br>
 -d &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; display the host's available certificates<br>
&nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; if neither -c or -o options are used, the -d option will be assumed<br>

 -n n&nbsp; &nbsp; &nbsp; &nbsp;in many cases more than one certificate may be available for a host<br>
&nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp;when the -d option is used each certificate will be displayed with a unique certification number<br>
&nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp;if not otherwise specified the first certificate will be used in code generation<br>
&nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp;to specify a certificate other than the first certificate<br>
&nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp;use -n n where the second n is the unique certificate number displayed with the -d option<br>
&nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp;for example, -n 3<br>
<br>
 -g x&nbsp; &nbsp; &nbsp; &nbsp; used to specifying the language in which the code will be generated<br>
&nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp;supported languages are: c++, python, and vb.net<br>
&nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp;for example: -g vb.net<br>
&nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp;if not used, the c++ will be assumed<br>
<br>
 -v x&nbsp; &nbsp; &nbsp; &nbsp; the variable name to be used in the output file<br>
 &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp;for example: -v server_root_cert<br>
 &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp;if not used, a variable name based on the host name / IP address will be generated<br>
<br>
 -c&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;   the generated code will be copied to the clipboard<br>
<br>
 -w x &nbsp; &nbsp; &nbsp; &nbsp;the generated code will be written to the specified (path and) file name<br>
&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;for example, -w c:\temp\certificate.h<br>
&nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp; the output path name is optional, if omitted the output file will be written to the current working directory<br>
&nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp; for example, -w certificate.h<br>
&nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp; if the path is specified but does not exist, it will be created<br>
<br>
 -o&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; if the -w option is used and the specified file already exists<br>
 &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp; the user will be prompted to overwrite the file unless the -o options is used<br>
 &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp;in which case the existing file will be automatically overwritten<br>
 &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp; for example, -w c:\temp\certificate.h -o<br>
<br>
 -f&nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; by default future dated certificates are ignored<br>
 &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp; if -f is used, then future dated certificates won't be ignored<br>
 &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp; note: expired certificates are always ignored<br>
<br>
 Certifiable used with no arguments, or with '?' displays this help<br>
<br>
Final notes on usage:<br>
 the -d option does not generate code, rather simply displays available information<br>
 code will only be generated if the -c or -o options are used<br>
<br>
Some common examples:<br>
 certifiable ?<br>
 certifiable www.google.com<br>
 certifiable 142.251.41.14 -d<br>
 certifiable github.com -d<br>
 certifiable github.com -n 2 -c<br>
 certifiable github.com -n 2 -g python -c<br>
 certifiable github.com -w certificate.h<br>
 certifiable github.com -w certificate.h -o<br>


## Components

Certifiable is built upon the .net framework 8

Certifiable makes use of TextCopy which is licensed under the MIT License
https://github.com/CopyText/TextCopy

* * *
 ## Support Certifiable

 To help support Certifiable, or to just say thanks, you're welcome to 'buy me a coffee'<br><br>
[<img alt="buy me  a coffee" width="200px" src="https://cdn.buymeacoffee.com/buttons/v2/default-blue.png" />](https://www.buymeacoffee.com/roblatour)
* * *
Copyright © 2025, Rob Latour
* * *

# Certifiable (v1.0 January 2, 2025)

Certifiable is a free open source command line program for Windows to generate code to assign a SSL Certificate PEM to a variable.

The following langages are supported for code generation: C++, Python, VB.net

## Download 

You are welcome to download and use Certifiable on as many computers as you would like.

A downloadable signed executable portable copy of the program for Windows (x64) is available from [here](https://github.com/roblatour/setvol/releases/download/v1..0.0/certifiable.exe).

## License
Certifiable is licensed under a [MIT license](https://github.com/roblatour/certifiable/blob/main/LICENSE)

## Setup

Certifiable does not need to be installed, rather download file (above) and run it.

## Using Certifiable

From the command prompt just type "certifiable ?" (without the quotes) to see the help (as detailed below).

Certifiable v1.0 Help

Given a host name or IP address, and some additional information as outlined below,
Certifiable will generate the code for assigning a variable a SSL certificate PEM

Usage:
certifiable [ host (-p n) (-d) (-n n) (-g x) (-v x) (-c) (-w) (-o) (-f) ] | [ ] | [ ? ]

host   the host name or IP address from which to get the certificate(s)
       for example: google.com, www.google.com, 142.251.41.14
       a host value is required in all cases other than displaying this help information

Options:

 -p n  the host's port number
       for example: -p 8096
       if not used a port number of 443 will be assumed

 -d    display the host's available certificates
       if neither -c or -o options are used, the -d option will be assumed

 -n n  in many cases more than one certificate may be available for a host
       when the -d option is used each certificate will be displayed with a unique certification number
       if not otherwise specified the first certificate will be used in code generation
       to specify a certificate other than the first certificate
       use -n n where the second n is the unique certificate number displayed with the -d option
       for example, -n 3

 -g x  used to specifying the language in which the code will be generated
       supported languages are: c++, python, and vb.net
       for example: -g vb.net
       if not used, the c++ will be assumed

 -v x  the variable name to be used in the output file
       for example: -v server_root_cert
       if not used, a variable name based on the host name / IP address will be generated

 -c    the generated code will be copied to the clipboard

 -w x  the generated code will be written to the specified (path and) file name
       for example, -w c:\temp\certificate.h
       the output path name is optional, if omitted the output file will be written to the current working directory
       for example, -w certificate.h
       if the path is specified but does not exist, it will be created

 -o    if the -w option is used and the specified file already exists
       the user will be prompted to overwrite the file unless the -o options is used
       in which case the existing file will be automatically overwritten
       for example, -w c:\temp\certificate.h -o

 -f    by default future dated certificates are ignored
       if -f is used, then future dated certificates won't be ignored
       note: expired certificates are always ignored

 Certifiable used with no arguments, or with '?' displays this help

Final notes on usage:
 the -d option does not generate code, rather simply displays available information
 code will only be generated if the -c or -o options are used

Some common examples:
 certifiable ?
 certifiable www.google.com
 certifiable 142.251.41.14 -d
 certifiable github.com -d
 certifiable github.com -n 2 -c
 certifiable github.com -n 2 -g python -c
 certifiable github.com -w certificate.h
 certifiable github.com -w certificate.h -o

Certifiable v1.0
Copyright © 2025, Rob Latour

## Example output

From the command:

certifiable github.com -c

<span style="color: green;">C++ certificate code for github.com:443 copied to the clipboard</span>

(and here is what was copied to the clipboard)

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

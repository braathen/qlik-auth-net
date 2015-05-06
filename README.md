## QlikAuthNet

QlikAuthNet is an ASP.NET module for simplifying custom authentication with Qlik Sense. With minimal coding effort it takes care of everything from ticket request to redirection.

```cs
using QlikAuthNet

var req = new Ticket()
{
  UserDirectory = "QLIK",
  UserId = "rfn"
};

req.TicketRequest();
```

It has some extra functionality to make adding of custom attributes and especially groups very simple. Here's some examples...

```cs
req.AddGroups("Developers;Enterprise Architects;PreSales");

req.AddAttributes("Email", "some@email.com");
req.AddAttributes("Country", "Sweden");
```

All of the above can be a semicolon separated list that will be split automatically. One or more custom delimiters can optionally be specified as second argument if semicolon is not appropriate.

## Installation

Easiest way to install is by using the NuGet Package Management Console inside of Visual Studio.

```sh
PM> Install-Package QlikAuthNet
```

Create a virtual proxy in Qlik Sense QMC and refer the Authentication Module to the URL of the website.

Note: The module will check for the presence of QlikClient certificate in the local certificate store. When deploying this to IIS the ApplicationPool must have access to this certificate.

## Authentication Module Demo

This solution includes a demo project to make it simple to demonstrate the concept of tickets with Qlik Sense. It should be fairly straight forward to get up and running.

![ScreenShot](https://raw.github.com/braathen/qlik-auth-net/master/Images/screenshot.png)

## F.A.Q

coming soon!

## License

This software is made available "AS IS" without warranty of any kind under The Mit License (MIT). Qlik support agreement does not cover support for this software.

## Meta

* Code: `git clone git://github.com/braathen/qlik-auth-net.git`
* Home: <https://github.com/braathen/qlik-auth-net>
* Bugs: <https://github.com/braathen/qlik-auth-net/issues>

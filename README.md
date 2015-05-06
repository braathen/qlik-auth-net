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

## Installation

Easiest way to install is by using the NuGet Package Management Console inside of Visual Studio.

```sh
PM> Install-Package QlikAuthNet
```

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

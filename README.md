## About

QlikAuthNet is an ASP.NET module for simplifying custom authentication with Qlik Sense. With minimal coding effort it takes care of everything from ticket request to redirection. Everything is more or less automated and the only thing required is to supply user credentials and attributes.

## Installation

Easiest way to install is by using the NuGet Package Management Console inside of Visual Studio.

```sh
PM> Install-Package QlikAuthNet
```

Create a virtual proxy in Qlik Sense QMC and refer the Authentication Module to the URL of the website. Please see the Qlik Sense help regarding how to set up a virtual proxy.

>Note: The module will check for the presence of QlikClient certificate in the local certificate store. When deploying to IIS the ApplicationPool must have access to this certificate.

## Examples

Minimal usage may look like this:

```cs
using QlikAuthNet

var req = new Ticket()
{
  UserDirectory = "QLIK",
  UserId = "rfn"
};

req.TicketRequest();
```

There is some extra functionality to make adding of custom attributes and especially groups very simple. Here's some examples...

```cs
req.AddGroups("Developers;Enterprise Architects;PreSales");

req.AddAttributes("Email", "some@email.com");
req.AddAttributes("Country", "Sweden");
```

All of the above can be a semicolon separated list that will be split automatically. One or more custom delimiters can optionally be specified as third argument if semicolon is not appropriate. It's also possible to use a `List<string>` directly.

## Authentication Module Demo

This solution includes a demo project to make it simple to demonstrate the concept of tickets with Qlik Sense. It should be fairly straight forward to get up and running.

![ScreenShot](https://raw.github.com/braathen/qlik-auth-net/master/Images/screenshot.png)

## F.A.Q

Explanations to common problems that might occur...

#### "proxyRestUri not defined!"
The module is designed to be used in a flow where the virtual proxy redirects to the module with the proxyRestUri and targetId parameters in the URL. If these can not be obtained automatically they need to be supplied manually.

#### "Please don't access this Authentication Module directly. Use a virtual proxy instead!"
Again, this is the same problem as above. Always access the module through the virtual proxy and the flow will be correct and according to best practise.

#### "Certificate not found! Verify AppPool credentials."
The module looks for QlikClient certificate in the local certificate store. If deployed to IIS the ApplicationPool must have access to the certificate.

#### "Unknown error"
You tell me!

## License

This software is made available "AS IS" without warranty of any kind under The Mit License (MIT). Qlik support agreement does not cover support for this software.

## Meta

* Code: `git clone git://github.com/braathen/qlik-auth-net.git`
* Home: <https://github.com/braathen/qlik-auth-net>
* Bugs: <https://github.com/braathen/qlik-auth-net/issues>

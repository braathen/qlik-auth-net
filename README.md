>Diclaimer: I no longer work for Qlik and I have no possibility to test, support or maintain this code any more. Most of my Qlik related repositories are a few years old and was built in an early stage of product releases as examples how to get started with the Qlik API's. Please take it for what it is and use as you like.

## About

QlikAuthNet is an ASP.NET module for simplifying custom authentication with Qlik Sense. With minimal coding effort it takes care of everything from ticket request to redirection. Everything is dynamic and more or less automated, the only thing required is to supply user credentials and attributes. No hardcoding paths are necessary and the same code can serve multiple proxys/deployments without changes.

## Installation

Easiest way to install is by using the NuGet Package Management Console inside of Visual Studio.

```sh
PM> Install-Package QlikAuthNet
```

Create a virtual proxy in Qlik Sense QMC and refer the Authentication Module to the URL of the website. Please see the Qlik Sense help regarding how to set up a virtual proxy.

>Note: The module will check for the presence of QlikClient certificate in the local certificate store. When deploying to IIS the ApplicationPool must have access to this certificate. If deployed on a separate server it's necessary to export certificates from QMC and install the client.pfx (QlikClient) certificate on the new server.

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

>Note: When using custom attributes together with security rules it's important to use `user.environment.<attribute>` as the ticket information belongs to the session, not the user record. For instance you might want to create security rules containing both `user.environment.group` and `user.group` to verify a users group membership if there are users accessing Qlik Sense both through tickets and Windows Authentication (different proxys).

## Authentication Module Demo

This solution includes a demo project to make it simple to demonstrate the concept of tickets with Qlik Sense. It should be fairly straight forward to get up and running.

![ScreenShot](https://raw.github.com/braathen/qlik-auth-net/master/Images/screenshot.png)

## F.A.Q

Explanations and answers to common problems and questions that might occur...

#### "proxyRestUri not defined!"
The module is designed to be used in a flow where the virtual proxy redirects to the module with the proxyRestUri and targetId parameters in the URL. If these can not be obtained automatically they need to be supplied manually.

#### "Please don't access this Authentication Module directly. Use a virtual proxy instead!"
Again, this is the same problem as above. Always access the module through the virtual proxy and the flow will be correct and according to best practise.

#### "Certificate not found! Verify AppPool credentials."
The module looks for QlikClient certificate in the local certificate store. If deployed to IIS the ApplicationPool must have access to the certificate. As of v1.1.2 it is possible to set the CertificateLocation (default=CurrentUser) and CertificateName (default=QlikClient).

#### "The remote server returned an error: (400) Bad Request."
This can be many things, but one thing to make sure is the upper/lowercase of the virtual proxy prefix. As of this writing it's still case sensitive. Always use lowercase only.

#### "Unknown error"
You tell me!

#### It says nothing at all... Nothing seems to happen?
The module is designed to do all the work and redirect the user to it's final destination, however if something goes wrong, an error message is returned as a string to be handled in some way, see the AuthenticationModuleDemo example.

#### Is it possible to override the automatic redirection?
Yes, by not including a TargetId in the request a string is returned (from v1.0.1) that looks something like `qlikTicket=G1xQJQOPWeG.3Wl4` which then may be added to the redirection URL manually. Be aware though that when being redirected from the proxy the module tries to request this parameter. Also, it's highly recommended to keep the flow from the proxy.

#### Is it possible to authenticate the ticket request without certificates?
No. Certificates is the only way to authenticate ticket requests. In QlikView it was possible to do Windows Authentication, but in Qlik Sense this is not possible.

#### My custom attributes does not seem to work?
See the note under Examples section. This works as designed.

## License

This software is made available "AS IS" without warranty of any kind under The Mit License (MIT). Qlik support agreement does not cover support for this software.

## Meta

* Code: `git clone git://github.com/braathen/qlik-auth-net.git`
* Home: <https://github.com/braathen/qlik-auth-net>
* Bugs: <https://github.com/braathen/qlik-auth-net/issues>

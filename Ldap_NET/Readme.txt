Steps to build the project:
1.Open the TechTrial.sln Solution in Visual Studio 2012 (Administrator Mode).
2. Build the solution.

Note for Running the project:
1. Update the Data Source of connection string in ~/Ldap_NET\Source\IdentityManagerRestApis\Web.config.
   The connection string name must be "IdentityManager"
	  Example  <add name="IdentityManager" providerName="System.Data.SqlClient" 				            connectionString="server=localhost;database=SSO;Trusted_Connection=Yes;"/>

2.  Update the value in Ldap_NET\Source\IdentityManagerRestApis\Web.config AppSetting Setions
	a. Update value of Ldap Serverin key  "Ldap.Connection".Just give the server address without protocol.
	b. update timeout value  of  "TicketExpirationTimeoutInMinutes".This is used to expire authentication token sfter specified time.

3. Use the same machine key in all the three web application web.config.
	a.SSOApp
	b.TestApp1
	c.TestApp2

Database will be created by EntityFramework.provided the connections string is valid.

Assumptions:
1. The LDAP server url is correct.
2. Using cookie for authentication.Hence cookies needs to be enbaled in browser.


Steps to install the api and web application on IIS using the deploy packages:
1. Open Inetmgr.
2. Under default website right click and add following applications.
3. Enter Alias as "Idm" and point it to "~/Ldap_NET\Source\IdentityManagerRestApis".
4. Enter Alias as "SSo" and point it to "~/Ldap_NET\Source\SSOApp".
5. Enter Alias as "App1" and point it to "~/Ldap_NET\Source\TestApp1".
6. Enter Alias as "App2" and point it to "~/Ldap_NET\Source\TestApp2".
7. Make sure that in  Application Pool .Net Framework is set to v4.0. and Managed Pipeline mode is set to Integrated.
8. Browse the site with with url "localhost/SSO" in browser.(Ensure to deploy update connection strings in web.config as decsrbed above.)

Note: if above names and structures structure is not followed need to update below setting in SSOApp config file.
	a. "SignOutUrl" in appsettings. (For TestApp2 and TestApp2)
	b  "Loginurl" in  authentication. (For SSOApp,TestApp2 and TestApp2)
	c. "AppOneUrl" in appsettings. (For SSOApp)
	d. "AppTwoUrl in appsettings. (For SSOApp)
	e. "AuthenticationApi" in appsettings (For SSoApp).

# App Configuration

Before running the app, make sure to configure the following settings:

- **AllowedHosts**: Set the value to `*`.

- **ConnectionStrings**: Update the `DefaultConnection` with your SQL server information. The format should be as follows:
- **DefaultConnection** :  `"server= ;database=  ;uid= ;pwd= ;TrustServerCertificate=True"`. Whith `server` is your  ServerSQL Name, `database`  is Your name Database,  `uid` you account SQL Server, `pwd` you password SQL Server .
- **Run Project** :  After configuring the server, `Package Manager Console` in  `NuGet Package Manager` (if you use Visul Studio). and run this Commant `Update-database` . Ok Now you can run server. ^^
# Nexus Email

Use the following credentials for Nexus Email:

- **Email**: nexus.corporate.group3@gmail.com
- **Password**: Nexus@123

# Importing Database

Please refer to the provided video for instructions on how to import the database.

# Test Payment Accounts

Use the following test accounts for payment testing:

- **Customer Accounts**:
- customerN@personal.example.com
- customerM@personal.example.com
- **Shared Password**: Matkhau1

- **Store Account**:
- Email: nexus.corporate.group3@gmail.com
- Password: Matkhau1

To view the payment results, visit the following link: [PayPal Sandbox Activity](https://www.sandbox.paypal.com/activities/)
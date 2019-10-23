![enter image description here](https://api.nuget.org/v3-flatcontainer/lightado.net/5.2.7/icon)
# LightAdo!

Let's face it Open, close, execute and handle errors for ADO is drag. That's why we wrote **LightAdo dotNet**

```C#
Employ employ = new Query().ExecuteToObject(
                "Employes_GetByID",
                System.Data.CommandType.StoredProcedure,
                new Parameter("ID", id));
```

LightAdo provides a straight-forward, ORM solution to handle ADO for your application data. 
It includes built-in casting, validation,  encryption, bulks , logs and more out of the box.

# Get Started
First Let's install LightAdo.net 

```sh
$ dotnet add package LightAdo.net 
```

Now say we like to get a list of all employess in from HR database, setting up the connection string will be 
the start point, open the appsettings.json and modify the ConnectionString 
section to add a new connection with name DefaultConnection: 

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "...."
  }
}
```

Assuming we have the following class: 

```C#
public class Employ {
	public int ID { get; set; }
	public string Name { get; set; }
	public string Email { get; set; }
	public DateTime CreatedDate { get; set; }
}
```

Which is a representation for the table employes in the database:

|Name| Type  |
|--|--|
| ID  | int |
| Name  | nvarchar(1500) |
| Email  | nvarchar(1500) |
| CreateDate  | DateTime |

Now let's get all employes: 

```C#
List<Employ> employes = new Query().ExecuteToListOfObject<Employ>("Select * from Employes", System.Data.CommandType.Text);
```

Now let's insert a new employ, assumming we have the following stored procedure: 

```SQL
@Name nvarchar(1500),
@Email nvarchar(1500),
@CreateDate datetime
GO 
insert into Employes (Name, Email, CreateDate) values (@Name, @Email, @CreateDate)
SET @ID = scope_identity();
```

Using **lighado** we can use the NonQuery Class to map and get the new id of the employe with one of code as following: 

```C#
new NonQuery().Execute("Create_Employes", this);
```

That's very much it, Done! things should be that simple and clean.

# Features

- Map Object vice versa between CLI and SQL Server.
- Validate the object before push it to SQL Server. 
- Custome Validation ber property.
- Setting default values for each property. 
- Relationship mapping between objects using Primary and forign kyes.
- SQL Server Transcation supported. 
- Change type of property on runtime before send it to SQL Server. 
- Use C# naming conventions to replace the naming conventions for SQL server. 
- Convert object vice verrsa between Json and XML and CLI.
- Go old schoole and get straight-forward Data set or datatable.
- Send Bulks of objects for insert, update and delete with support for workflow and Relationship.
- Plug your logic to **lightado** events gives you the power to control and see the transactions all the way from mapping, validations execution, closing tell the object get back to you.
- Enable logging using the power of log4net.
- Encrypt each property and decrypt on run time, using your encryption methods.
- Multi Mapping convert result to multiple types of objects.

# Chanage Logs

#### 5.2.7 
- upgrading to core 3.0. 
- Adding the applity to set default values for property.

 #### 5.1.2
 - Adding the transcation Ability so now you can do muilt sql command in one line of code

 #### 5.1.1
 - Fixing the Ecrypted Key Length

 #### 5.0.0
 - This is majer update to support .net core.

 #### 4.5.1
 - Fixing the issue of converting type to Nullable.

 #### 4.5.0
 - Adding the ability to remove control characters from a string.

 #### 4.4.0
 - Adding dynimc type changer in the output.

 #### 4.3.1
 - Fixing the bigger then and less then method.

 #### 4.3.0
 - Adding IsNotContain.

 #### 4.0.0
 - Features:
   - Addint the following validation options: 
     * MaxLenght. 
     * MinLenght. 
     * IsNotBiggerThen. 
     * IsNotLessThen. 
     * IsNotContains
     * IsContains
   - Majer Change: 
     * move all validation class into valudation namespace: 
     * rename the following class: 
       NullValidation => IsNotNulll RegularExpressionsValidation => IsValidRegularExpressions

 #### 3.3.0
 - Features:
   - Make the Data Mapper public class.
   - Adding the ability to call  selected method with auto validation class

 #### 3.2.0 
 - Features:
   - Adding the support to get JSON and XML as result  directly from the query resutl .

   -  Adding the support to get List of JSON and XML as result  directly from the query resutl .

 #### 3.1.0 
 - Features:
   - Adding Support to Map Enum Property .

 #### 3.0.0 
   - Features:
     - Your own Encryption Class
     Give the developer the ability to inherit the 
 EncryptEngine attribute so he can create his own encrypt and decrypt methods. 

 - On Error Event
 Developer now can hendle any error thow by the SQL server using OnError Event so 
 now you don't need to write try catch just one please to handle all of exception thow by SQL Server. 

 - Major Change: 
   - Normal Exception: 
 I aslo remove the lightADOExpection and replaces it with normal
  Exception, so now you just try {} catch ( Exception ex) {} 

 - Bug Fixes: 
   No Bug found yet.

 #### 1.1.4 
 - Features:
   - Adding: 4 Event's : 
     - After and before opern connection. 
     - after and before close connection.
     - after and before Execute Query or NonQuery

   - Adding the ability  to trow cusmte Exception when Query return null value from the database

 - Bug Fixes: 
   - Fixing the the issue with data row when  propert do not belong to a table.

 #### 1.1.4 
 - Features:
   - Adding: 4 Event's : 
     - After and before opern connection. 
     - after and before close connection.
     - after and before Execute Query or NonQuery

   - Adding the ability  to trow cusmte Exception when Query return null value from the database

 - Bug Fixes: 
   - Fixing the the issue with data row when  propert do not belong to a table.

 #### 1.0.11 
 - Fixing the dynamic List issue.

 #### 1.0.10
 - Fixing the DbNull Issue with Data Mapping

 #### 1.0.9
 - Add the ability   to set custom column name for propertiesby setting the ColumnName Attribute . 
 - Add the ability to ignore data mapping for property by setting the IgnoreDataMapping attribute in the top of the property

 #### 1.0.8
 - Add Encryption as Attributefor the calss,  lightado.net will encrypt and decrypt that class automatically 
 - Adding the ability  to map this as object ..

 #### 1.0.7
 - Adding the ability to Encrypt or decrypt a Wholly class or single Property in it, by mark the class or the property as [Encrypted]

 #### 1.0.6
 - Fxing the Loading Connection string with auto mapping in NonQuery. 
 - Add Encryption as Attribute so you can mark property as encrypted lightado.net will encrypt and decrypt this property automatically.

# Support
 1. [LightADO](https://lightado.net)
 2. [Gitter](https://gitter.im/lightado/community#)
 3. [Issues.](https://github.com/aalghabban/lightado/issues)
 4. [Examples](https://examples.lightado.net)
 5. [Stackoverflow](https://stackoverflow.com/questions/tagged/lightado)
 6. Videos

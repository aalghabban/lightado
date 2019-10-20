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
It includes built-in casting, validation,  encryption, bulks , logs and more, out of the box.

# Get Started
First Let's install LightAdo.net 

```sh
$ dotnet add package LightAdo.net 
```

Now say we like list all employess in our HR database, setting up the connection string will be 
the start point, open the appsettings.json, app.config or web.config and modify the ConnectionString 
section to add a new connection with name DefaultConnection: 

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <connectionStrings>
      <add name="DefaultConnection" connectionString="...."/>  
  </connectionStrings>
</configuration>
```

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

Now let's get all employes: 

```C#
List<Employ> employes = new Query().ExecuteToListOfObject<Employ>("Select * from Employes", System.Data.CommandType.Text);
```

That's it very much!

# Support
 1. [LightADO](https://lightado.net)
 2. [Gitter](https://gitter.im/lightado/community#)
 3. [Issues.](https://github.com/aalghabban/lightado/issues)
 4. [Examples](https://examples.lightado.net)
 5. [Stackoverflow](https://stackoverflow.com/questions/tagged/lightado)
 6. Videos

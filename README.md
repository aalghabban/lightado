# LightADO

LightADO is a ORM Interested in serving the integration developers with functionalities like:

  - Parsing json or xml object to C# object.
  - connting to multiple sql server databases. 
  - Logges and encryption built in. 

It's Data Access Layer for SQL Server it will handle all of the legacy that come when dealing with sql server database like " open connections, create sqlcommand, loop throw DataReader, Get a Data Table , convert Data table to Object or data set, close connection, get output and parameters values.. etc , with lightAdo.net you can Execute Query and Non Query, get direct object or even dynamic object, auto mapping stored procedures to object properties set foreign  key to get sub object details, Validation nulls without writing single if statement and more with just simple one line of code.

# Change Log 
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
- Add Encryption as Attribute so you can mark property as encrypted lightado.net will encrypt and decrypt this property automatically




## Installation

```sh
$ dotnet add package LightAdo.net --version 5.1.2
```

Or you can use Visual studio search for lightado.net.
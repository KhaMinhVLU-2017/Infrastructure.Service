# Infrastructure Service

**Appendium**

- Overview
- Installation
- Basic Usage

## Overview

- Open end-point API allow build dynamic query for searching and paging. It **auto compile query from text** on Front-end to **Expression Tree** Linq.
- Integration with [Infrastructure.Repository](https://www.nuget.org/packages/Infrastructure.Repository!)
- Support .NetCore if you want run on .NetFramework you need override the **Repository Layer**

## Installation

To install Infrastructure Service [Infrastructure.Service](https://www.nuget.org/packages/Infrastructure.Service!) library, right-click on your project in Solution Explore,then select **Manage NuGet Packages...**, and install the following package.

- Infrastructure.Service

You can also install this library using **.NET CLI**

```cs
dotnet add package Infrastructure.Service --version 4.0.0
```

Setup code base, It similar version 3.x.x

- [Guide for setup code](<!https://www.mnlifeblog.com/posts/Dynamic-Search-(continue)-post23.html>)

## Basic Usage

- End-point API will be public with append some query params: **filters, sorts, pageSize, pageIndex.** You can view more _BaseCriteria_'s class
- You should take care some class:
  - **BaseCriteria**: the query params for api
  - **PagedList**: the response model
  - **ISearchService**: the service handler logic.

### Simple Query

#### Single query

When querying data using Linq, everything can be express look like below

```cs
var result = context.Users.Where(s => s.Website == "www.mnlifeblog.com");
```

Infrastructure.Service look like:

- JSON: {"Key":"website","Operate":"eq","Value":"www.mnlifeblog.com"}
- Example API: {{host}}/users?filters={"Key":"website","Operate":"eq","Value":"www.mnlifeblog.com"}

#### Multiple query

Note that it's also possible to query data using multiple predicates:

```cs
var result = context.Users.Where(s => s.Website == "www.mnlifeblog.com" ||
                                      s.Name == "mnlifeblog");

```

That code can be used when using Infrastructure.Service

- JSON: {"or":[{"Key":"website","Operate":"eq","Value":"www.mnlifeblog.com"},{"Key":"name","Operate":"eq","Value":"mnlifeblog"}]}
- Example API: {{host}}/users?filters={"or":[{"Key":"website","Operate":"eq","Value":"www.mnlifeblog.com"},{"Key":"name","Operate":"eq","Value":"mnlifeblog"}]}

**Operand summary table**

| Operate | Translate | Description         |
| ------- | --------- | ------------------- |
| eq      | ==        | Equals              |
| neq     | !=        | Not Equals          |
| gt      | >         | Greater Than        |
| gte     | >=        | Greater Than Equals |
| lt      | <         | Less Than           |
| lte     | <=        | Less Than Equals    |
| in      | Contains  | Contains            |
| nin     | !Contains | Not Contains        |
| btw     | >= <=     | Between             |

### Sorts

#### Sort by ASC or DESC

When use **orderby** on Linq:

```cs
 var result = context.Users.Orderby(s => s.Website);
```

The code can be use on Infrastructure.Service:

- JSON: {"key":"website","criteria":"asc"}
- Example API: {{host}}/users?sorts={"key":"website","criteria":"asc"}

When you want to apply **DESC** for sort you just alter _asc_ to _desc_.

#### Multiple sort

The strongly typed LINQ:

```cs
 var result = context.Users.Orderby(s => s.Website).ThenByDescending(s => s.Date);
```

The code you can build:

- JSON: [{"key":"website","criteria":"asc"},{"key":"date","criteria":"desc"}]
- Example API: {{host}}/users?sorts=[{"key":"website","criteria":"asc"},{"key":"date","criteria":"desc"}]

### Paging

Depend on pageIndex and pageSize field for paging

- Example API: {{host}}/users?pageIndex=1&&pageSize=20

The payload response:

```json
{
  "paged": {
    "totalCount": 42,
    "count": 10,
    "pageIndex": 1,
    "pageSize": 10,
    "durationMilliseconds": 10,
    "queryMilliseconds": 8,
    "totalMiliseconds": 10
  }
}
```

### Validation

You can restrict some fields and not allow search by **SearchRestrictions**'s section on _appsetting.json_

```json
  "SearchRestrictions": {
    "User": "name, id, date ",
  }
```

Take note: the fields separated by **a comma** and it **not case sensitive**

The **user's property** on JSON will be matched to the **name** of the entity.

```cs
 public class User : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        ...
    }
```

Throw exception message: "<font color=red>The field is restricted!</font>" when you access to field restricted.

Source: https://github.com/KhaMinhVLU-2017/Infrastructure.Service

**<font color=#0fb503>_Thanks for watching_</font>**

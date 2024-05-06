# csprojLink

When you have to migrate an existing .NET application from .NET Framework to .NET Core, this tool is helping with setting up the .NET Core project and saving a lot of time for manual labour doing so.

You can see the tool in action in this blog post:
[How to migrate an AutoCAD based application to .NET Core 8.0 [Part II]](https://thebuildingcoder.typepad.com/blog/2024/04/migrating-from-net-48-to-net-core-8.html)

Scroll down to about 60% of the post to find the detailed description of how this works.

## Parameters

The application needs two parameters to work. 

The first parameter is the full path to the location of the *.csproj file. 

The second parameter is the link we need to apply.

This is an example parameter set:

    D:\DEV_AuStMgd\AuSt10Mgd\AuSt10Mgd
    …\AuSt10Mgd_net4

**Both parameters should not be trailed by a backslash!**

## Usage 

Call with Command Prompt

```
csprojLink "D:\DEV\_AuStMgd\AuSt10Mgd\AuSt10Mgd" "..\AuSt10Mgd_net4"
```

Call with PowerShell

```
.\csprojLink "D:\DEV\_AuStMgd\AuSt10Mgd\AuSt10Mgd" "..\AuSt10Mgd_net4"
```

If there are no spaces in the parameters, you can omit the quotation marks.

Call with Command Prompt
```
csprojLink D:\DEV\_AuStMgd\AuSt10Mgd\AuSt10Mgd ..\AuSt10Mgd_net4
```

Call with PowerShell
```
.\csprojLink D:\DEV\_AuStMgd\AuSt10Mgd\AuSt10Mgd ..\AuSt10Mgd_net4
```

The *.csproj includes will now be linked.

## Reference 

Again, please refer to the blog post 
[How to migrate an AutoCAD based application to .NET Core 8.0 [Part II]](https://thebuildingcoder.typepad.com/blog/2024/04/migrating-from-net-48-to-net-core-8.html)


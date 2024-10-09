# Unofficial API client for managing Outline server
#### .net API wrapper for Outline VPN https://getoutline.org/


## Install
Build the project yourself and connect it to the project as a .dll or install the nuget package ```OutlineClient```



## Configuring
```csharp
using TDV.OutlineClient;

var _client = new OutlineClient(apiUrl, certSha256)
```
where
 ```apiUrl``` - server url, like ```https://127.0.0.1:51083/xlUG4F5BBft4rSrIvDSWuw/```. Should be ended on the ```/``` char
```certSha256``` - certificate hash, like ```A7B693EF6582E719F578764D7CE407930DF177B4433ECDD22402FB12162CA7BF```

## Server
### Returns information about the server
```csharp
var info = await _client.Server.Info();
```

### Renames the server
```csharp
await _client.Server.Rename("New server name");
```

### Set port for new access keys
```csharp
int newPort = 12332;
await _client.Server.SetPortForNewAccessKeys(newPort);
```

### Set data limit for all access keys
```csharp
long bytesLimit = 500000;
await _client.Server.SetDataLimit(bytesLimit);
```

### Delete data limit for all access keys
```csharp
await _client.Server.DeleteDataLimit();
```

## Access keys
### List all
```csharp
List<AccessKey> keys = await _client.AccessKeys.List();
```

### Get key by id
```csharp
string keyId = "2";
AccessKey key = await _client.AccessKeys.Get(keyId);
```

### Create a key
```csharp
AccessKey key1 = await _client.AccessKeys.New();
// or
AccessKey key2 = await _client.AccessKeys.New(method, id, name, password, port, limit);

```
mehtod - ```aes-192-gcm``` or ```chacha20-ietf-poly1305```. ```aes-192-gcm``` by default

### Remove key
```csharp
await _client.AccessKeys.Delete("2");
```

### Rename key
```csharp
string keyId = "2";
await _client.AccessKeys.Rename(keyId, "New key name");
```

### Set key data limit
```csharp
string keyId = "2";
long bytesLimit = 500000;
await _client.AccessKeys.SetDataLimit(keyId, bytesLimit);
```

### Remove key data limit
```csharp
string keyId = "2";
await _client.AccessKeys.DeleteDataLimit(keyId);
```

## Metrics
### Receive  data usage by users for last 30 days
```csharp
List<MetricsTransfer> sut = await _client.Metrics.List();
```

## Tests
To run tests, set the ```apiUrl``` and ```certSha256``` enviromen variables for  ```OutlineClient.Test``` project
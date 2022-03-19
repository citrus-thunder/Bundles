# Bundles

Bundles is an abstract and flexible data modeling library that focuses on ease-of-use.

Bundles allows engineers to easily design application save data structure from the top down by describing the data's final layout rather than working from the runtime data structure outward.

## Installation

The easiest way to include Bundles in your project is via the [MRWilliams.Bundles Nuget package](https://www.nuget.org/packages/MRWilliams.Bundles). The Nuget page describes several methods of including the package in your project.

## Example

Bundles is configured using XML content referred to as a BundleDef definition. This can be provided to the Bundler constructor as a path to an XML resource, or as a populated `XmlDocument` reference.

```xml
<BundleDef>
	<Data root="BundleSamples/PlayerSample">
		<Bundle name="PlayerData">
			<Field name="Name" />
			<Field name="Experience" />
			<Field name="HP" />
			<Field name="MP" />
		</Bundle>
	</Data>
</BundleDef>
```

This describes a single "PlayerData" Bundle that contains four data fields: Name, Experience, HP, and MP. This also describes that this entity can be loaded from/saved to the file system at `{Environment.SpecialFolder.ApplicationData}/BundleSamples/PlayerSample/PlayerData.bundle`. This path can be configured, but this is what the Bundler will use as the default given this definition file.

Now that we have a definition, we can use it to create a Bundler.
```csharp
// let's assume the above XML definition is included in the file "bundledef.xml"

var bundler = new Bundler("bundledef.xml");
```
This will create a `Bundler` that contains a single `Bundle` node in its data root (`bundler.Data`), named `"PlayerData"`.

We can access it using its name:
```csharp
var playerData = bundler.Data["PlayerData"] as Bundle;

// If the bundle has been saved previously, we can populate it
// with the saved data with Load() or TryLoad()
playerData.TryLoad();
```

And now that we have our Bundle, we can access, assign, and update its `Field`s by accessing them in much the same way we accessed the Bundle:
```csharp
playerData["Name"].Value = "John Doe";
playerData["Experience"].Value = "99";
playerData["HP"].Value = "100";
playerData["MP"].Value = "25";
```

Finally, once we're finished updating field values, we can save the Bundle to the filesystem by simply calling `Save()`.
```csharp
playerData.Save();
```

Bundles also offers more complex node types to model more complex data structures, such as `Folder`s `FolderList`s, `NodeList`s, and more.

Check out the [full documentation](https://citrus-thunder.github.io/bundles/) for more information on advanced usage.
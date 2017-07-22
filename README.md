# Morphous.Api
Morphous.Api is an Orchard module containing an ApiController that returns Orchard content items as JSON. To use this module you must be using [Morphous.Orchard](https://github.com/Morphous/Morphous.Orchard), our modified fork of Orchard CMS.

For more information and examples, see this [blog post](https://tom-pratt.github.io/morphous-orchardcms-rest-webservice/).

## Installation (Visual Studio)
1. Get the [Morphous.Orchard](https://github.com/Morphous/Morphous.Orchard) fork of Orchard CMS and set up an Orchard site with database as normal.
2. Get [Morphous.Api](https://github.com/Morphous/Morphous.Api) and add it to your Orchard modules folder then include in the Visual Studio solution.

  ![Add module](https://github.com/tom-pratt/tom-pratt.github.io/blob/master/images/github/add_api_module.png)

3. Enable Morphous API in the modules section of your Orchard site's admin area.

  ![Enable module](https://github.com/tom-pratt/tom-pratt.github.io/blob/master/images/github/enable_api_mod.png)


## Usage
Request a content item by its ID using the following URL scheme and headers. `{address}/api/Contents/Item/{id}`

&nbsp;

  ![Request content item](https://github.com/tom-pratt/tom-pratt.github.io/blob/master/images/github/10_get_article2.png)


## Options

### DisplayType

Add `?displayType={value}` query string to the URL, possible values are `Detail` or `Summary`.

### Preview

Add `?version={versionNumber}` query string to the URL to preview a draft content item.

### Alternates

Morphous supports alternates which can be specified in the `Accept-Alternates` header. The options built into Morphous.Api are:
- If you don't specify this header then the default format is fairly flat but with some detail about content parts and fields.
- `flat` - returns content items in a flattened, typical DTO type format.

### Placement

Use the `BindingType` attribute in the `<Match>` element to target either JSON or HTML output separately. `"Translate"` targets the JSON output. `"Display"` targets the normal HTML output.


```
<Match BindingType="Translate">
    <Place Parts_Common_Body="-"/>
</Match>
```


```
<Match BindingType="Display">
    <Place Parts_Title="-"/>
</Match>
```



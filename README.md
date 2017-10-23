# System.Linq.Dynamic.ApiFilter

Parse query string filter to System.Linq.Dynamic. 

### filter example1:
```
someurl/someresource?filter=attributes.contacts.name~adam
```
Will return all resources that has a contact with name containing 'adam'.

### filter example2:
```
someurl/someresource?filter=attributes.contacts.name~adam;attributes.contacts.email~com
```
Will return all resources that has a contact with name containing 'adam' and email containing 'com'.

You can also write the filter like so:
```
someurl/someresource?filter=.contacts.name~adam;.contacts.email~com
```
It is important to have a dot before the searched attribute. For clarity it can be good to use som prefix before the first dot, like attributes.someptoperty or x.somepropert. But thats up to the consuming part.

### usage example:
```
// create predicate factory and filter provider
var builderFactory = new PredicateBuilderFactory();
var provider = new FilterProvider(builderFactory);

using(vad db = new SomeDbContext())
{
    var q = db.SomeTable.AsQueryable();
    // apply filter
    q = filterProvider.ApplyFilter(q, filter);
    // materialize query
    return q.ToArray();
}

```

### supported operands and there sql equivalent
 operands | sql equivalent 
--- | ---|
 : | equal
 ~ | like
 < | smaler than
 <: | smaler than or equal
 \>  | greater than
 \>: | greater than or equal
 :(x,y,z) | inclusive or (WHERE (x=x OR y=y OR z=z))
 ; | and (combines filters)

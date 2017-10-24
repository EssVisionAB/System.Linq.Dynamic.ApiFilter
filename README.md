# System.Linq.Dynamic.ApiFilter

Parse query string filter to System.Linq.Dynamic. 

### Filter example1:
```
someurl/someresource?filter=attributes.contacts.name~adam
```
Will return all resources that has a contact with name containing 'adam'.

### Filter example2:
```
someurl/someresource?filter=attributes.contacts.name~adam;attributes.contacts.email~com
```
Will return all resources that has a contact with name containing 'adam' and email containing 'com'.

You can also write the filter like so:
```
someurl/someresource?filter=.contacts.name~adam;.contacts.email~com
```
It is important to have a dot before the searched attribute. For clarity it can be good to use som prefix before the first dot, like attributes.someptoperty or x.somepropert. But thats up to the consuming part.

### Inclusive or filter example:
```
someurl/someresource?filter=attributes.name:(name1, name2, name3)
```
Will return all resource wich names are 'name1', 'name2' or 'name3'.

### Backend simple usage example:
```C#
// create predicate factory and filter provider
var builderFactory = new PredicateBuilderFactory();
var provider = new FilterProvider(builderFactory);

using(vad db = new SomeDbContext())
{
    var q = db.SomeTable.AsQueryable();
    // apply query filter
    q = provider.ApplyFilter(q, filters);
    // materialize data
    return q.ToArray();
}

```

#### The main service to use in any web app is IFilterProvider that is defined like so:
```C#
    public interface IFilterProvider
    {
        IQueryable<TEntity> ApplyFilter<TEntity>(IQueryable<TEntity> query, string filters);

        IQueryable<TEntity> ApplyFilter<TEntity>(IQueryable<TEntity> query, IEnumerable<Filter> filters);
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
 :(a,b,c) | inclusive or queal (WHERE (@0='a' OR @0='b' OR @0='c'))
 ; | and (combines filters)

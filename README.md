# System.Linq.Dynamic.ApiFilter

Parse query string filter to be used with System.Linq.Dynamic and translated to sql. 

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
It is important to have a dot before the searched attribute. For clarity it can be good to use a prefix before the first dot, like attributes.someptoperty or x.somepropert. But thats up to the consuming part.

The reason for using a leading dot and expecting a possible prefix is for furure use of prefix categories.

### Inclusive or equal filter example:
```
someurl/someresource?filter=attributes.name:(a, b, c)
```
Returns all resource wich names are 'a', 'b' or 'c'.

### Inclusive or like filter example:
```
someurl/someresource?filter=attributes.name~(a, b, c)
```
Returns all resource wich names conatins 'a', 'b' or 'c'.

### Backend simple usage example:
```C#
// create predicate factory and filter provider
var factory = new PredicateBuilderFactory();
var provider = new FilterProvider(factory);

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
#### Filter provider is extendable through PredicateBuilderFactory's method AddBuilderType: 
```C#
    public void AddBuilderType(string targetTypeName, Type builderType){...}
    public void AddBuilderType<T>(string targetTypeName){...}
```
Where targetTypeName is the fullname of the data type and builderType is an implementation of IPredicateBuilder extending the abstract PredicateBuilder<TEntity> base class.

### supported operands and there sql equivalent
 operands | sql equivalent | data types
--- | --- | ---
 : | equal | all data types
 ~ | like | string
 < | smaler than | all data types exept string
 <: | smaler than or equal | all data types exept string
 \>  | greater than | most data all exept string
 \>: | greater than or equal | all data types exept string
 :(a,b,c) | inclusive or equal. -\> .Where("field == @0 \|\| field == @1 ...") | all data types
 ~(a,b,c) | inclusive or like. -\> .Where("field.Contains(@0) \|\| field.Contains(@1)...") | string
 ; | and (combines filters)

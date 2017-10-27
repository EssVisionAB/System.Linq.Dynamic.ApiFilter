# System.Linq.Dynamic.ApiFilter

Portable core-level .NET class library. 
Parses and applies query string filters to be used on data entities or in-memory collections. 

### Filter example1:
```
someurl/someresource?filter=attributes.contacts.name~adam
```
Returns all resources that has a contact with a name containing 'adam'.

### Filter example2:
```
someurl/someresource?filter=attributes.contacts.name~adam;attributes.contacts.email~com
```
Returns all resources that has a contact with a name containing 'adam' and an email containing 'com'.

You can also write the filter like so:
```
someurl/someresource?filter=.contacts.name~adam;.contacts.email~com
```
The leading dot is mandatory before each search attribute. For clarity it can be good to use a prefix before the first dot, like 'attributes.someproperty' or 'x.someproperty', but thats up to the consuming part.

The reason for using a leading dot and expecting a possible prefix is based on an earlier requirement 
(filter on [JsonApi](http://jsonapi.org/format) attributes) and retained for future use of prefix categories.

### Inclusive or equal filter example:
```
someurl/someresource?filter=attributes.name:(a, b, c)
```
Returns all resource wich names are 'a', 'b' or 'c'.

### Inclusive or like filter example:
```
someurl/someresource?filter=attributes.name~(a, b, c)
```
Returns all resource wich names contains 'a', 'b' or 'c'.

### Backend simple usage example:
```C#
// Create the filter provider with a predicate factory
var factory = new PredicateBuilderFactory();
var provider = new FilterProvider(factory);

using(vad db = new SomeDbContext())
{
    var q = db.SomeTable.AsQueryable();
    // Apply query filters. Under the hood System.Linq.Dynamic is used
    q = provider.ApplyFilter(q, filter);
    // Materialize the data
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
Where targetTypeName is the fullname of the data type and builderType is an implementation of IPredicateBuilder extending the abstract PredicateBuilder\<TEntity> base class.

### supported operands and there sql equivalent
 operands | sql equivalent | data types
--- | --- | ---
 : | equal | all data types
 ~ | like | string
 < | smaller than | all data types exept string
 <: | smaller than or equal | all data types exept string
 \>  | greater than | all data types exept string
 \>: | greater than or equal | all data types exept string
 :(a,b...) | inclusive or equal. -\> q.Where("field == @0 \|\| field == @1 ...") | all data types
 ~(a,b...) | inclusive or like. -\> q.Where("field.Contains(@0) \|\| field.Contains(@1)...") | string
 ; | and (combines filters) | n/a

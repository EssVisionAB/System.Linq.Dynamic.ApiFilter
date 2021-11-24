# System.Linq.Dynamic.ApiFilter

Portable core-level .NET class library. 
Parses and applies query string filters to be used on data entities or in-memory collections. 

![](https://github.com/EssVisionAB/System.Linq.Dynamic.ApiFilter/workflows/.NET/badge.svg)

### Filter example1:
```
http://example.com/documents?filter=contacts.name~adam
```
Filter for document resources that has a name containing 'adam'.

### Filter example2:
```
http://example.com/documents?filter=contacts.name~adam;contacts.email~com
```
Filter for document resources that has a contact with a name containing 'adam' and an email containing 'com'.

### Special case example1
```
http://example.com/documents?filter=folderid:{guid}
```
Filter for documents directly in specified folder

### Special case example2
```
http://example.com/documents?filter=folderid~{guid}
```
Filter for documents in specified folder and its children

### Inclusive or equal filter example:
```
http://example.com/documents?filter=name:(a, b, c)
```
Filter for document resources wich names are 'a', 'b' or 'c'.

### Inclusive or like filter example:
```
http://example.com/documents?filter=name~(a, b, c)
```
Filter for document resources wich names contains 'a', 'b' or 'c'.

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
 <> | not equal | all data types
 ~ | like | string
 < | smaller than | all data types exept string
 <: | smaller than or equal | all data types exept string
 \>  | greater than | all data types exept string
 \>: | greater than or equal | all data types exept string
 :(a,b...) | inclusive or equal. -\> q.Where("field == @0 \|\| field == @1 ...") | all data types
 ~(a,b...) | inclusive or like. -\> q.Where("field.Contains(@0) \|\| field.Contains(@1)...") | string
 ; | and (combines filters) | n/a
